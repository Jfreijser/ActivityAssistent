using ActivityAssistent.Api.Interfaces.Conversations;
using ActivityAssistent.Shared.Dtos.Conversations;
using Dapper;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace ActivityAssistent.Api.Infrastructure.Repositories.DapperRepository
{
    public class ConversationRepository(IDbConnectionFactory connection) : IConversationRepository
    {
        public async Task<Guid> CreateAsync(CreateConversationDto Conversation, CancellationToken Token)
        {
            var sql = @"insert into Conversations (Title, CompanyId, MeetingDate , [Status], SalesUserId ) 
                        OUTPUT INSERTED.ConversationId
                        values (@Title, @CompanyId, @MeetingDate, @Status, @SalesUserId )";
            using (var db = connection.CreateConnection())
            {
                return await db.QuerySingleAsync<Guid>(sql, Conversation);
            }
        }

        public Task<bool> ExistsAsync(Guid ConversationId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ConversationDto>> GetAllAsync(Guid OwnerUserId, CancellationToken Token)
        {
            var sql = "select * from Conversations where SalesUserId = @SalesUserId";
            using (var db = connection.CreateConnection())
            {
                var result = await db.QueryAsync<ConversationDto>(sql, new { SalesUserId = OwnerUserId });
                return result.ToList();
            }
             
        }

        public Task<IEnumerable<ConversationDto>> GetByCompanyIdAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<ConversationDto?> GetByIdAsync(Guid ConversationId, CancellationToken Token)
        {
            var sql = @"select con.*, Com.Name as CompanyName from Conversations as Con
                        left Join Companies As Com on Com.CompanyId = Con.CompanyId
                        where ConversationId = @ConversationId";

            using (var db = connection.CreateConnection())
            {
                return await db.QueryFirstOrDefaultAsync<ConversationDto>(sql, new { ConversationId });
            }
        }

        public async Task<bool> UpdateAsync(UpdateConversationDto Conversation, CancellationToken Token)
        {
            var sql = @"update Conversations set Title = @Title, MeetingDate = @MeetingDate, [Status] = @Status, Description = @Description where ConversationId = @ConversationId";
            using (var db = connection.CreateConnection())
            {
                try
                {
                    var command = new CommandDefinition(sql, Conversation, cancellationToken: Token);
                    var result = await db.ExecuteAsync(command);
                    return result > 0;
                }
                catch (Exception ex )
                {

                    Console.WriteLine($"[DB ERROR] Update failed for conversation {Conversation.ConversationId}: {ex.Message}");

                    return false;
                }
                
            }
        }
    }
}
