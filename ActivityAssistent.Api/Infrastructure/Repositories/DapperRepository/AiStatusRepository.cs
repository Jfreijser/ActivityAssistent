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
        public Task<bool> DeleteAnalysisResultsAsync(Guid Token, CancellationToken CancelToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<AiAnalysisStateRecord>> GetAnalysisStateByConversationIdAsync(Guid ConversationId, CancellationToken CancelToken)
        {
            var sql = "select Token, ConversationId, FilePath, Status as State, Transcription, AiSummary, IsCompleted, HasError, Created, UpdatedAt as Updated from AiAnalysisState where ConversationId = @ConversationId order by Created desc";
            var Params = new DynamicParameters();
            Params.Add("@ConversationId", ConversationId);

            var command = new CommandDefinition(sql, Params, cancellationToken: CancelToken);
            try
            {
                using (var db = connection.CreateConnection())
                {
                    var result = await db.QueryAsync<AiAnalysisStateRecord>(command);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching analysis state from database: {ex.Message}");
                return Array.Empty<AiAnalysisStateRecord>();
            }
        }

        public async Task<AiStatusDto> GetStatusByTokenAsync(Guid Token, CancellationToken CancelToken)
        {
            var sql = "select Status as CurrentState, UpdatedAt from AiAnalysisState where Token = @Token";
            var Params = new DynamicParameters();
            Params.Add("@Token", Token);

            var command = new CommandDefinition(sql, Params, cancellationToken: CancelToken);
            try
            {
                using (var db = connection.CreateConnection())
                {
                    var result = await db.QueryFirstOrDefaultAsync<AiStatusDto>(command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching status from database: {ex.Message}");
                return null;
            }
        }

        public Task<bool> SaveAnalysisResultsAsync(MeetingAnalysisResultDto Model, CancellationToken CancelToken)
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

        public async Task<bool> UpdateAnalysisResultsAsync(MeetingAnalysisResultDto Model, CancellationToken CancelToken)
        {
            var sql = "update AiAnalysisState set Status = 50, AiSummary = @Summary, IsCompleted = 1, UpdatedAt = @UpdatedAt where ConversationId = @ConversationId and Token = @Token";

            var Params = new DynamicParameters();
            Params.Add("@Summary", Model.Summary);
            Params.Add("@UpdatedAt", DateTime.Now);
            Params.Add("@ConversationId", Model.ConversationId);
            Params.Add("@Token", Model.Token);
            var command = new CommandDefinition(sql, Params, cancellationToken: CancelToken);

            using (var db = connection.CreateConnection())
            {
                try
                {
                    var result = await db.ExecuteAsync(command);
                    return result > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating analysis results in database: {ex.Message}");
                    return false;
                }
            }
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
