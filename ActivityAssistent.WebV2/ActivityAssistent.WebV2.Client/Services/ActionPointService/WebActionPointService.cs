using ActivityAssistent.Shared.Dtos.ActionPoints;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Dtos.Response;
using ActivityAssistent.WebV2.Client.Interfaces.ActionPoint;
using Microsoft.AspNetCore.Components.Authorization;

namespace ActivityAssistent.WebV2.Client.Services.ActionPointService
{
    public class WebActionPointService(HttpClient Http, AuthenticationStateProvider AuthStateProvider) : BaseService(Http, AuthStateProvider), IActionPointService
    {
        public async Task<ActionPointDto> CreateActionPointAsync(CreateActionPointDto ActionPoint, CancellationToken Token)
        {
            try
            {
                var response = await PostAsync<ApiResponse<ActionPointDto>>("api/ActionPoint/create", ActionPoint, Token);

                if (response.IsSuccess)
                {
                    return response.Data;
                }

                return new ActionPointDto();
            }
            catch (Exception)
            {
                return new ActionPointDto();
            }
        }

        public async Task<List<UserProfileDto>> GetDelegationUsersAsync(CancellationToken Token)
        {
            try
            {
                var response = await GetAsync<ApiResponse<List<UserProfileDto>>>("api/ActionPoint/GetDelegationUsers", Token);

                if (response == null || !response.IsSuccess)
                {
                    throw new HttpRequestException($"Error fetching delegation users: {response?.ErrorMessage}");
                }

                return response.Data;
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch delegation users.");
            }
        }

        public async Task<bool> DeleteActionPointAsync(Guid ActionPointId, CancellationToken Token)
        {
            var response = await DeleteAsync<ApiResponse<bool>>($"api/ActionPoint/delete/{ActionPointId}", Token);

            if (response == null)
            {
                return false;
            }

            return response.IsSuccess && response.Data;
        }

        public async Task<List<ActionPointDto>> GetActiveActionPointsAsync(string UserId, CancellationToken Token)
        {
            try
            {
                var response = await GetAsync<ApiResponse<List<ActionPointDto>>>($"api/ActionPoint/GetActiveByUser/{UserId}", Token);

                if (!response.IsSuccess)
                {
                    throw new HttpRequestException($"Error fetching action points: {response.ErrorMessage}");
                }

                return response.Data;
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch action points.");
            }
        }

        public async Task<List<ActionPointDto>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token)
        {
            try
            {
                var response = await GetAsync<ApiResponse<List<ActionPointDto>>>($"api/ActionPoint/GetByConversation/{ConversationId}", Token);

                if (!response.IsSuccess)
                {
                    throw new HttpRequestException($"Error fetching action points: {response.ErrorMessage}");
                }

                return response.Data;
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch action points.");
            }
        }

        public async Task<ActionPointResolutionsDto> ResolveActionPointAsync(CreateActionPointResolutionDto Resolution, CancellationToken Token)
        {
            try
            {
                var response = await PostAsync<ApiResponse<ActionPointResolutionsDto>>("api/ActionPoint/CreateResolution", Resolution, Token);

                if (response != null && response.IsSuccess && response.Data != null)
                {
                    return response.Data;
                }

                return new ActionPointResolutionsDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API FOUT] Opslaan van afsluitreden mislukt: {ex.Message}");
                return new ActionPointResolutionsDto();
            }
        }

        public async Task<List<ActionPointResolutionsDto>> GetActionPointResolutionsAsync(Guid ActionPointId, CancellationToken Token)
        {
            try
            {
                var response = await GetAsync<ApiResponse<List<ActionPointResolutionsDto>>>("api/ActionPoint/GetResolutions/{ActionPointId}", Token);

                if (response == null || !response.IsSuccess)
                {
                    throw new HttpRequestException($"Error fetching resolutions: {response?.ErrorMessage}");
                }

                return response.Data ?? new List<ActionPointResolutionsDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API FOUT] Ophalen van afsluitredenen mislukt: {ex.Message}");
                throw new Exception("Failed to fetch action point resolutions.");
            }
        }

        public async Task<ActionPointDto> GetActionPointByIdAsync(Guid ActionPointId, CancellationToken Token)
        {
            try
            {
                var response = await GetAsync<ApiResponse<ActionPointDto>>($"api/ActionPoint/GetById/{ActionPointId}", Token);

                if (response.IsSuccess)
                {
                    return response.Data;
                }

                return new ActionPointDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API FOUT] Ophalen mislukt: {ex.Message}");
                return new ActionPointDto();
            }
        }

        public async Task<ActionPointDto> UpdateActionPointAsync(UpdateActionPointDto UpdatedActionPoint, CancellationToken Token)
        {
            try
            {
                var result = await PutAsync<ApiResponse<ActionPointDto>>("api/ActionPoint/update", UpdatedActionPoint, Token);

                if (!result.IsSuccess)
                {
                    throw new HttpRequestException($"Error updating action point: {result.ErrorMessage}");
                }

                if (result.Data == null)
                {
                    throw new HttpRequestException("The API update was successful, but the returned data was empty.");
                }

                return result.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API FOUT] Updaten mislukt: {ex.Message}");
                throw;
            }
        }
    }
}
