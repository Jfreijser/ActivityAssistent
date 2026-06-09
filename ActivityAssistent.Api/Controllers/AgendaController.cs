using ActivityAssistent.Api.Interfaces.Agenda;
using ActivityAssistent.Shared.Dtos.Agenda;
using ActivityAssistent.Shared.Dtos.Response;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActivityAssistent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaController(IAgendaService agendaService) : BaseApiController
    {
        [HttpGet("GetAgendaPoints")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<AgendaDto>>>> GetAgendaPoints(CancellationToken Token)
        {
            var response = await agendaService.GetAgendaPointsAsync(Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
           
        }
    }
}
