using Faluf.Portfolio.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Faluf.Portfolio.API.Data;

public class UserDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
	}
}