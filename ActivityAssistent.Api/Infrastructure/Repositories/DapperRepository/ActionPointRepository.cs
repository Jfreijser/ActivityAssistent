using ActivityAssistent.Api.Interfaces.ActionPoint;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using Dapper;

namespace ActivityAssistent.Api.Infrastructure.Repositories.DapperRepository
{
    public class ActionPointRepository(IDbConnectionFactory connection) : IActionPointRepository
    {
        public async Task<Guid> CreateAsync(CreateActionPointDto ActionPoint, CancellationToken Token)
        {
            var sql = @"insert into ActionPoints (ConversationId, Description, SalesUserId, DueDate, IsCompleted, SubNrId)
                        OUTPUT INSERTED.ActionPointId
                        values (@ConversationId, @Description, @SalesUserId, @DueDate, @IsCompleted, @SubNrId)";

            using (var db = connection.CreateConnection())
            {
                var command = new CommandDefinition(sql, ActionPoint, cancellationToken: Token);
                return await db.QuerySingleAsync<Guid>(command);
            }
        }

        public async Task<bool> DeleteAsync(Guid ActionPointId, CancellationToken Token)
        {
            var sql = "delete from ActionPoints where ActionPointId = @ActionPointId";

            using (var db = connection.CreateConnection())
            {
                var command = new CommandDefinition(sql, new { ActionPointId }, cancellationToken: Token);
                var result = await db.ExecuteAsync(command);
                return result > 0;
            }
        }

        public async Task<IEnumerable<ActionPointDto>> GetActiveByUserIdAsync(Guid SalesUserId, CancellationToken Token)
        {
            var sql = "select * from ActionPoints where SalesUserId = @SalesUserId and IsCompleted = 0";

            using (var db = connection.CreateConnection())
            {
                var command = new CommandDefinition(sql, new { SalesUserId }, cancellationToken: Token);
                var result = await db.QueryAsync<ActionPointDto>(command);
                return result.ToList();
            }
        }

        public async Task<IEnumerable<ActionPointDto>> GetByConversationIdAsync(Guid ConversationId, CancellationToken Token)
        {
            var sql = "select * from ActionPoints where ConversationId = @ConversationId";

            using (var db = connection.CreateConnection())
            {
                var command = new CommandDefinition(sql, new { ConversationId }, cancellationToken: Token);
                var result = await db.QueryAsync<ActionPointDto>(command);
                return result.ToList();
            }
        }

        public async Task<ActionPointDto?> GetByIdAsync(Guid ActionPointId, CancellationToken Token)
        {
            var sql = "select * from ActionPoints where ActionPointId = @ActionPointId";

            using (var db = connection.CreateConnection())
            {
                var command = new CommandDefinition(sql, new { ActionPointId }, cancellationToken: Token);
                return await db.QueryFirstOrDefaultAsync<ActionPointDto>(command);
            }
        }

        public async Task<bool> UpdateAsync(UpdateActionPointDto ActionPoint, CancellationToken Token)
        {
            var sql = @"update ActionPoints
                        set ConversationId = @ConversationId,
                            Description = @Description,
                            SalesUserId = @SalesUserId,
                            DueDate = @DueDate,
                            IsCompleted = @IsCompleted,
                            SubNrId = @SubNrId
                        where ActionPointId = @ActionPointId";

            using (var db = connection.CreateConnection())
            {
                try
                {
                    var command = new CommandDefinition(sql, ActionPoint, cancellationToken: Token);
                    var result = await db.ExecuteAsync(command);
                    return result > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DB ERROR] Update failed for action point {ActionPoint.ActionPointId}: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
