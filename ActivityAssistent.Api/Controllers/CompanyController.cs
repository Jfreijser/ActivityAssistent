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
            var response = await companyService.CreateCompanyAsync(Company, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
           
        }
        [Authorize]
        [HttpGet("GetCompanyNames")]
        public async Task<ActionResult<ApiResponse<List<CompanyNames>>>> GetCustomerAsync(CancellationToken Token)
        {
            var response = await companyService.GetCompanyNamesAsync(Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpGet("GetCompanyOverview")]
        public async Task<ActionResult<ApiResponse<List<CompanyOverviewDto>>>> GetCompanyOverviewAsync(CancellationToken Token)
        {
            var response = await companyService.GetCompanyOverviewAsync(Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

    }
}
