using ActivityAssistent.Shared.Interfaces.Conversations;
using ActivityAssistent.Shared.Interfaces.Identity;
using ActivityAssistent.WebV2.Client.Services;
using ActivityAssistent.WebV2.Client.Services.AuthService;
using ActivityAssistent.WebV2.Client.Services.ConversationService;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 1. Registreer de handler
builder.Services.AddTransient<BlazorAuthorizationHandler>();

Action<HttpClient> ConfigureBackendClient = Client =>
{
    Client.BaseAddress = new Uri("https://localhost:7230");
};


builder.Services.AddHttpClient<IAuthService, WebAuthService>(ConfigureBackendClient)
    .AddHttpMessageHandler<BlazorAuthorizationHandler>();

builder.Services.AddHttpClient<IConversationService, ConversationService>(ConfigureBackendClient)
    .AddHttpMessageHandler<BlazorAuthorizationHandler>();

// 3. Herstel de standaard, naamloze HttpClient voor het framework zelf (Poort 7142 voor lokale assets)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// 4. Registreer overige benodigdheden
builder.Services.AddMudServices();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<ErrorService>();

await builder.Build().RunAsync();