using Faluf.Portfolio.Blazor;
using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.Interfaces;
using Faluf.Portfolio.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<IRepository<ApplicationUser>, UserService>();
builder.Services.AddTransient<IRepository<Subject>, SubjectService>();
builder.Services.AddTransient<IRepository<Document>, DocumentService>();
builder.Services.AddTransient<IRepository<Log>, LogService>();
builder.Services.AddTransient<ILocalStorageService, LocalStorageService>();

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();