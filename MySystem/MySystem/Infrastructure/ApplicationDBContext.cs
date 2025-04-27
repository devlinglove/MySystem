using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySystem.Domain;

namespace MySystem.Infrastructure
{
	public class ApplicationDBContext:IdentityDbContext
	{
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
		{
			
		}
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			// Do any changes to the identity models here.
		}
	}
}
