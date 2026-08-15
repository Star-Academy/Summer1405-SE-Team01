using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Xunit;

namespace Mini_query_builder.IntegrationTests;

public class SqlServerContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("YourStrong!Passw0rd")
        .Build();

    private SqlConnection Connection { get; set; } = null!;
    public string ConnectionString { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        ConnectionString = _container.GetConnectionString();
        Connection = new SqlConnection(ConnectionString);
        await Connection.OpenAsync();
        await CreateTableAsync();
        await InsertSeedDataAsync();
    }

    public async Task DisposeAsync()
    {
        if (Connection != null)
        {
            await Connection.DisposeAsync();
        }
        await _container.DisposeAsync();
    }

    private async Task CreateTableAsync()
    {
        await using var command = Connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE [Student2]
            (
                [StudentNumber] VARCHAR(8) NOT NULL PRIMARY KEY,
                [Grade] FLOAT NULL,
                [FirstName] VARCHAR(20) NOT NULL,
                [LastName] VARCHAR(20) NOT NULL,
                [IsMale] BIT NOT NULL,
                [DateOfBirth] DATETIME2 NOT NULL,
                [LeftUnitsCount] INT NOT NULL
            );
        """;
        await command.ExecuteNonQueryAsync();
    }

    private async Task InsertSeedDataAsync()
    {
        await using var command = Connection.CreateCommand();
        command.CommandText = """
            INSERT INTO [Student2] VALUES
                ('99100001', 18.5, 'Ali', 'Ahmadi', 1, '2000-01-01 00:00:00', 20),
                ('99100002', 14.0, 'Sara', 'Razi', 0, '2001-05-12 00:00:00', 40),
                ('99100003', 18.5, 'Reza', 'Karimi', 1, '2000-03-15 00:00:00', 20),
                ('99100004', 11.5, 'Mina', 'Nouri', 0, '2002-08-20 00:00:00', 60),
                ('99100005', NULL, 'Omid', 'Hosseini', 1, '2001-11-02 00:00:00', 30);
        """;
        await command.ExecuteNonQueryAsync();
    }
}