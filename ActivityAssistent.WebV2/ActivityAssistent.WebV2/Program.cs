using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.WebV2;
using ActivityAssistent.WebV2.Client.Interfaces.companies;
using ActivityAssistent.WebV2.Client.Interfaces.Conversations;
using ActivityAssistent.WebV2.Client.Interfaces.Identity;
using ActivityAssistent.WebV2.Client.Services;
using ActivityAssistent.WebV2.Client.Services.AuthService;
using ActivityAssistent.WebV2.Client.Services.Companies;
using ActivityAssistent.WebV2.Client.Services.ConversationService;
using ActivityAssistent.WebV2.Components;
using ActivityAssistent.WebV2.Endpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var Builder = WebApplication.CreateBuilder(args);

Builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

Builder.Services.AddCascadingAuthenticationState();
Builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(Options =>
{
    Options.LoginPath = "/login";
});
Builder.Services.AddAuthorization();

Action<HttpClient> ConfigureBackendClient = Client =>
{
    Client.BaseAddress = new Uri("https://localhost:7230");
};

Builder.Services.AddHttpClient<IAuthService, WebAuthService>(ConfigureBackendClient);
Builder.Services.AddHttpClient<IConversationService, ConversationService>(ConfigureBackendClient);
Builder.Services.AddHttpClient<ICompanyService, WebCompanyService>(ConfigureBackendClient);
Builder.Services.AddHttpClient("ApiClient", ConfigureBackendClient);


Builder.Services.AddHttpContextAccessor();

Builder.Services.AddMudServices();
Builder.Services.AddScoped<IThemeService, ThemeService>();
Builder.Services.AddScoped<ErrorService>();

var WebApp = Builder.Build();

WebApp.UseStaticFiles();
WebApp.UseAntiforgery();

WebApp.UseAuthentication();
WebApp.UseAuthorization();

WebApp.MapRazorComponents<App>()
      .AddInteractiveServerRenderMode()
      .AddAdditionalAssemblies(typeof(ActivityAssistent.WebV2.Client._Imports).Assembly);

WebApp.MapAuthEndpoints();


WebApp.Run();