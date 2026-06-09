using ActivityAssistent.Shared.Dtos.Agenda;

namespace ActivityAssistent.Api.Interfaces.Agenda
{
    public interface IAgendaRepository
    {
        Task<List<AgendaDto>> GetAgendaPointsAsync(Guid SubNrId, CancellationToken Token);
    }
}
