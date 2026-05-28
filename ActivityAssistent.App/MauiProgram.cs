using ActivityAssistent.App.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace ActivityAssistent.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped<CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(ServiceProvider =>
                ServiceProvider.GetRequiredService<CustomAuthenticationStateProvider>());

            builder.Services.AddAuthorizationCore();
            Action<HttpClient> ConfigureBackendClient = Client =>
            {
                // Vergeet niet dat localhost voor een Android emulator straks 10.0.2.2 wordt
                Client.BaseAddress = new Uri("https://localhost:7230");
            };

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            //builder.Services.AddHttpClient<IConversationService, MauiConversationService>(ConfigureBackendClient);
            
            return builder.Build();
        }
    }
}
