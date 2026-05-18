using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Interfaces.companies;
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

    }
}
