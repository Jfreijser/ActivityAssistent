using System.Data;
using Microsoft.Data.SqlClient;



namespace ActivityAssistent.Api.Infrastructure;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration config)
        => _connectionString = config.GetConnectionString("Default")!;

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}