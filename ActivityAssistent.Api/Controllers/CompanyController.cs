using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Dtos.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityAssistent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CompanyController(ICompanyService companyService) : BaseApiController
    {
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCompanyAsync(CreateCompanyDto Company, CancellationToken Token)
        {
            try
            {
                var result = await companyService.CreateCompanyAsync(Company, Token);
                return Ok(result);
            }
            catch (Exception ex )
            {

                Console.WriteLine($"An Error happend while creating a Company: {ex}");
                return BadRequest(ex);
            }
            
        }
        [Authorize]
        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomerAsync(CancellationToken Token)
        {
            try
            {
                var result = await companyService.GetCustomerNamesAsync(Token);
                if (result.Any())
                {
                    var SuccessResult = new ApiResponse<List<CompanyNames>>
                    {
                        IsSuccess = true,
                        Data = result,
                        ErrorMessage = string.Empty
                    };
                    return Ok(SuccessResult);
                }
                else
                {
                    return Ok(new ApiResponse<List<CompanyNames>>
                    {
                        IsSuccess = true,
                        Data = new List<CompanyNames>(),
                        ErrorMessage = "No customers found."
                    });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching customers: {ex}");
                var SuccessResult = new ApiResponse<List<CompanyNames>>
                {
                    IsSuccess = false,
                    Data = new List<CompanyNames>(),
                    ErrorMessage = "An error occurred while fetching customers."
                };
                return Ok(SuccessResult);
            }
        }

    }
}
