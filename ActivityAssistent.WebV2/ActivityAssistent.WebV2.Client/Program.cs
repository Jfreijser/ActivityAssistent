using ActivityAssistent.Shared.Interfaces.Identity;
using ActivityAssistent.WebV2.Client.Services;
using ActivityAssistent.WebV2.Client.Services.AuthService;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7230") });

builder.Services.AddMudServices();

// Registreer eigen services
builder.Services.AddScoped<IThemeService, ThemeService>();

builder.Services.AddScoped<IAuthService,WebAuthService>();
builder.Services.AddScoped<ErrorService>();

await builder.Build().RunAsync();
