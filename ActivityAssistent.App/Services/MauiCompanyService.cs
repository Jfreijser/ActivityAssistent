using ActivityAssistent.App.Auth;
using ActivityAssistent.App.Interfaces.companies;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.App.Services
{
    public class MauiCompanyService(HttpClient Http, CustomAuthenticationStateProvider authStateProvider) : BaseService(Http, authStateProvider), ICompanyService
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

        public async Task<List<CompanyNames>> GetCustomerAsync(CancellationToken Token)
        {
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
                        return new List<CompanyNames>();
                }

            }
            catch (Exception)
            {

                return new List<CompanyNames>();
            }
        }

        public Task UpdateCompanyAsync(CompanyDto Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
