﻿namespace MySystem.DTOs
{
	public class RegisterUserDto
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
	}
	//public record RegisterDto(string Email, string Password);
}
