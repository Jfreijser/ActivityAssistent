using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Shared.Dtos.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityAssistent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController(ICompanyService companyService) : BaseApiController
    {
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

        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomerAsync(CancellationToken Token)
        {
            try
            {
                var result = await companyService.GetCustomerNamesAsync(Token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching customers: {ex}");
                return BadRequest(ex);
            }
        }

    }
}
