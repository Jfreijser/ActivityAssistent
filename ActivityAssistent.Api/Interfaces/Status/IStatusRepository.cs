using ActivityAssistent.Shared.Enums;
using ActivityAssistent.Shared.Dtos.Ai;
namespace ActivityAssistent.Api.Interfaces.Status
{
    public interface IStatusRepository<TRecord, TStateEnum> where TRecord : class where TStateEnum : Enum
    {
        Task <bool> SaveInitialStatusAsync(TRecord StatusRecord);
        Task <bool> UpdateStatusAsync(Guid Token, TStateEnum NewStatus, CancellationToken CancelToken);
        Task<TRecord?> GetStatusByTokenAsync(Guid Token, CancellationToken CancelToken);
       
    }
}
