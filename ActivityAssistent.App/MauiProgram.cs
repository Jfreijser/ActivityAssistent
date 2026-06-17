using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.ActionPoint;
using ActivityAssistent.App.Interfaces.Agenda;
using ActivityAssistent.App.Interfaces.Ai;
using ActivityAssistent.App.Interfaces.Audio;
using ActivityAssistent.App.Interfaces.companies;
using ActivityAssistent.App.Interfaces.Conversations;
using ActivityAssistent.App.Interfaces.Email;
using ActivityAssistent.App.Interfaces.Identity;
using ActivityAssistent.App.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Plugin.Maui.Audio;
using Radzen;
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
            builder.Services.AddSingleton(AudioManager.Current);

            builder.Services.AddAuthorizationCore();
            builder.Services.AddMudServices();
            builder.Services.AddRadzenComponents();
            Action<HttpClient> ConfigureBackendClient = Client =>
            {
                // Gebruik 10.0.2.2 voor de Android emulator, anders localhost
#if ANDROID
                Client.BaseAddress = new Uri("https://10.0.2.2:7230");
#else
                Client.BaseAddress = new Uri("https://localhost:7230");
#endif
                Client.Timeout = TimeSpan.FromMinutes(5);
            };

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            AddActivityAssistentHttpClient<IConversationService, MauiConversationService>(builder.Services, ConfigureBackendClient);
            AddActivityAssistentHttpClient<IActionPointService, MauiActionPointService>(builder.Services, ConfigureBackendClient);
            AddActivityAssistentHttpClient<ICompanyService, MauiCompanyService>(builder.Services, ConfigureBackendClient);
            AddActivityAssistentHttpClient<IAuthService, MauiAuthService>(builder.Services, ConfigureBackendClient);
            AddActivityAssistentHttpClient<IAudioRecorderService, MauiAudioService>(builder.Services, ConfigureBackendClient);
            AddActivityAssistentHttpClient<IAiMeetingAnalyzer, MauiAiService>(builder.Services, ConfigureBackendClient);
            AddActivityAssistentHttpClient<IAgenda, MauiAgendaService>(builder.Services, ConfigureBackendClient);
            builder.Services.AddScoped<IThemeService, Services.ThemeService>();
            builder.Services.AddScoped<IEmailService, MauiEmailService>();
            return builder.Build();
        }

        private static IHttpClientBuilder AddActivityAssistentHttpClient<TClient, TImplementation>(IServiceCollection services, Action<HttpClient> configureClient)
            where TClient : class
            where TImplementation : class, TClient
        {
            var builder = services.AddHttpClient<TClient, TImplementation>(configureClient);
#if ANDROID
            builder.ConfigurePrimaryHttpMessageHandler(() => new Xamarin.Android.Net.AndroidMessageHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            });
#endif
            return builder;
        }
    }
}
