using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MySystem.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MySystem
{
	public static class MySystemExtensios
	{
		public static IServiceCollection MySystemExtensionsMethods(this IServiceCollection services)
		{
			services.AddTransient<IEmailSenderAsync, MimeKitEmailService>();
			return services;
		}
	}
}
