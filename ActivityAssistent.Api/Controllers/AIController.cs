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
                if (!string.IsNullOrWhiteSpace(result.Transcription))
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

    }
}
