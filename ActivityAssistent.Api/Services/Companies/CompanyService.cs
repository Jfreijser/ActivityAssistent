using ActivityAssistent.Api.Infrastructure;
using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Api.Mappings;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.Api.Services.Companies
{
    public class CompanyService (ICompanyRepository CompanyRepository, IUserContext UserContext) : ICompanyService
    {
        public async Task<ApiResponse<bool>> CreateCompanyAsync(CreateCompanyDto CompanyDto, CancellationToken Token)
        {
            try
            {
                var Exist =  await CompanyRepository.GetByNameAsync(CompanyDto.Name, UserContext.CurrentUserId, Token);
                if (Exist.Any())
                {
                    return Fail(false, "Company with the same name already exists.");
                }

                CompanyDto.OwnerUserId = UserContext.CurrentUserId;
            
                var Id = await CompanyRepository.CreateAsync(CompanyDto, Token);

                return Id != Guid.Empty
                    ? Ok(true)
                    : Fail(false, "Something went wrong while creating the company.");
            }
            catch (Exception ex)
            {
                return Fail(false, ex.Message);
            }
        }

        public Task<ApiResponse<bool>> DeleteCompanyAsync(Guid CompanyId, CancellationToken Token)
        {
            return System.Threading.Tasks.Task.FromResult(Fail(false, "Delete company is not implemented yet."));
        }

        public Task<ApiResponse<List<CompanyDto>>> GetAllCompaniesAsync(CancellationToken Token)
        {
            return System.Threading.Tasks.Task.FromResult(Fail(new List<CompanyDto>(), "Get all companies is not implemented yet."));
        }

        public async Task<ApiResponse<CompanyDto>> GetCompanyByIdAsync(Guid CompanyId, CancellationToken Token)
        {
            try
            {
                var company = await CompanyRepository.GetByIdAsync(CompanyId, Token);
                return company.CompanyId == Guid.Empty
                    ? Fail(new CompanyDto(), "Company not found.")
                    : Ok(company);
            }
            catch (Exception ex)
            {
                return Fail(new CompanyDto(), ex.Message);
            }
        }

        public  async Task<ApiResponse<List<CompanyNames>>> GetCompanyNamesAsync(CancellationToken Token)
        {
            try
            {

                var Customers = await CompanyRepository.GetCompanyNamesAsync(UserContext.CurrentUserId, Token);
                return Ok(Customers);

            }
            catch (Exception ex )
            {
                return Fail(new List<CompanyNames>(), ex.Message);
            }
        }

        public async Task<ApiResponse<List<CompanyOverviewDto>>> GetCompanyOverviewAsync(CancellationToken Token)
        {
            try
            {
                var Overview = await CompanyRepository.GetCompanyOverviewAsync(UserContext.CurrentUserId, Token);
                return Ok(Overview);
            }
            catch (Exception ex)
            {
                return Fail(new List<CompanyOverviewDto>(), ex.Message);
            }
        }

        public Task<ApiResponse<bool>> UpdateCompanyAsync(CompanyDto Company, CancellationToken Token)
        {
            return System.Threading.Tasks.Task.FromResult(Fail(false, "Update company is not implemented yet."));
        }

        private static ApiResponse<T> Ok<T>(T data)
            => new() { IsSuccess = true, Data = data, ErrorMessage = string.Empty };

        private static ApiResponse<T> Fail<T>(T data, string message)
            => new() { IsSuccess = false, Data = data, ErrorMessage = message };
    }
}
