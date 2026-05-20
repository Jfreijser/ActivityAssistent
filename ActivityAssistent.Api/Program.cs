using System.Configuration;
using System.Net.NetworkInformation;
using ActivityAssistent.Api.Configuration;
using ActivityAssistent.Api.Infrastructure;
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
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ActivityAssistent.Api.Infrastructure.Repositories.DataverseRepository;
using ActivityAssistent.Shared.Interfaces.companies;
using ActivityAssistent.Api.Services.Companies;

var builder = WebApplication.CreateBuilder(args);

var DataverseSettings = builder.Configuration.GetSection("DataverseConfig");
builder.Services.Configure<DataverseOptions>(DataverseSettings);

builder.Services.AddScoped<IOrganizationServiceAsync>(ServiceProvider =>
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

    
    return serviceClient;


});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

var JwtSettings = builder.Configuration.GetSection("Jwt");
var Key = Encoding.ASCII.GetBytes(JwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = JwtSettings["Issuer"],
        ValidAudience = JwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key)
    };
});


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
builder.Services.AddScoped<ICompanyRepository, DataverseCompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserRepository, DataverseUserRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

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
