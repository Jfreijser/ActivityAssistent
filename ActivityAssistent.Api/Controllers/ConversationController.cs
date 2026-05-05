using ActivityAssistent.Shared.Interfaces.Conversations;
using Microsoft.AspNetCore.Mvc;

namespace ActivityAssistent.Api.Controllers
{
    public class ConversationController(IConversationService ConversationService) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken Token)
        {
            var Results = await ConversationService.GetAllAsync(Token);
            return Ok(Results);
        }
    }
}
