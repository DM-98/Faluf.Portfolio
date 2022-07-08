using Faluf.Portfolio.API.Data;
using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Request;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using Faluf.Portfolio.Infrastructure.EFRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"/app/dataprotectionkeys")).ProtectKeysWithCertificate(new X509Certificate2(builder.Configuration["X509Cert:FilePath"], builder.Configuration["X509Cert:Password"]));
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<UserDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("UserDbContext") ?? throw new InvalidOperationException("Connectionstring 'UserDbContext' was not found.")));
builder.Services.AddDbContext<LogDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("LogDbContext") ?? throw new InvalidOperationException("Connectionstring 'LogDbContext' was not found.")));
builder.Services.AddDbContext<SubjectDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SubjectDbContext") ?? throw new InvalidOperationException("Connectionstring 'SubjectDbContext' was not found.")));
builder.Services.AddDbContext<DocumentDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DocumentDbContext") ?? throw new InvalidOperationException("Connectionstring 'DocumentDbContext' was not found.")));

builder.Services.AddTransient<IRepositoryAPI<ApplicationUser, UserModel, UserDTO>, EFRepository<ApplicationUser, UserModel, UserDTO, UserDbContext>>();
builder.Services.AddTransient<IRepositoryAPI<Log, Log, Log>, EFRepository<Log, Log, Log, LogDbContext>>();
builder.Services.AddTransient<IRepositoryAPI<Subject, SubjectModel, SubjectDTO>, EFRepository<Subject, SubjectModel, SubjectDTO, SubjectDbContext>>();
builder.Services.AddTransient<IRepositoryAPI<Document, DocumentModel, DocumentDTO>, EFRepository<Document, DocumentModel, DocumentDTO, DocumentDbContext>>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.Authority = "https://localhost:7097";
	options.Audience = "weatherapi";
	options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
});

builder.Services.AddAuthorization(options => options.AddPolicy("Read", authBuilder => authBuilder.RequireClaim("scope", "weatherapi.read")));

builder.Services.AddCors();
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();