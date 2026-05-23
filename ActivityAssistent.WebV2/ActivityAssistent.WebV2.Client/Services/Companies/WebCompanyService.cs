using System.Net;
using System.Net.Http.Json;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.WebV2.Client.Interfaces.companies;

namespace ActivityAssistent.WebV2.Client.Services.Companies
{
    public class WebCompanyService(HttpClient Http) : ICompanyService
    {
        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto Company, CancellationToken Token)
        {
            var Response = await Http.PostAsJsonAsync("api/Company/create", Company, Token);
            if (Response.IsSuccessStatusCode)
            {
                return await Response.Content.ReadFromJsonAsync<CompanyDto>(cancellationToken: Token);
            }
            else
            {
                throw new HttpRequestException($"Error creating company: {Response.ReasonPhrase}");
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

        public async Task<List<CustomerDto>> GetCustomerAsync(CancellationToken Token)
        {
            var Response = await Http.GetAsync("api/Company/GetCustomers", Token);
            if (Response. IsSuccessStatusCode)
            {
                return await Response.Content.ReadFromJsonAsync<List<CustomerDto>>(cancellationToken: Token);
            }
            else
            {
                throw new HttpRequestException($"Error creating company: {Response.ReasonPhrase}");
            }
        }

        public Task UpdateCompanyAsync(CompanyDto Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
