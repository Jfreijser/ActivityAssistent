using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace ActivityAssistent.WebV2;

public class BlazorAuthorizationHandler(AuthenticationStateProvider AuthStateProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage Request, CancellationToken Token)
    {
        // Haal de live inlogstatus en claims van de huidige Blazor-sessie op
        var AuthState = await AuthStateProvider.GetAuthenticationStateAsync();
        var User = AuthState.User;

        // Vis de API-token uit de claims die we tijdens het inloggen hebben opgeslagen
        var AccessToken = User.FindFirst("ApiToken")?.Value;

        // Log naar de Visual Studio Output console om te controleren of de token aanwezig is
        Console.WriteLine($"Token gevonden in Handler: {(string.IsNullOrEmpty(AccessToken) ? "NEE, HIJ IS LEEG!" : "JA, GEVONDEN!")}");

        // Als er een token is, plakken we deze als Bearer-header op het uitgaande HTTP-verzoek
        if (!string.IsNullOrWhiteSpace(AccessToken))
        {
            Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        }

        // Stuur het verzoek met de eventuele header door naar de API
        return await base.SendAsync(Request, Token);
    }
}