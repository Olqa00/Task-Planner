namespace Planner.Infrastructure.DAL;

internal sealed class DatabaseInitializer : IHostedService
{
    private const string CREATE_DATABASE = @"
        IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Planner')
        BEGIN
            CREATE DATABASE Planner;
        END;";

    private const string CREATE_TABLE = @"
        USE Planner;
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Task' AND xtype='U')
        BEGIN
            CREATE TABLE Task (
                Id UNIQUEIDENTIFIER PRIMARY KEY,
                Title NVARCHAR(255) NOT NULL,
                CreatedAt DATETIME2 NOT NULL,
                CompletedAt DATETIME2 NULL
            );
        END;";

    private readonly IServiceProvider serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = this.serviceProvider.GetRequiredService<IConfiguration>()
            .GetConnectionString("SqlConnection");

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var createDatabaseCommand = new SqlCommand(CREATE_DATABASE, connection);
        await createDatabaseCommand.ExecuteNonQueryAsync(cancellationToken);

        await using var createTableCommand = new SqlCommand(CREATE_TABLE, connection);
        await createTableCommand.ExecuteNonQueryAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
