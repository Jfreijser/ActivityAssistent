using ActivityAssistent.Shared.Interfaces.Identity;
using ActivityAssistent.WebV2.Client.Pages;
using ActivityAssistent.WebV2.Client.Services;
using ActivityAssistent.WebV2.Client.Services.AuthService;
using ActivityAssistent.WebV2.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
//services
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7230") });
builder.Services.AddMudServices();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<IAuthService, WebAuthService>();
builder.Services.AddScoped<ErrorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ActivityAssistent.WebV2.Client._Imports).Assembly);

app.Run();
