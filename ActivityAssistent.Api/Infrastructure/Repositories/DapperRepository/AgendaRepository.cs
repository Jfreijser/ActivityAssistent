using ActivityAssistent.Api.Interfaces.Agenda;
using ActivityAssistent.Shared.Dtos.Agenda;
using Dapper;

namespace ActivityAssistent.Api.Infrastructure.Repositories.DapperRepository
{
    public class AgendaRepository(IDbConnectionFactory connection) : IAgendaRepository
    {
        public async Task<List<AgendaDto>> GetAgendaPointsAsync(Guid SubNrId, CancellationToken Token)
        {
            string sql = @"select u.UserId, u.FullName, a.ActionPointId, a.ConversationId, a.Description, a.DueDate, a.IsCompleted  from ActionPoints as a
                            left join Users as u on u.UserId = a.SalesUserId
                            where a.SubNrId = @SubNrId";
            var dict = new Dictionary<Guid, AgendaDto>();
            using (var db = connection.CreateConnection())
            {
               await db.QueryAsync<AgendaDto, AgendaActionPointDto, AgendaDto>(sql, (agenda, actionPoint) =>
               {
                   if (!dict.TryGetValue(agenda.UserId, out var currentAgenda))
                   {
                       currentAgenda = agenda;
                       dict.Add(currentAgenda.UserId, currentAgenda);
                   }

                   if (actionPoint != null && actionPoint.ActionPointId != Guid.Empty)
                   {
                       currentAgenda.ActionPoints.Add(actionPoint);
                   }
                   return currentAgenda;
               }, new { SubNrId }, splitOn: "ActionPointId");
            }

            return dict.Values.ToList();
        }
    }
}
