using ActivityAssistent.Api.Interfaces.Conversations;
using ActivityAssistent.Shared.Dtos.Conversations;
using Microsoft.AspNetCore.Mvc;
using ActivityAssistent.Shared.Dtos.Response;
using Microsoft.AspNetCore.Authorization;
namespace ActivityAssistent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController(IConversationService ConversationService) : BaseApiController
    {


        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ConversationDto>>> CreateAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            var response = await ConversationService.CreateConversationAsync(Conversation, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }

        [HttpGet("GetConversationById/{conversationId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ConversationDto>>> GetConversationById([FromRoute] Guid conversationId, CancellationToken Token)
        {
            var response = await ConversationService.GetConversationAsync(conversationId, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAllAsync")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ConversationDto>>>> GetAllAsync(CancellationToken Token)
        {
            var response = await ConversationService.GetAllAsync(Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ConversationDto>>> UpdateAsync(UpdateConversationDto Conversation, CancellationToken Token)
        {
            var response = await ConversationService.UpdateConversationAsync(Conversation, Token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
