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
            try
            {
                var CreatedConversation = await ConversationService.CreateConversationAsync(Conversation, Token);
                if (CreatedConversation != null)
                {
                    return Ok(new ApiResponse<ConversationDto>
                    {
                        IsSuccess = true,
                        Data = CreatedConversation,
                        ErrorMessage = string.Empty
                    });
                }
                return BadRequest(new ApiResponse<ConversationDto>
                {
                    IsSuccess = false,
                    Data = new ConversationDto(),
                    ErrorMessage = "An error occurred while creating the conversation."
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching customers: {ex}");
                return BadRequest(new ApiResponse<ConversationDto>
                {
                    IsSuccess = false,
                    Data = new ConversationDto(),
                    ErrorMessage = "An error occurred while creating the conversation."
                });
            }

        }

        [HttpGet("GetConversationById/{conversationId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ConversationDto>>> GetConversationById([FromRoute] Guid conversationId, CancellationToken Token)
        {
            try
            {
                var Conversation = await ConversationService.GetConversationAsync(conversationId, Token);
                if (Conversation.ConversationId != Guid.Empty)
                {
                    return Ok(new ApiResponse<ConversationDto>
                    {
                        IsSuccess = true,
                        Data = Conversation,
                        ErrorMessage = string.Empty
                    });
                }
                return BadRequest(new ApiResponse<ConversationDto>
                {
                    IsSuccess = false,
                    Data = new ConversationDto(),
                    ErrorMessage = "Conversation not found."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching customers: {ex}");
                return BadRequest(new ApiResponse<ConversationDto>
                {
                    IsSuccess = false,
                    Data = new ConversationDto(),
                    ErrorMessage = "An error occurred while fetching the conversation."
                });
            }
        }

        [HttpGet("GetAllAsync")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ConversationDto>>>> GetAllAsync(CancellationToken Token)
        {
            try
            {
                var Conversations = await ConversationService.GetAllAsync(Token);
                return Ok(new ApiResponse<List<ConversationDto>>
                {
                    IsSuccess = true,
                    Data = Conversations.ToList(),
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching customers: {ex}");
                return BadRequest(new ApiResponse<List<ConversationDto>>
                {
                    IsSuccess = false,
                    Data = new List<ConversationDto>(),
                    ErrorMessage = "An error occurred while fetching conversations."
                });
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ConversationDto>>> UpdateAsync(UpdateConversationDto Conversation, CancellationToken Token)
        {
            try
            {
                var UpdatedConversation = await ConversationService.UpdateConversationAsync(Conversation, Token);
                if (UpdatedConversation != null)
                {
                    return Ok(new ApiResponse<ConversationDto>
                    {
                        IsSuccess = true,
                        Data = UpdatedConversation,
                        ErrorMessage = string.Empty
                    });
                }
                return BadRequest(new ApiResponse<ConversationDto>
                {
                    IsSuccess = false,
                    Data = new ConversationDto(),
                    ErrorMessage = "An error occurred while updating the conversation."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching customers: {ex}");
                return BadRequest(new ApiResponse<ConversationDto>
                {
                    IsSuccess = false,
                    Data = new ConversationDto(),
                    ErrorMessage = "An error occurred while updating the conversation."
                });
            }
        }
    }
}
