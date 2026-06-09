using ActivityAssistent.Api.Infrastructure.Repositories.DapperRepository;
using ActivityAssistent.Api.Interfaces.Agenda;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Shared.Dtos.Agenda;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.Api.Services.Agenda
{
    public class AgendaService(IAgendaRepository agendaRepository, IUserContext UserContext, IUserRepository UserRepository) : IAgendaService
    {
        public async Task<ApiResponse<List<AgendaDto>>> GetAgendaPointsAsync(CancellationToken Token)
        {
            var SubNrId = UserContext.SubNrId;
            if (SubNrId == null || SubNrId == Guid.Empty)
            {
                return Fail<List<AgendaDto>>(null, "Invalid user context: SubNrId is missing or empty");
            }
            var result = await agendaRepository.GetAgendaPointsAsync(SubNrId.Value, Token);
            return result != null ? Ok(result) : Fail<List<AgendaDto>>(null, "Failed to retrieve agenda points");
        }

        private static ApiResponse<T> Ok<T>(T data)
            => new() { IsSuccess = true, Data = data, ErrorMessage = string.Empty };

        private static ApiResponse<T> Fail<T>(T data, string message)
            => new() { IsSuccess = false, Data = data, ErrorMessage = message };
    }
}
