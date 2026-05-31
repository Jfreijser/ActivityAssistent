using System.Configuration;
using System.Net.NetworkInformation;
using System.Text;
using ActivityAssistent.Api.Configuration;
using ActivityAssistent.Api.Infrastructure;
using ActivityAssistent.Api.Infrastructure.Repositories.DapperRepository;
using ActivityAssistent.Api.Infrastructure.Repositories.DataverseRepository;
using ActivityAssistent.Api.Interfaces.ActionPoint;
using ActivityAssistent.Api.Interfaces.Ai;
using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Api.Interfaces.Conversations;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Api.Services;
using ActivityAssistent.Api.Services.ActionPoints;
using ActivityAssistent.Api.Services.Companies;
using ActivityAssistent.Api.Services.Conversations;
using ActivityAssistent.Api.Services.AI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ActivityAssistent.Api.Interfaces.Status;

var builder = WebApplication.CreateBuilder(args);

//var DataverseSettings = builder.Configuration.GetSection("DataverseConfig");
builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Options =>
    {
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "ZetHierEenFallbackSleutelVoorDeZekerheid123!"))
        };
    });

builder.Services.AddAuthorization();


string MyAllowSpecificOrigins = "MyCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7142", "https://localhost:7000") // Voeg hier de URL van je frontend toe
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IActionPointRepository, ActionPointRepository>();
builder.Services.AddScoped<IActionPointService, ActionPointService>();
builder.Services.AddScoped<IAiMeetingAnalyzerService, AIMeetingAnalyzerService>();
builder.Services.AddScoped<ISpeechRecognitionService, SpeechRecognitionService>();
builder.Services.AddScoped<IAiStatusRepository, AiStatusRepository>();
//builder.Services.AddScoped<ICompanyRepository, DataverseCompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddHttpClient<ISpeechRecognitionService, SpeechRecognitionService>();
builder.Services.AddHttpClient<IAiAssistantService, AiActionPointService>();
//builder.Services.AddScoped<IUserRepository, DataverseUserRepository>();
//builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
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
