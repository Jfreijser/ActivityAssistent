using ActivityAssistent.Shared.Dtos.Agenda;
using ActivityAssistent.Shared.Dtos.Response;

namespace ActivityAssistent.Api.Interfaces.Agenda
{
    public interface IAgendaService
    {
        Task<ApiResponse<List<AgendaDto>>> GetAgendaPointsAsync(CancellationToken Token);
    }
}
