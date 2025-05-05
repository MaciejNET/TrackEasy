using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using TrackEasy.Infrastructure.Database;

namespace TrackEasy.IntegrationTests;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
        .WithName("trackeasy-test-db")
        .WithPortBinding(1434)
        .WithPassword("P@ssw0rd")
        .WithCleanUp(true)
        .WithWaitStrategy(Wait.ForUnixContainer().AddCustomWaitStrategy(new DatabaseReadyWaitStrategy()))
        .Build();
    
    public string ConnectionString => $"{_dbContainer.GetConnectionString()};TrustServerCertificate=True;";
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await using var dbContext = CreateDbContext();
        await dbContext.Database.MigrateAsync();
    }

    private TrackEasyDbContext CreateDbContext() => new(new DbContextOptionsBuilder<TrackEasyDbContext>()
        .UseAzureSql(ConnectionString).Options);

    public async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}

public class DatabaseReadyWaitStrategy : IWaitUntil
{
    public async Task<bool> UntilAsync(DotNet.Testcontainers.Containers.IContainer container)
    {
        var port = container.GetMappedPublicPort(1433);
        var host = container.Hostname;
        var connectionString = $"Server={host},{port};Database=master;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=True;";

        try
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand("SELECT 1;", connection);
            await command.ExecuteScalarAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}