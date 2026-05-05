using MudBlazor.Services;
using ActivityAssistent.Web.Services;
using ActivityAssistent.Web.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ActivityAssistent.Shared.Interfaces.Identity;
using ActivityAssistent.Web.Services.AuthService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
});
builder.Services.AddHttpClient<IAuthService, WebAuthService>(Client =>
{
    Client.BaseAddress = new Uri("https://localhost:7123/");
});

// MudBlazor Services
builder.Services.AddMudServices();

// Theme Service
builder.Services.AddScoped<IThemeService, ThemeService>();

builder.Services.AddScoped<IErrorService, ErrorService>();

await builder.Build().RunAsync();
