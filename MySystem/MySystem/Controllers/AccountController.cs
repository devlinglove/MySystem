using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySystem.Domain;
using MySystem.DTOs;
using MySystem.Infrastructure;
using MySystem.Models;
using MySystem.Services;
using MySystemWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace MySystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IOptions<AuthConfiguration> _authOptions;
		private readonly IEmailSenderAsync _emailSender;
		private readonly IOptions<EmailConfiguration> _emailOptions;

		public AccountController(
				UserManager<ApplicationUser> userManager,
				SignInManager<ApplicationUser> signInManager,
				RoleManager<IdentityRole> roleManager,
				IOptions<AuthConfiguration> authOptions,
				IEmailSenderAsync emailSender,
				IOptions<EmailConfiguration> emailOptions
			)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_authOptions = authOptions;
			_emailSender = emailSender;
			_emailOptions = emailOptions;
		}

		[HttpPost("add-role")]
		public async Task<IActionResult> AddRole([FromBody] string roleName)
		{
			var adminRole = new Role { Name = "Admin" };
			var authorRole = new Role { Name = "Author" };
			var userRole = new Role { Name = "User" };

			await _roleManager.CreateAsync(adminRole);
			await _roleManager.CreateAsync(authorRole);
			await _roleManager.CreateAsync(userRole);


			return Ok("Role created successfully");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			if (user is null)
			{
				return NotFound("User not found");
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
			if (!result.Succeeded)
			{
				return Unauthorized();
			}

			var token = GenerateJwtToken(user, _authOptions.Value);
			return Ok(new { Token = token });
		}

		[HttpPost("forgot-password")]
		public async Task<ActionResult<UserViewModel>> ForgotPassword([FromBody] ForgotPasswordModel model)
		{
			IdentityUser? user  = await _userManager.FindByEmailAsync(model.Email);
			if (user is null)
			{
				return NotFound("User not found");
			}
			
			UserViewModel userViewModel = new UserViewModel
			{
				Id = user.Id,
				Email = user.Email
			};

			return Ok(userViewModel);
		}



		[HttpPost("register")]
		public async Task<IActionResult> AddUser([FromBody] RegisterInputModel model)
		{
			var isUserExists = await _userManager.Users.AnyAsync(x => x.Email == model.Email);
			if (isUserExists)
			{
				return BadRequest($"An existing account is using {model.Email}, email addres. Please try with another email address");
			}

			var userToAdd = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				FullName = model.FullName
			};

			var result = await _userManager.CreateAsync(userToAdd, model.Password);
			if (!result.Succeeded) return BadRequest(result.Errors);

			await _userManager.AddToRoleAsync(userToAdd, "User");

			await SendConfirmationLinkEmail(userToAdd);
			return Ok(new RegisterViewModel(model));
		}

		[HttpPost("confirm-email")]
		public async Task<IActionResult> ConfirmationEmail([FromBody] ConfirmEmailModel model)
		{
			if (string.IsNullOrWhiteSpace(model.EmailAddress) || string.IsNullOrWhiteSpace(model.Token))
			{
				return BadRequest("Email address cannot be empty");
			}

			ApplicationUser? user = await _userManager.FindByEmailAsync(model.EmailAddress);
			if (user == null)
			{
				return BadRequest($"No user found for email {model.EmailAddress}");
			}

			IdentityResult result = await _userManager.ConfirmEmailAsync(user, model.Token);
			if (!result.Succeeded)
			{
				Console.WriteLine("IdentityResultErrors", result.Errors);
				return BadRequest(result.Errors.Select(e => e.Description).ToList());
			}

			return Ok("Your email has been confirmed successfully");
		}

		[HttpGet("resend-confirmation")]
		public async Task<IActionResult> ResendConfirmationLink([FromQuery] string emailAddress)
		{
			if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrWhiteSpace(emailAddress))
			{
				return BadRequest("Email address cannot be empty");
			}

			ApplicationUser? result = await _userManager.FindByEmailAsync(emailAddress);
			if (result == null)
			{
				return BadRequest($"No user found for email {emailAddress}");
			}

			await SendConfirmationLinkEmail(result);
			return Ok("Email sent successfully");
		}

		private async Task SendConfirmationLinkEmail(ApplicationUser user)
		{
			string userId = await _userManager.GetUserIdAsync(user);
			string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			EmailConfiguration emailOptions = _emailOptions.Value;
			AuthConfiguration authOptions = _authOptions.Value;
			var url = $"{authOptions.ClientUrl}/{emailOptions.ConfirmEmailPath}?token={confirmationToken}&email={user.Email}";

			var body = $"<h1>Hello world!</h1><p>Hello: {user.FullName}</p>" +
				"<p>Please confirm your email address by clicking on the following link.</p>" +
				$"<p><a href=\"{url}\">Click here</a></p>" +
				"<p>Thank you,</p>" +
				$"<br>{emailOptions.ApplicationName}";

			var emailContent = new EmailContent
			{
				From = emailOptions.From,
				To = user.Email!,
				Subject = "Account Created",
				Body = body
			};


			await _emailSender.SendConfirmationLinkAsync(emailContent);
		}


		private string GenerateJwtToken(ApplicationUser user, AuthConfiguration authConfiguration)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.Key));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
				new Claim("userid", user.Id)
			};

			var token = new JwtSecurityToken(
				issuer: authConfiguration.Issuer,
				audience: authConfiguration.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		internal string DecodeToken(string token) => Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

		internal string EncodeToken(string token) => WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

	}
}
