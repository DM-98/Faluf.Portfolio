using Faluf.Portfolio.API.Data;
using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? migrationsAssembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddDbContext<UserDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>().AddEntityFrameworkStores<UserDbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentityServer()
	.AddConfigurationStore(options => options.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"), sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
	.AddOperationalStore(options => options.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"), sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
	.AddAspNetIdentity<ApplicationUser>()
	.AddDeveloperSigningCredential();
builder.Services.AddCors();
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());
app.UseHttpsRedirection();

app.UseIdentityServer();
app.UseAuthorization();

if (args.Contains("/seed"))
{
	SeedData.EnsureSeedData(builder.Configuration.GetConnectionString("ApplicationDbContext"));
	return;
}

app.Run();