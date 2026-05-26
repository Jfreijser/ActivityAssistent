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
        public async Task<ActionResult<ApiResponse<bool>>> CreateCompanyAsync(CreateCompanyDto Company, CancellationToken Token)
        {
            try
            {
                var result = await companyService.CreateCompanyAsync(Company, Token);
                if (result)
                {
                    var SuccessResult = new ApiResponse<bool>
                    {
                        IsSuccess = true,
                        Data = result,
                        ErrorMessage = string.Empty
                    };
                    return Ok(SuccessResult);

                }
                else
                {
                    var SuccessResult = new ApiResponse<bool>
                    {
                        IsSuccess = false,
                        Data = result,
                        ErrorMessage = "Something went wrong while creating the company."
                    };
                    return Ok(SuccessResult);
                }
                
            }
            catch (Exception ex )
            {

                Console.WriteLine($"An Error happend while creating a Company: {ex}");
                var SuccessResult = new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    ErrorMessage = "Something went wrong while creating the company."
                };
                return BadRequest(SuccessResult);
            }
           
        }
        [Authorize]
        [HttpGet("GetCustomers")]
        public async Task<ActionResult<ApiResponse<List<CompanyNames>>>> GetCustomerAsync(CancellationToken Token)
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
                return BadRequest(SuccessResult);
            }
        }

    }
}
