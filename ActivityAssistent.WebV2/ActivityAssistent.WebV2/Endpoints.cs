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
            var FullNameClaim = Jwt.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
            if (FullNameClaim != null)
            {
                Identity.AddClaim(new Claim("name", FullNameClaim));
            }
            var SubNrClaimValue = Jwt.Claims.FirstOrDefault(c => c.Type == "groupsid" || c.Type == "groups")?.Value;
            if (!string.IsNullOrEmpty(SubNrClaimValue))
            {
                // We gebruiken GroupSid als het gestandaardiseerde .NET alternatief voor een bedrijfs/groeps-ID
                Identity.AddClaim(new Claim(ClaimTypes.GroupSid, SubNrClaimValue));
            }

            // 2. Rol claim overzetten (Azure AD gebruikt vaak "roles" of "role" in het JWT)
            var RoleClaimValue = Jwt.Claims.FirstOrDefault(c => c.Type == "roles" || c.Type == "role")?.Value;
            if (!string.IsNullOrEmpty(RoleClaimValue))
            {
                Identity.AddClaim(new Claim(ClaimTypes.Role, RoleClaimValue));
            }
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