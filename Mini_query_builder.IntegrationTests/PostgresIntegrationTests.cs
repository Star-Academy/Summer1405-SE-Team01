// PostgresIntegrationTests.cs
using SqlBuilder.QueryBuilders.Implementations;
using SqlBuilder.Querying;
using Npgsql;
using FluentAssertions;


namespace Mini_query_builder.IntegrationTests;

public class PostgresIntegrationTests : IClassFixture<PostgresContainerFixture>, IAsyncLifetime
{
    private readonly string _connectionString;

    public PostgresIntegrationTests(PostgresContainerFixture fixture)
    {
        _connectionString = fixture.ConnectionString;
    }
//extract
    private static async Task<List<Dictionary<string, object>>> RunQueryAsync(string connectionString, Query query)
    {
        var compiler = new QueryCompiler(new PostgreQueryDecomposer());
        var compiled = compiler.Compile(query);

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        //connectin bad
        await using var command = new NpgsqlCommand(compiled.RawQuery, connection);
        foreach (var binding in compiled.Bindings)
            command.Parameters.Add(new NpgsqlParameter { Value = binding });

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
    public async Task Query_ShouldReturnAllRows_WhenNoWhereConditionIsApplied()
    {
        // Arrange
        var query = new Query().From("Student");

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Should().HaveCount(4);
    }

    [Theory]
    [InlineData("Grade", 18.5, "99100001,99100003")]
    [InlineData("IsMale", true, "99100001,99100003")]
    [InlineData("StudentNumber", "99100002", "99100002")]
    [InlineData("Grade", 999, "")]
    //name
    public async Task Query_ShouldReturnRowsMatchingSingleWhereCondition_WhenApplied(
    string column, object value, string expectedStudentNumbersCsv)
    {
        // Arrange

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
    public async Task Query_Should_ReturnOnlyMatchingRows_When_MultipleWhereConditionsAreAppliedTogether()
    {
        // Arrange
        var query = new Query().From("Student").Where("Grade", 10).Where("IsMale", true);

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Should().HaveCount(2);
        rows.Select(r => r["FirstName"]).Should().BeEquivalentTo("Ali", "Reza");
    }

    [Fact]
    public async Task Query_Should_ReturnOnlySelectedColumns_When_SpecificColumnsAreSelected()
    {
        // Arrange
        var query = new Query().From("Student").Select("FirstName").Where("Id", 1);

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Should().ContainSingle();
        rows[0].Keys.Should().BeEquivalentTo("FirstName");
        rows[0]["FirstName"].Should().Be("Ali");
    }

    [Fact]
    public async Task Query_Should_ReturnNoRows_When_NoRowMatchesTheWhereCondition()
    {
        // Arrange
        var query = new Query().From("Student").Where("Grade", 999);

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert
        rows.Should().BeEmpty();
    }
    [Fact]
    public async Task Query_Should_PreventSqlInjection_When_InputContainsSqlStatements()
    {
        // Arrange
        string maliciousInput = "Ali'; DROP TABLE \"Student2\";--";
        var query = new Query().From("Student2").Where("FirstName", maliciousInput);

        // Act
        var rows = await RunQueryAsync(_connectionString, query);

        // Assert 
        rows.Should().BeEmpty();

        var verifyQuery = new Query().From("Student2").Where("StudentNumber", "99100001");
        var verifyRows = await RunQueryAsync(_connectionString, verifyQuery);

        verifyRows.Should().ContainSingle();
    }
    
    //null
}