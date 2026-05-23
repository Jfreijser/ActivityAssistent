using ActivityAssistent.Api.Interfaces.Conversations;
using ActivityAssistent.Shared.Dtos.Conversations;
using Microsoft.AspNetCore.Mvc;

namespace ActivityAssistent.Api.Controllers
{
    public class ConversationController(IConversationService ConversationService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IActionResult>>GetAllAsync(CancellationToken Token)
        {
            var Results = await ConversationService.GetAllAsync(Token);
            return Ok(Results);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ConversationDto>> CreateAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            try
            {
                var CreatedConversation = await ConversationService.CreateConversationAsync(Conversation, Token);
                if (CreatedConversation != null)
                {
                    return Ok(CreatedConversation);
                }
                return BadRequest("Failed to create conversation.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching customers: {ex}");
                return BadRequest(ex);
            }
            
        }
    }
}
