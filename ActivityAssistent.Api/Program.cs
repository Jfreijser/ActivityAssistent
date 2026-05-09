using ActivityAssistent.Api.Configuration;
using ActivityAssistent.Api.Infrastructure;
using ActivityAssistent.Api.Infrastructure.Repositories;
using ActivityAssistent.Api.Services;
using ActivityAssistent.Api.Services.Conversations;
using ActivityAssistent.Shared.Interfaces.Conversations;
using ActivityAssistent.Shared.Interfaces.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.PowerPlatform.Dataverse.Client;
var builder = WebApplication.CreateBuilder(args);

var DataverseSettings = builder.Configuration.GetSection("DataverseConfig");
builder.Services.Configure<DataverseOptions>(DataverseSettings);

//string ConnectionString = $@"
//    AuthType=ClientSecret;
//    Url={DataverseSettings["BaseUrl"]};
//    ClientId={DataverseSettings["ClientId"]};
//    ClientSecret={DataverseSettings["ClientSecret"]};";

builder.Services.AddSingleton<IOrganizationServiceAsync>(ServiceProvider =>
{
   
    var Config = ServiceProvider.GetRequiredService<IOptions<DataverseOptions>>().Value;

    var ConnectionString = $"AuthType=ClientSecret;Url={Config.BaseUrl};ClientId={Config.ClientId};ClientSecret={Config.ClientSecret};";

    return new ServiceClient(ConnectionString);
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
