using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using ActivityAssistent.App.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace ActivityAssistent.App.Services
{
    public abstract class BaseService(HttpClient Http, CustomAuthenticationStateProvider AuthStateProvider)
    {
        private async Task<T?> SendAsyncInternal<T>(HttpMethod Method, string Url, object? Content = null, CancellationToken Token = default)
        {
            var AuthState = await AuthStateProvider.GetAuthenticationStateAsync();
            var ApiToken = AuthState.User.FindFirst("ApiToken")?.Value;

            var Request = new HttpRequestMessage(Method, Url);

            if (!string.IsNullOrWhiteSpace(ApiToken))
                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);

            if (Content != null)
                Request.Content = JsonContent.Create(Content);

            var Response = await Http.SendAsync(Request, Token);

            if (!Response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API Error: {Response.StatusCode} - {Url}");
                return default;
            }

            return await Response.Content.ReadFromJsonAsync<T>(cancellationToken: Token);
        }

        protected Task<T?> GetAsync<T>(string Url, CancellationToken Token = default) =>
            SendAsyncInternal<T>(HttpMethod.Get, Url, null, Token);

        protected Task<T?> PostAsync<T>(string Url, object Content, CancellationToken Token = default) =>
            SendAsyncInternal<T>(HttpMethod.Post, Url, Content, Token);

        protected Task<T?> DeleteAsync<T>(string Url, CancellationToken Token = default) =>
            SendAsyncInternal<T>(HttpMethod.Delete, Url, null, Token);

        protected Task<T?> PutAsync<T>(string Url, object Content, CancellationToken Token = default) =>
            SendAsyncInternal<T>(HttpMethod.Put, Url, Content, Token);
    }
}
