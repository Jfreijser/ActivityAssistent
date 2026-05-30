using ActivityAssistent.Api.Interfaces.Status;
using ActivityAssistent.Shared.Dtos.Ai;
using ActivityAssistent.Shared.Enums;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace ActivityAssistent.Api.Infrastructure.Repositories.DapperRepository
{
    public class AiStatusRepository(IDbConnectionFactory connection) : IAiStatusRepository
    {
        public async Task<AiStatusDto> GetStatusByTokenAsync(Guid Token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveInitialStatusAsync(AudioProcessingRequestDto StatusRecord, CancellationToken Token)
        {
            var sql = "Insert into AiAnalysisState (Token, ConversationId, FilePath, Created) VALUES (@Token, @ConversationId, @FilePath, @Created)";
            var Params = new DynamicParameters();
            Params.Add("@Token", StatusRecord.AudioToken);
            Params.Add("@ConversationId", StatusRecord.ConversationId);
            Params.Add("@FilePath", StatusRecord.filePath);
            Params.Add("@Created", DateTime.Now);

            var command = new CommandDefinition(sql, Params, cancellationToken: Token);
            try
            {
                using (var db = connection.CreateConnection())
                {
                    var result = await db.ExecuteAsync(command);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving initial status to database: {ex.Message}");
                return false;
            }
            
        }


        public Task<bool> SaveInitialStatusAsync(AiStatusDto StatusRecord)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStatusAsync(Guid Token, AiStatus NewStatus)
        {
            var sql = "UPDATE AiAnalysisState SET Status = @NewStatus, UpdatedAt = @UpdatedAt WHERE Token = @Token";
            var Params = new DynamicParameters();
            Params.Add("@NewStatus", NewStatus);
            Params.Add("@UpdatedAt", DateTime.Now);
            Params.Add("@Token", Token);

            var command = new CommandDefinition(sql, Params);
            try
            {
                using (var db = connection.CreateConnection())
                {
                    var result = await db.ExecuteAsync(command);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating status in database: {ex.Message}");
                return false;
            }
        }
    }
}
