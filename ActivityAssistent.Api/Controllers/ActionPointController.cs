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
            try
            {
                var createdActionPoint = await ActionPointService.CreateActionPointAsync(ActionPoint, Token);
                if (createdActionPoint != null)
                {
                    return Ok(new ApiResponse<ActionPointDto>
                    {
                        IsSuccess = true,
                        Data = createdActionPoint,
                        ErrorMessage = string.Empty
                    });
                }

                return BadRequest(new ApiResponse<ActionPointDto>
                {
                    IsSuccess = false,
                    Data = new ActionPointDto(),
                    ErrorMessage = "An error occurred while creating the action point."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while creating action point: {ex}");
                return BadRequest(new ApiResponse<ActionPointDto>
                {
                    IsSuccess = false,
                    Data = new ActionPointDto(),
                    ErrorMessage = "An error occurred while creating the action point."
                });
            }
        }

        [HttpGet("GetDelegationUsers")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<UserProfileDto>>>> GetDelegationUsersAsync(CancellationToken Token)
        {
            try
            {
                var users = await ActionPointService.GetDelegationUsersAsync(Token);
                return Ok(new ApiResponse<List<UserProfileDto>>
                {
                    IsSuccess = true,
                    Data = users,
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching delegation users: {ex}");
                return BadRequest(new ApiResponse<List<UserProfileDto>>
                {
                    IsSuccess = false,
                    Data = new List<UserProfileDto>(),
                    ErrorMessage = "An error occurred while fetching delegation users."
                });
            }
        }

        [HttpGet("GetById/{actionPointId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ActionPointDto>>> GetByIdAsync([FromRoute] Guid actionPointId, CancellationToken Token)
        {
            try
            {
                var actionPoint = await ActionPointService.GetByIdAsync(actionPointId, Token);
                if (actionPoint.ActionPointId != Guid.Empty)
                {
                    return Ok(new ApiResponse<ActionPointDto>
                    {
                        IsSuccess = true,
                        Data = actionPoint,
                        ErrorMessage = string.Empty
                    });
                }

                return BadRequest(new ApiResponse<ActionPointDto>
                {
                    IsSuccess = false,
                    Data = new ActionPointDto(),
                    ErrorMessage = "Action point not found."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching action point: {ex}");
                return BadRequest(new ApiResponse<ActionPointDto>
                {
                    IsSuccess = false,
                    Data = new ActionPointDto(),
                    ErrorMessage = "An error occurred while fetching the action point."
                });
            }
        }

        [HttpGet("GetByConversation/{conversationId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ActionPointDto>>>> GetByConversationAsync([FromRoute] Guid conversationId, CancellationToken Token)
        {
            try
            {
                var actionPoints = await ActionPointService.GetByConversationIdAsync(conversationId, Token);
                return Ok(new ApiResponse<List<ActionPointDto>>
                {
                    IsSuccess = true,
                    Data = actionPoints.ToList(),
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching action points: {ex}");
                return BadRequest(new ApiResponse<List<ActionPointDto>>
                {
                    IsSuccess = false,
                    Data = new List<ActionPointDto>(),
                    ErrorMessage = "An error occurred while fetching action points."
                });
            }
        }

        [HttpGet("GetActiveByUser/{userId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ActionPointDto>>>> GetActiveByUserAsync([FromRoute] string userId, CancellationToken Token)
        {
            try
            {
                var actionPoints = await ActionPointService.GetActiveActionPointsAsync(userId, Token);
                return Ok(new ApiResponse<List<ActionPointDto>>
                {
                    IsSuccess = true,
                    Data = actionPoints.ToList(),
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while fetching action points: {ex}");
                return BadRequest(new ApiResponse<List<ActionPointDto>>
                {
                    IsSuccess = false,
                    Data = new List<ActionPointDto>(),
                    ErrorMessage = "An error occurred while fetching action points."
                });
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ActionPointDto>>> UpdateAsync(UpdateActionPointDto ActionPoint, CancellationToken Token)
        {
            try
            {
                var updatedActionPoint = await ActionPointService.UpdateActionPointAsync(ActionPoint, Token);
                if (updatedActionPoint != null)
                {
                    return Ok(new ApiResponse<ActionPointDto>
                    {
                        IsSuccess = true,
                        Data = updatedActionPoint,
                        ErrorMessage = string.Empty
                    });
                }

                return BadRequest(new ApiResponse<ActionPointDto>
                {
                    IsSuccess = false,
                    Data = new ActionPointDto(),
                    ErrorMessage = "An error occurred while updating the action point."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while updating action point: {ex}");
                return BadRequest(new ApiResponse<ActionPointDto>
                {
                    IsSuccess = false,
                    Data = new ActionPointDto(),
                    ErrorMessage = "An error occurred while updating the action point."
                });
            }
        }

        [HttpDelete("delete/{actionPointId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAsync([FromRoute] Guid actionPointId, CancellationToken Token)
        {
            try
            {
                var result = await ActionPointService.DeleteActionPointAsync(actionPointId, Token);
                return Ok(new ApiResponse<bool>
                {
                    IsSuccess = true,
                    Data = result,
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error happend while deleting action point: {ex}");
                return BadRequest(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    ErrorMessage = "An error occurred while deleting the action point."
                });
            }
        }
    }
}
