using System.Configuration;
using ActivityAssistent.Api.Configuration;
using ActivityAssistent.Api.Infrastructure;
using ActivityAssistent.Api.Infrastructure.Repositories;
using ActivityAssistent.Api.Services;
using ActivityAssistent.Api.Services.Conversations;
using ActivityAssistent.Shared.Interfaces.Conversations;
using ActivityAssistent.Shared.Interfaces.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.ReportingServices.ReportProcessing.OnDemandReportObjectModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
var builder = WebApplication.CreateBuilder(args);

var DataverseSettings = builder.Configuration.GetSection("DataverseConfig");
builder.Services.Configure<DataverseOptions>(DataverseSettings);

builder.Services.AddSingleton<IOrganizationServiceAsync>(ServiceProvider =>
{
    var Config = ServiceProvider.GetRequiredService<IOptions<DataverseOptions>>().Value;

    
    Console.WriteLine($"BaseUrl: {Config.BaseUrl}");
    Console.WriteLine($"ClientId: {Config.ClientId}");
    Console.WriteLine($"ClientSecret: {(string.IsNullOrEmpty(Config.ClientSecret) ? "MISSING" : "PRESENT")}");
    Console.WriteLine($"TenantId: {Config.TenantId}");

    string ClientId = $"{Config.ClientId}";
    string TenantId = $"{Config.TenantId}";
    string ClientSecret = $"{Config.ClientSecret}";
    // Voor Microsoft Graph is dit bijvoorbeeld: https://graph.microsoft.com/.default
    string resourceUrl = Config.BaseUrl.EndsWith("/") ? Config.BaseUrl : Config.BaseUrl + "/";
    string[] Scopes = new[] { $"{resourceUrl}.default" };

    // 1. Initialiseer de Confidential Client
    IConfidentialClientApplication ConfidentialApp = ConfidentialClientApplicationBuilder.Create(ClientId)
        .WithTenantId(TenantId)
        .WithClientSecret(ClientSecret)
        .Build();
    ServiceClient serviceClient;
    try
    {

        // 4. Maak de ServiceClient aan met de externe Token Provider
        // De lambda-functie haalt (indien nodig) live het token op uit de MSAL cache
         serviceClient = new ServiceClient(
            tokenProviderFunction: async (string crmUrl) =>
            {
                var authResult = await ConfidentialApp.AcquireTokenForClient(Scopes).ExecuteAsync();
                return authResult.AccessToken;
            },
            instanceUrl: new Uri(Config.BaseUrl)
        );

    }
    catch (Exception ex)
    {
        Console.WriteLine("DATAVERSE ERROR: " + ex.Message);
        if (ex.InnerException != null) Console.WriteLine($"Inner: {ex.InnerException.Message}");
        throw;
    }

    if (serviceClient.IsReady)
    {
        Console.WriteLine("Succesvol verbonden met Dataverse!");
        IOrganizationServiceAsync service = (IOrganizationServiceAsync)serviceClient;

        // Voorbeeld aanroep: Haal de naam van de eerste 5 accounts op
        QueryExpression query = new QueryExpression("account") { ColumnSet = new ColumnSet("name") };
        EntityCollection results = Task.Run(() => service.RetrieveMultipleAsync(query)).Result;

        foreach (var entity in results.Entities)
        {
            Console.WriteLine($"Account Naam: {entity.GetAttributeValue<string>("name")}");
        }
    }
    else
    {
        Console.WriteLine($"Verbinding mislukt: {serviceClient.LastError}");
    }
    return serviceClient;


});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
string MyAllowSpecificOrigins = "MyCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7142", "https://localhost:7000") // Voeg hier de URL van je frontend toe
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IUserRepository, DataverseUserRepository>();
builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
