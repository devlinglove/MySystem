using Microsoft.AspNetCore.Identity;

namespace MySystem.Domain
{
	public class ApplicationUser: IdentityUser
	{
		public string? FullName { get; set; }
	}
}
