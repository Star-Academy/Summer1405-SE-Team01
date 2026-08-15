// PostgresContainerFixture.cs
using Testcontainers.PostgreSql;

namespace Mini_query_builder.IntegrationTests;

public class PostgresContainerFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("testdb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await SeedDataAsync();
    }

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();

    private async Task SeedDataAsync()
    {
        await using var connection = new Npgsql.NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE "Student2"
            (
                "StudentNumber" VARCHAR(8) NOT NULL PRIMARY KEY,
                "Grade" FLOAT(2),
                "FirstName" VARCHAR(20) NOT NULL,
                "LastName" VARCHAR(20) NOT NULL,
                "IsMale" BOOLEAN NOT NULL,
                "DateOfBirth" TIMESTAMP NOT NULL,
                "LeftUnitsCount" INT NOT NULL
            );
            INSERT INTO "Student2" VALUES
                ('99100001', 18.5, 'Ali', 'Ahmadi', true, '2000-01-01 00:00:00', 20),
                ('99100002', 14.0, 'Sara', 'Razi', false, '2001-05-12 00:00:00', 40),
                ('99100003', 18.5, 'Reza', 'Karimi', true, '2000-03-15 00:00:00', 20),
                ('99100004', 11.5, 'Mina', 'Nouri', false, '2002-08-20 00:00:00', 60);
        """;
        await command.ExecuteNonQueryAsync();
    }
}
    
   //folder
   //break sead
   //parametric
   