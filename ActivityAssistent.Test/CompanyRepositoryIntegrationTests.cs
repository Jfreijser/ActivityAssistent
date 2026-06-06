using System.Data;
using ActivityAssistent.Shared.Dtos.Companies;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ActivityAssistent.Test;

public class CompanyRepositoryIntegrationTests
{
    private static string? ConnectionString => ResolveConnectionString();

    [Fact]
    public async Task DatabaseConnection_OpenConnection_Succeeds()
    {
        var connectionString = RequireConnectionString();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        Assert.Equal(ConnectionState.Open, connection.State);
    }

    [Fact]
    public async Task CompanyCrud_CreateAndGetById_ReturnsInsertedCompany()
    {
        var connectionString = RequireConnectionString();
        var companyId = Guid.NewGuid();
        var name = $"IT-{Guid.NewGuid():N}";

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        var ownerUserId = await GetExistingOwnerUserIdAsync(connection);

        var ensureSql = @"
IF OBJECT_ID('dbo.Companies', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Companies(
        CompanyId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(320) NOT NULL,
        PhoneNumber NVARCHAR(20) NOT NULL,
        CreatedOn DATETIME2 NOT NULL,
        City NVARCHAR(100) NOT NULL,
        Address NVARCHAR(200) NOT NULL,
        OwnerUserId UNIQUEIDENTIFIER NOT NULL
    );
END";

        await using (var ensureCommand = new SqlCommand(ensureSql, connection))
        {
            await ensureCommand.ExecuteNonQueryAsync();
        }

        var insertSql = @"
INSERT INTO dbo.Companies (CompanyId, Name, Email, PhoneNumber, CreatedOn, City, Address, OwnerUserId)
VALUES (@CompanyId, @Name, @Email, @PhoneNumber, @CreatedOn, @City, @Address, @OwnerUserId);";

        await using (var insertCommand = new SqlCommand(insertSql, connection))
        {
            insertCommand.Parameters.AddWithValue("@CompanyId", companyId);
            insertCommand.Parameters.AddWithValue("@Name", name);
            insertCommand.Parameters.AddWithValue("@Email", "integration@test.com");
            insertCommand.Parameters.AddWithValue("@PhoneNumber", "0612345678");
            insertCommand.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);
            insertCommand.Parameters.AddWithValue("@City", "Utrecht");
            insertCommand.Parameters.AddWithValue("@Address", "Integration Street 1");
            insertCommand.Parameters.AddWithValue("@OwnerUserId", ownerUserId);
            await insertCommand.ExecuteNonQueryAsync();
        }

        var selectSql = "SELECT CompanyId, Name, Email, PhoneNumber, City, Address FROM dbo.Companies WHERE CompanyId = @CompanyId";

        await using var selectCommand = new SqlCommand(selectSql, connection);
        selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

        await using var reader = await selectCommand.ExecuteReaderAsync();

        Assert.True(await reader.ReadAsync());

        var dto = new CompanyDto
        {
            CompanyId = reader.GetGuid(0),
            CompanyName = reader.GetString(1),
            EmailAddress = reader.GetString(2),
            PhoneNumber = reader.GetString(3),
            City = reader.GetString(4),
            Address = reader.GetString(5)
        };

        Assert.Equal(companyId, dto.CompanyId);
        Assert.Equal(name, dto.CompanyName);

        await reader.DisposeAsync();

        await DeleteCompanyAsync(connection, companyId);
    }

    [Fact]
    public async Task CompanyCrud_CreateThenDelete_RemovesCompany()
    {
        var connectionString = RequireConnectionString();
        var companyId = Guid.NewGuid();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        var ownerUserId = await GetExistingOwnerUserIdAsync(connection);

        var ensureSql = @"
IF OBJECT_ID('dbo.Companies', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Companies(
        CompanyId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(320) NOT NULL,
        PhoneNumber NVARCHAR(20) NOT NULL,
        CreatedOn DATETIME2 NOT NULL,
        City NVARCHAR(100) NOT NULL,
        Address NVARCHAR(200) NOT NULL,
        OwnerUserId UNIQUEIDENTIFIER NOT NULL
    );
END";

        await using (var ensureCommand = new SqlCommand(ensureSql, connection))
        {
            await ensureCommand.ExecuteNonQueryAsync();
        }

        var insertSql = @"
INSERT INTO dbo.Companies (CompanyId, Name, Email, PhoneNumber, CreatedOn, City, Address, OwnerUserId)
VALUES (@CompanyId, @Name, @Email, @PhoneNumber, @CreatedOn, @City, @Address, @OwnerUserId);";

        await using (var insertCommand = new SqlCommand(insertSql, connection))
        {
            insertCommand.Parameters.AddWithValue("@CompanyId", companyId);
            insertCommand.Parameters.AddWithValue("@Name", $"IT-{Guid.NewGuid():N}");
            insertCommand.Parameters.AddWithValue("@Email", "delete@test.com");
            insertCommand.Parameters.AddWithValue("@PhoneNumber", "0612345678");
            insertCommand.Parameters.AddWithValue("@CreatedOn", DateTime.UtcNow);
            insertCommand.Parameters.AddWithValue("@City", "Utrecht");
            insertCommand.Parameters.AddWithValue("@Address", "Integration Street 1");
            insertCommand.Parameters.AddWithValue("@OwnerUserId", ownerUserId);
            await insertCommand.ExecuteNonQueryAsync();
        }

        await DeleteCompanyAsync(connection, companyId);

        var existsSql = "SELECT COUNT(1) FROM dbo.Companies WHERE CompanyId = @CompanyId";
        await using var existsCommand = new SqlCommand(existsSql, connection);
        existsCommand.Parameters.AddWithValue("@CompanyId", companyId);

        var count = (int)await existsCommand.ExecuteScalarAsync();
        Assert.Equal(0, count);
    }

    private static async Task DeleteCompanyAsync(SqlConnection connection, Guid companyId)
    {
        var deleteSql = "DELETE FROM dbo.Companies WHERE CompanyId = @CompanyId";
        await using var deleteCommand = new SqlCommand(deleteSql, connection);
        deleteCommand.Parameters.AddWithValue("@CompanyId", companyId);
        await deleteCommand.ExecuteNonQueryAsync();
    }

    private static string RequireConnectionString()
    {
        if (!string.IsNullOrWhiteSpace(ConnectionString))
        {
            return ConnectionString;
        }

        throw new InvalidOperationException(
            "Set user secret 'ConnectionStrings:Default' (preferred) or environment variable ACTIVITYASSISTENT_TEST_DB_CONNECTION with a valid SQL Server connection string before running integration tests.");
    }

    private static string? ResolveConnectionString()
    {
        var fromEnvironment = Environment.GetEnvironmentVariable("ACTIVITYASSISTENT_TEST_DB_CONNECTION");
        if (!string.IsNullOrWhiteSpace(fromEnvironment))
        {
            return fromEnvironment;
        }

        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<CompanyRepositoryIntegrationTests>(optional: true)
            .Build();

        return configuration.GetConnectionString("Default");
    }

    private static async Task<Guid> GetExistingOwnerUserIdAsync(SqlConnection connection)
    {
        const string sql = "SELECT TOP 1 UserId FROM dbo.Users";
        await using var command = new SqlCommand(sql, connection);
        var result = await command.ExecuteScalarAsync();

        if (result is Guid userId)
        {
            return userId;
        }

        throw new InvalidOperationException(
            "Integration tests require at least one row in dbo.Users because Companies.OwnerUserId has a foreign key to Users.UserId.");
    }
}
