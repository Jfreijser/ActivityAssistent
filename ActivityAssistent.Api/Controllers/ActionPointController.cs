using ActivityAssistent.Api.Interfaces.ActionPoint;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Dtos.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityAssistent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionPointController(IActionPointService ActionPointService) : BaseApiController
    {
        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ActionPointDto>>> CreateAsync(CreateActionPointDto ActionPoint, CancellationToken Token)
        {
            var response = await ActionPointService.CreateActionPointAsync(ActionPoint, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetDelegationUsers")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<UserProfileDto>>>> GetDelegationUsersAsync(CancellationToken Token)
        {
            var response = await ActionPointService.GetDelegationUsersAsync(Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetById/{actionPointId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ActionPointDto>>> GetByIdAsync([FromRoute] Guid actionPointId, CancellationToken Token)
        {
            var response = await ActionPointService.GetByIdAsync(actionPointId, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetByConversation/{conversationId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ActionPointDto>>>> GetByConversationAsync([FromRoute] Guid conversationId, CancellationToken Token)
        {
            var response = await ActionPointService.GetByConversationIdAsync(conversationId, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetActiveByUser/{userId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ActionPointDto>>>> GetActiveByUserAsync([FromRoute] string userId, CancellationToken Token)
        {
            var response = await ActionPointService.GetActiveActionPointsAsync(userId, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ActionPointDto>>> UpdateAsync(UpdateActionPointDto ActionPoint, CancellationToken Token)
        {
            var response = await ActionPointService.UpdateActionPointAsync(ActionPoint, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("delete/{actionPointId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] Guid actionPointId, CancellationToken Token)
        {
            var response = await ActionPointService.DeleteActionPointAsync(actionPointId, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
