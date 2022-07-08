using Faluf.Portfolio.Blazor;
using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Request;
using Faluf.Portfolio.Core.DTOs.Response;
using Faluf.Portfolio.Core.Interfaces;
using Faluf.Portfolio.Infrastructure.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<IRepository<ApplicationUser, UserModel, UserDTO>, UserService>();
builder.Services.AddTransient<IRepository<Subject, SubjectModel, SubjectDTO>, SubjectService>();
builder.Services.AddTransient<IRepository<Document, DocumentModel, DocumentDTO>, DocumentService>();
builder.Services.AddTransient<IRepository<Log, Log, Log>, LogService>();

builder.Services.AddTransient<ILocalStorageService, LocalStorageService>();
builder.Services.AddTransient<IAuthenticationService<TokenResponse>, AuthenticationService>();

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7059") });

//builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();