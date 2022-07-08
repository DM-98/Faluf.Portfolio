using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.EntityFramework.Storage;
using Faluf.Portfolio.API.Data;
using Faluf.Portfolio.Core.Domain;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Faluf.Portfolio.IdentityServer;

public class SeedData
{
	public static void EnsureSeedData(string connectionString)
	{
		ServiceCollection services = new();

		services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));
		services.AddIdentity<ApplicationUser, IdentityRole<int>>().AddEntityFrameworkStores<UserDbContext>().AddDefaultTokenProviders();
		services.AddOperationalDbContext(options => options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)));
		services.AddConfigurationDbContext(options => options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)));

		ServiceProvider serviceProvider = services.BuildServiceProvider();

		using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
		{
			scope.ServiceProvider.GetService<PersistedGrantDbContext>()?.Database.Migrate();

			ConfigurationDbContext context = scope.ServiceProvider.GetService<ConfigurationDbContext>()!;
			context.Database.Migrate();

			EnsureSeedData(context);

			UserDbContext ctx = scope.ServiceProvider.GetService<UserDbContext>()!;
			ctx.Database.Migrate();

			EnsureUsers(scope);
		}
	}

	private static void EnsureUsers(IServiceScope scope)
	{
		UserManager<ApplicationUser> userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
		ApplicationUser alice = userMgr.FindByNameAsync("alice").Result;

		if (alice == null)
		{
			alice = new ApplicationUser
			{
				IPAddress = "0.0.0.0",
				UserName = "alice",
				Email = "AliceSmith@email.com",
				EmailConfirmed = true,
			};

			IdentityResult result = userMgr.CreateAsync(alice, "Pass123$").Result;

			if (!result.Succeeded)
			{
				throw new Exception(result.Errors.First().Description);
			}

			result = userMgr.AddClaimsAsync(alice, new Claim[]
			{
				new Claim(JwtClaimTypes.Name, "Alice Smith"),
				new Claim(JwtClaimTypes.GivenName, "Alice"),
				new Claim(JwtClaimTypes.FamilyName, "Smith"),
				new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
			}).Result;

			if (!result.Succeeded)
			{
				throw new Exception(result.Errors.First().Description);
			}
		}

		ApplicationUser bob = userMgr.FindByNameAsync("bob").Result;

		if (bob == null)
		{
			bob = new ApplicationUser
			{
				IPAddress = "1.1.1.1",
				UserName = "bob",
				Email = "BobSmith@email.com",
				EmailConfirmed = true
			};

			IdentityResult result = userMgr.CreateAsync(bob, "Pass123$").Result;

			if (!result.Succeeded)
			{
				throw new Exception(result.Errors.First().Description);
			}

			result = userMgr.AddClaimsAsync(bob, new Claim[]
			{
				new Claim(JwtClaimTypes.Name, "Bob Smith"),
				new Claim(JwtClaimTypes.GivenName, "Bob"),
				new Claim(JwtClaimTypes.FamilyName, "Smith"),
				new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
				new Claim("location", "somewhere")
			}).Result;

			if (!result.Succeeded)
			{
				throw new Exception(result.Errors.First().Description);
			}
		}
	}

	private static void EnsureSeedData(ConfigurationDbContext context)
	{
		if (!context.Clients.Any())
		{
			foreach (var client in Config.Clients.ToList())
			{
				context.Clients.Add(client.ToEntity());
			}

			context.SaveChanges();
		}

		if (!context.IdentityResources.Any())
		{
			foreach (var resource in Config.IdentityResources.ToList())
			{
				context.IdentityResources.Add(resource.ToEntity());
			}

			context.SaveChanges();
		}

		if (!context.ApiScopes.Any())
		{
			foreach (var resource in Config.ApiScopes.ToList())
			{
				context.ApiScopes.Add(resource.ToEntity());
			}

			context.SaveChanges();
		}

		if (!context.ApiResources.Any())
		{
			foreach (var resource in Config.ApiResources.ToList())
			{
				context.ApiResources.Add(resource.ToEntity());
			}

			context.SaveChanges();
		}
	}
}