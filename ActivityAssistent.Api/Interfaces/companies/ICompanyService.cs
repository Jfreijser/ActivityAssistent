using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.Api.Interfaces.companies
{
    public interface ICompanyService
    {
        Task<ApiResponse<CompanyDto>> GetCompanyByIdAsync(Guid CompanyId, CancellationToken Token);
        Task<ApiResponse<List<CompanyDto>>> GetAllCompaniesAsync(CancellationToken Token);
        Task<ApiResponse<bool>> UpdateCompanyAsync(CompanyDto Company, CancellationToken Token);
        Task<ApiResponse<bool>> DeleteCompanyAsync(Guid CompanyId, CancellationToken Token);
        Task<ApiResponse<bool>> CreateCompanyAsync(CreateCompanyDto Company, CancellationToken Token);
        Task<ApiResponse<List<CompanyNames>>> GetCompanyNamesAsync(CancellationToken Token);
        Task<ApiResponse<List<CompanyOverviewDto>>> GetCompanyOverviewAsync(CancellationToken Token);

    }
}
