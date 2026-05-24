using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ActivityAssistent.Shared.Dtos.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ActivityAssistent.WebV2.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder App)
    {
        // Let op de [FromForm]!
        App.MapPost("/auth/login", async ([FromForm] LoginCredentialsDto Credentials, IHttpClientFactory HttpClientFactory, HttpContext Context) =>
        {
            var Client = HttpClientFactory.CreateClient("ApiClient");
            var Response = await Client.PostAsJsonAsync("api/auth/login", Credentials);

            if (!Response.IsSuccessStatusCode) return Results.Redirect("/login?error=InvalidCredentials");

            var Result = await Response.Content.ReadFromJsonAsync<AuthResultDto>();
            if (Result is null || !Result.IsSuccess) return Results.Redirect("/login?error=InvalidCredentials");

            var Jwt = new JwtSecurityTokenHandler().ReadJwtToken(Result.AccessToken);
            var Identity = new ClaimsIdentity(Jwt.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            Identity.AddClaim(new Claim("ApiToken", Result.AccessToken));
            var Principal = new ClaimsPrincipal(Identity);

            var AuthProperties = new AuthenticationProperties { IsPersistent = true };
            AuthProperties.StoreTokens(new[]
            {
                new AuthenticationToken { Name = "access_token", Value = Result.AccessToken }
            });

            await Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal, AuthProperties);

            // Stuur de gebruiker na succesvol inloggen door naar de beveiligde omgeving
            return Results.Redirect("/");
        });

        App.MapPost("/auth/logout", async (HttpContext Context) =>
        {
            // Verwijder de cookie
            await Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Stuur de gebruiker terug naar de login pagina
            return Results.Redirect("/login");
        });
    }
}