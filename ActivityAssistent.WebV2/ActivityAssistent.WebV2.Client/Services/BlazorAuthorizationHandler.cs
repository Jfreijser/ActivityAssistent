using System.Net.Http.Headers;
using Microsoft.JSInterop;


namespace ActivityAssistent.WebV2.Client.Services;

public class BlazorAuthorizationHandler(IJSRuntime JsRuntime) : DelegatingHandler
{
    private const string TokenKey = "AuthToken";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage Request, CancellationToken CancellationToken)
    {
        try
        {
            // Probeer de token op te halen. Faalt dit (bijv. tijdens prerendering), dan gaan we naar de catch.
            var SavedToken = await JsRuntime.InvokeAsync<string>("localStorage.getItem", CancellationToken, TokenKey);

            if (!string.IsNullOrEmpty(SavedToken))
            {
                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SavedToken);
            }
        }
        catch (Exception ex )
        {
            Console.WriteLine($"[AuthHandler] Token kon nog niet worden geladen: {ex.Message}");
        }

        return await base.SendAsync(Request, CancellationToken);
    }
}