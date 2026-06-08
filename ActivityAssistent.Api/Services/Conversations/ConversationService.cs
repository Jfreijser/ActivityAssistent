using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Api.Interfaces.Conversations;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Shared.Dtos.Conversations;
using ActivityAssistent.Shared.Dtos.Response;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Api.Services.Conversations
{
    public class ConversationService(IConversationRepository ConversationRepository, ICompanyRepository CompanyRepository, IUserContext UserContext) : IConversationService
    {
        public async Task<ApiResponse<ConversationDto>> CreateConversationAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            try
            {
                var Company = await CompanyRepository.GetByIdAsync(Conversation.CompanyId, Token);
                if (Company == null || Company.CompanyId == Guid.Empty)
                {
                    return Fail(new ConversationDto(), $"Company with ID {Conversation.CompanyId} was not found.");
                }

                Conversation.Status = ConversationStatus.Scheduled;
                var CreatedId = await ConversationRepository.CreateAsync(Conversation, Token);
                if (CreatedId == Guid.Empty)
                {
                    return Fail(new ConversationDto(), "Failed to create the conversation in the database.");
                }

                return await GetConversationAsync(CreatedId, Token);
            }
            catch (Exception ex)
            {
                return Fail(new ConversationDto(), ex.Message);
            }

        }

        public Task<ApiResponse<bool>> DeleteConversationAsync(Guid conversationId, CancellationToken Token)
        {
            return System.Threading.Tasks.Task.FromResult(Fail(false, "Delete conversation is not implemented yet."));
        }

        public async Task<ApiResponse<List<ConversationDto>>> GetAllAsync(CancellationToken Token)
        {
            try
            {
                var result = await ConversationRepository.GetAllAsync(UserContext.CurrentUserId, Token);
                return Ok(result.ToList());
            }
            catch (Exception)
            {
                return Fail(new List<ConversationDto>(), "Failed to retrieve conversations from the database.");
            }
          
        }

        public async Task<ApiResponse<ConversationDto>> GetConversationAsync(Guid ConversationId, CancellationToken Token)
        {
            try
            {
                var result = await ConversationRepository.GetByIdAsync(ConversationId, Token);
                return result is null
                    ? Fail(new ConversationDto(), "Conversation not found.")
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return Fail(new ConversationDto(), ex.Message);
            }
        }

        public Task<ApiResponse<List<ConversationDto>>> GetRecentConversationsAsync(CancellationToken Token)
        {
            return System.Threading.Tasks.Task.FromResult(Fail(new List<ConversationDto>(), "Get recent conversations is not implemented yet."));
        }

        public async Task<ApiResponse<ConversationDto>> UpdateConversationAsync(UpdateConversationDto conversation, CancellationToken Token)
        {
            try
            {
                var result = await ConversationRepository.UpdateAsync(conversation, Token);

                if (result)
                {
                    return await GetConversationAsync(conversation.ConversationId, Token);
                }
                else
                {
                    return Fail(new ConversationDto(), "Failed to update the conversation.");
                }
            }
            catch (Exception ex)
            {
                return Fail(new ConversationDto(), $"An error occurred while updating the conversation. {ex.Message}");
            }
        }

        public Task<ApiResponse<bool>> UploadAudioAsync(Guid ConversationId, byte[] AudioData, CancellationToken Token)
        {
            return System.Threading.Tasks.Task.FromResult(Fail(false, "Upload audio is not implemented yet."));
        }

        private static ApiResponse<T> Ok<T>(T data)
            => new() { IsSuccess = true, Data = data, ErrorMessage = string.Empty };

        private static ApiResponse<T> Fail<T>(T data, string message)
            => new() { IsSuccess = false, Data = data, ErrorMessage = message };
    }
}
