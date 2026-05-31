using ActivityAssistent.Api.Interfaces.Ai;
using ActivityAssistent.Shared.Dtos.Ai;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ActivityAssistent.Shared.Dtos.Response;
using Microsoft.AspNetCore.Mvc;

namespace ActivityAssistent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController(IAiMeetingAnalyzerService AiMeetingAnalyzer) : BaseApiController
    {
        [HttpPost("analyzeMeeting")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<MeetingAnalysisResultDto>>> AnalyzeMeetingAsync(AudioProcessingRequestDto RequestPayload, CancellationToken Token)
        {
            try
            {
                var result = await AiMeetingAnalyzer.StartAnalyzeMeetingAsync(RequestPayload, Token);
                if (!string.IsNullOrWhiteSpace(result.Summary))
                {
                    return Ok(new ApiResponse<MeetingAnalysisResultDto>
                    {
                        IsSuccess = true,
                        Data = result,
                        ErrorMessage = ""
                    });
                }

                return BadRequest(new ApiResponse<MeetingAnalysisResultDto>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrorMessage = "Failed to analyze the meeting."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while analyzing the meeting: {ex}");
                return BadRequest(new ApiResponse<MeetingAnalysisResultDto>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrorMessage = "Failed to analyze the meeting."
                });
            }
        }

        [HttpGet("AiStatus/{Token}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<AiStatusDto?>>> GetAiStatusAsync([FromRoute] Guid Token, CancellationToken CancelToken)
        {
            try
            {
                var result = await AiMeetingAnalyzer.GetAiStatusAsync(Token, CancelToken);
                if (result != null)
                {
                    return Ok(new ApiResponse<AiStatusDto>
                    {
                        IsSuccess = true,
                        Data = result,
                        ErrorMessage = ""
                    });
                }
                return NotFound(new ApiResponse<AiStatusDto>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrorMessage = "AI status not found for the provided token."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching AI status: {ex}");
                return BadRequest(new ApiResponse<AiStatusDto>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrorMessage = "Failed to fetch AI status."
                });
            }

        }


        [HttpPost("SaveAnalysisActionPoints")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> SaveAnalysisResultsAsync(MeetingAnalysisResultDto Model, CancellationToken CancelToken)
        {
            try
            {
                var result = await AiMeetingAnalyzer.SaveAnalysisResultsAsync(Model, CancelToken);
                if (result)
                {
                    return Ok(new ApiResponse<bool>
                    {
                        IsSuccess = true,
                        Data = result,
                        ErrorMessage = ""
                    });
                }

                return BadRequest(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    ErrorMessage = "Failed to save analysis results."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving analysis results: {ex}");
                return BadRequest(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    ErrorMessage = "Failed to save analysis results."
                });
            }
        }
    }
}
