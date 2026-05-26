using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Dtos.Response;
using ActivityAssistent.WebV2.Client.Interfaces.companies;
using Microsoft.AspNetCore.Components.Authorization;

namespace ActivityAssistent.WebV2.Client.Services.Companies
{
    public class WebCompanyService(HttpClient Http, AuthenticationStateProvider AuthStateProvider) : BaseService(Http, AuthStateProvider), ICompanyService
    {
        public async Task<bool> CreateCompanyAsync(CreateCompanyDto Company, CancellationToken Token)
        {
            var Response = await PostAsync<ApiResponse<bool>>("api/Company/create", Company, Token);
            if (Response.IsSuccess)
            {
                
                if (Response is null)
                {
                    throw new HttpRequestException("The API response was empty or invalid.");
                }
                if (Response.IsSuccess)
                {
                    return Response.Data;
                }
                else 
                {
                    return Response.Data;
                }
            }
            else
            {
                throw new HttpRequestException($"Error creating company: {Response.ErrorMessage}");
            }
        }

        public Task DeleteCompanyAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto> GetCompanyByIdAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CompanyNames?>> GetCustomerAsync(CancellationToken Token)
        {
            //try
            //{
            //    // 1. Haal de token rechtstreeks op vanuit jouw actieve Blazor-sessie
            //    var AuthState = await AuthStateProvider.GetAuthenticationStateAsync();
            //    var ApiToken = AuthState.User.FindFirst("ApiToken")?.Value;

            //    // 2. Bouw het HTTP-verzoek handmatig op (in plaats van GetAsync)
            //    var Request = new HttpRequestMessage(HttpMethod.Get, "api/Company/GetCustomers");

            //    // 3. Plak de Bearer token veilig in de header
            //    if (!string.IsNullOrWhiteSpace(ApiToken))
            //    {
            //        Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);
            //    }

            //    // 4. Verstuur het verzoek
            //    var Response = await Http.SendAsync(Request, Token);
            //    Console.WriteLine($"API Status: {Response.StatusCode}");

            //    if (Response.IsSuccessStatusCode)
            //    {
            //        var ApiResult = await Response.Content.ReadFromJsonAsync<ApiResponse<List<CompanyNames>>>(cancellationToken: Token);

            //        if (ApiResult != null && ApiResult.IsSuccess && ApiResult.Data != null)
            //        {
            //            return ApiResult.Data;
            //        }

            //        Console.WriteLine($"Backend error: {ApiResult?.ErrorMessage}");
            //        return new List<CompanyNames>();
            //    }

            //    Console.WriteLine($"HTTP Error: {Response.ReasonPhrase}");
            //    return new List<CompanyNames>();
            //}
            //catch (Exception Ex)
            //{
            //    Console.WriteLine($"An error occurred while fetching customers: {Ex.Message}");
            //    return new List<CompanyNames>();
            //}
            try
            {
                var Result = await GetAsync<ApiResponse<List<CompanyNames>>>("api/Company/GetCustomers", Token);
                switch (Result)
                {
                    case null:
                        throw new HttpRequestException("The API response was empty or invalid.");
                    case { IsSuccess: true, Data: not null }:
                        return Result.Data!;
                    default:
                        Console.WriteLine($"Backend error: {Result.ErrorMessage}");
                        return new List<CompanyNames?>();
                }

            }
            catch (Exception)
            {

                return new List<CompanyNames?>();
            }


        }

        

        public Task UpdateCompanyAsync(CompanyDto Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
