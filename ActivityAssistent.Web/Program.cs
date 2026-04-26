using MudBlazor.Services;
using ActivityAssistent.Web.Services;
using ActivityAssistent.Web.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
});

// MudBlazor Services
builder.Services.AddMudServices();

// Theme Service
builder.Services.AddScoped<IThemeService, ThemeService>();

builder.Services.AddScoped<IErrorService, ErrorService>();

await builder.Build().RunAsync();
