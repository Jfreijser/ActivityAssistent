using ActivityAssistent.Shared.Dtos.Ai;
using ActivityAssistent.Shared.Enums;

namespace ActivityAssistent.Api.Interfaces.Status
{
    public interface IAiStatusRepository : IStatusRepository<AiStatusDto, AiStatus>
    {
        Task <bool> SaveInitialStatusAsync(AudioProcessingRequestDto StatusRecord, CancellationToken CancellationToken);
    }
}
