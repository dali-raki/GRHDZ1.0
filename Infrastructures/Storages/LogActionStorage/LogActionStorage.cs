
using GrhDz.Domains.Models.Logs;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Infrastructures.Storages.LogActionStorage;

public class LogActionStorage(IConfiguration configuration) : ILogActionStorage
{
    private readonly string _connectionString = configuration.GetConnectionString("DBConnection");  
    private const string insertLogCommand = "INSERT INTO LogsActions (ActionType, PerformedBy,Description) " + "VALUES (@aActionType, @aPerformedBy, @aDescription)";

    private const string selectLogsQuery = "SELECT * FROM LogsActions";

    public async Task<int> InsertLog(LogAction logActions)
    {
        await using var connection = new SqlConnection(_connectionString);

        await using var cmd = new SqlCommand(insertLogCommand, connection);
        cmd.Parameters.AddWithValue("@aActionType", logActions.ActionType.ToString());
        cmd.Parameters.AddWithValue("@aPerformedBy", logActions.PerformedBy ?? "N/A");
        cmd.Parameters.AddWithValue("@aDescription", logActions.Description ?? "N/A");

        await connection.OpenAsync();
        
        int insertEdRow = await cmd.ExecuteNonQueryAsync();
        return insertEdRow;
    }
    public async Task<List<LogAction>> SelectAllLogs()
    {
        var logs = new List<LogAction>();

       await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var cmd = new SqlCommand(selectLogsQuery, connection);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            logs.Add(logActionFromReader(reader));
        }

     
        return logs;
    }

    private static LogAction logActionFromReader(SqlDataReader reader) =>
        new LogAction
        {
            Id = reader.GetInt32(0),
            ActionType = Enum.Parse<ActionType>(reader.GetString(1)),
            PerformedBy = reader.GetString(2),
            Description = reader.GetString(3),
            ActionDate = reader.GetDateTime(4)
        };
}
