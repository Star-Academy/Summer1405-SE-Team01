using SqlBuilder.QueryBuilders.Implementations;
using SqlBuilder.Querying;
using Microsoft.Data.SqlClient;
using FluentAssertions;

namespace Mini_query_builder.IntegrationTests;

public class SqlServerIntegrationTests : IClassFixture<SqlServerContainerFixture>, IAsyncLifetime
{
    private readonly string _connectionString;

    public SqlServerIntegrationTests(SqlServerContainerFixture fixture)
    {
        _connectionString = fixture.ConnectionString;
    }

    public async Task InitializeAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            DROP TABLE IF EXISTS [Student2];
            CREATE TABLE [Student2]
            (
                [StudentNumber] VARCHAR(8) NOT NULL PRIMARY KEY,
                [Grade] FLOAT(2),
                [FirstName] VARCHAR(20) NOT NULL,
                [LastName] VARCHAR(20) NOT NULL,
                [IsMale] BIT NOT NULL,
                [DateOfBirth] DATETIME2 NOT NULL,
                [LeftUnitsCount] INT NOT NULL
            );
        """;
        await command.ExecuteNonQueryAsync();
    }

    public async Task DisposeAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "DROP TABLE IF EXISTS [Student2];";
        await command.ExecuteNonQueryAsync();
    }

    private async Task SeedDataAsync(string insertSql)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = insertSql;
        await command.ExecuteNonQueryAsync();
    }

    private static async Task<List<Dictionary<string, object>>> RunQueryAsync(string connectionString, Query query)
    {
        var compiler = new QueryCompiler(new SqlServerQueryDecomposer());
        var compiled = compiler.Compile(query);

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(compiled.RawQuery, connection);
        for (var i = 0; i < compiled.Bindings.Count; i++)
            command.Parameters.AddWithValue($"@p{i + 1}", compiled.Bindings[i]);

        await using var reader = await command.ExecuteReaderAsync();
        var rows = new List<Dictionary<string, object>>();
        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
                row[reader.GetName(i)] = reader.GetValue(i);
            rows.Add(row);
        }
        return rows;
    }

    [Fact]
    public async Task Query_ShouldReturnEmptyList_WhenTableIsEmpty()
    {
        // Arrange
        var query = new Query().From("Student2");

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Grade", 18.5, "99100001,99100003")]
    [InlineData("IsMale", true, "99100001,99100003")]
    [InlineData("StudentNumber", "99100002", "99100002")]
    [InlineData("Grade", 999, "")]
    public async Task Query_ShouldReturnRowsMatchingSingleWhereCondition_WhenApplied(
        string column, object value, string expectedStudentNumbersCsv)
    {
        // Arrange
        await SeedDataAsync("""
            INSERT INTO [Student2] VALUES
                ('99100001', 18.5, 'Ali', 'Ahmadi', 1, '2000-01-01 00:00:00', 20),
                ('99100002', 14.0, 'Sara', 'Razi', 0, '2001-05-12 00:00:00', 40),
                ('99100003', 18.5, 'Reza', 'Karimi', 1, '2000-03-15 00:00:00', 20),
                ('99100004', 11.5, 'Mina', 'Nouri', 0, '2002-08-20 00:00:00', 60);
        """);

        var query = new Query().From("Student2").Where(column, value);
        var expectedStudentNumbers = string.IsNullOrEmpty(expectedStudentNumbersCsv)
            ? Array.Empty<string>()
            : expectedStudentNumbersCsv.Split(',');

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Select(r => (string)r["StudentNumber"]).Should().BeEquivalentTo(expectedStudentNumbers);
    }

    [Fact]
    public async Task Query_Should_ReturnAllRows_When_NoWhereConditionIsApplied()
    {
        // Arrange
        await SeedDataAsync("""
            INSERT INTO [Student2] VALUES
                ('99100001', 15.0, 'Hassan', 'Hossieni', 1, '1999-01-01 00:00:00', 10),
                ('99100002', 18.0, 'Zahra', 'Tehrani', 0, '2000-02-02 00:00:00', 15);
        """);

        var query = new Query().From("Student2");

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Should().HaveCount(2);
    }

    [Fact]
    public async Task Query_Should_ReturnOnlyMatchingRows_When_MultipleWhereConditionsAreAppliedTogether()
    {
        // Arrange
        await SeedDataAsync("""
            INSERT INTO [Student2] VALUES
                ('99100001', 18.5, 'Arman', 'Sadeghi', 1, '2000-01-01 00:00:00', 20),
                ('99100002', 18.5, 'Nima', 'Ebrahimi', 1, '2000-05-05 00:00:00', 20),
                ('99100003', 18.5, 'Sima', 'Moradi', 0, '2001-03-03 00:00:00', 20),
                ('99100004', 14.0, 'Farhad', 'Khosravi', 1, '1998-04-04 00:00:00', 50);
        """);

        var query = new Query().From("Student2").Where("Grade", 18.5).Where("IsMale", true);

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Select(r => (string)r["StudentNumber"]).Should().BeEquivalentTo("99100001", "99100002");
    }

    [Fact]
    public async Task Query_Should_ReturnOnlySelectedColumns_When_SpecificColumnsAreSelected()
    {
        // Arrange
        await SeedDataAsync("""
            INSERT INTO [Student2] VALUES
                ('99100001', 20.0, 'Babak', 'Salimi', 1, '2000-01-01 00:00:00', 0);
        """);

        var query = new Query().From("Student2").Select("FirstName", "LastName").Where("StudentNumber", "99100001");

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Should().ContainSingle();
        rows[0].Keys.Should().BeEquivalentTo("FirstName", "LastName");
    }

    [Fact]
    public async Task Query_Should_PreventSqlInjection_When_InputContainsSqlStatements()
    {
        // Arrange
        await SeedDataAsync("""
            INSERT INTO [Student2] VALUES
                ('99100001', 15.0, 'Ali', 'Ahmadi', 1, '2000-01-01 00:00:00', 20);
        """);

        string maliciousInput = "Ali'; DROP TABLE [Student2];--";
        var query = new Query().From("Student2").Where("FirstName", maliciousInput);

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Should().BeEmpty();

        var verifyQuery = new Query().From("Student2").Where("StudentNumber", "99100001");
        var verifyRows = await RunQueryAsync(_connectionString, verifyQuery);

        verifyRows.Should().ContainSingle();
    }
}