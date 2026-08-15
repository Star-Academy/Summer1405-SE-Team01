using SqlBuilder.QueryBuilders.Implementations;
using SqlBuilder.Querying;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Executors.Implementations;
using Xunit;
using FluentAssertions;

namespace Mini_query_builder.IntegrationTests;

public class SqlServerIntegrationTests : IClassFixture<SqlServerContainerFixture>, IAsyncLifetime
{
    private readonly SqlServerContainerFixture _fixture;
    private string _connectionString = null!;
    private ISQLServerExecutor _sut = null!;

    public SqlServerIntegrationTests(SqlServerContainerFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync()
    {
        _connectionString = _fixture.ConnectionString;
        _sut = new SQLServerExecutor(
            new SQLServerExecutorConnection(),
            new ExecutorGetResult(),
            new SQLServerExecutorAddParameters(),
            new SqlServerCommandExecutor());
        return Task.CompletedTask;
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private Task<List<Dictionary<string, object>>> RunQueryAsync(Query query)
    {
        var compiler = new QueryCompiler(new SqlServerQueryDecomposer());
        var compiled = compiler.Compile(query);
        var result = _sut.ExecuteOnSqlServer(compiled, _connectionString);
        return Task.FromResult(result);
    }

    [Fact]
    public async Task Query_ShouldReturnAllRows_WhenNoWhereConditionIsApplied()
    {
        var query = new Query().From("Student2");
        var rows = await RunQueryAsync(query);
        rows.Should().HaveCount(5);
    }

    [Theory]
    [InlineData("Grade", 18.5, "99100001,99100003")]
    [InlineData("IsMale", true, "99100001,99100003,99100005")]
    [InlineData("StudentNumber", "99100002", "99100002")]
    [InlineData("Grade", 999, "")]
    public async Task Query_ShouldReturnRowsMatchingSingleWhereCondition_WhenApplied(
        string column, object value, string expectedStudentNumbersCsv)
    {
        var query = new Query().From("Student2").Where(column, value);
        var expectedStudentNumbers = string.IsNullOrEmpty(expectedStudentNumbersCsv)
            ? Array.Empty<string>()
            : expectedStudentNumbersCsv.Split(',');

        var rows = await RunQueryAsync(query);

        rows.Select(r => (string)r["StudentNumber"]).Should().BeEquivalentTo(expectedStudentNumbers);
    }

    [Fact]
    public async Task Query_ShouldReturnOnlyMatchingRows_WhenMultipleWhereConditionsAreAppliedTogether()
    {
        var query = new Query().From("Student2").Where("Grade", 18.5).Where("IsMale", true);
        var rows = await RunQueryAsync(query);

        rows.Should().HaveCount(2);
        rows.Select(r => r["FirstName"]).Should().BeEquivalentTo(new[] { "Ali", "Reza" });
    }

    [Fact]
    public async Task Query_ShouldReturnOnlySelectedColumns_WhenSpecificColumnsAreSelected()
    {
        var query = new Query().From("Student2").Select("FirstName").Where("StudentNumber", "99100001");
        var rows = await RunQueryAsync(query);

        rows.Should().ContainSingle();
        rows[0].Keys.Should().BeEquivalentTo("FirstName");
        rows[0]["FirstName"].Should().Be("Ali");
    }

    [Fact]
    public async Task Query_ShouldReturnNoRows_WhenNoRowMatchesTheWhereCondition()
    {
        var query = new Query().From("Student2").Where("Grade", 999);
        var rows = await RunQueryAsync(query);
        rows.Should().BeEmpty();
    }

    [Fact]
    public async Task Query_ShouldPreventSqlInjection_WhenInputContainsSqlStatements()
    {
        string maliciousInput = "Ali'; DROP TABLE [Student2];--";
        var query = new Query().From("Student2").Where("FirstName", maliciousInput);
        var rows = await RunQueryAsync(query);
        rows.Should().BeEmpty();

        var verifyQuery = new Query().From("Student2").Where("StudentNumber", "99100001");
        var verifyRows = await RunQueryAsync(verifyQuery);
        verifyRows.Should().ContainSingle();
    }

    [Fact]
    public async Task Query_ShouldFilterByIsNull_WhenConditionValueIsNull()
    {
        var query = new Query().From("Student2").Where("Grade", (object?)null);
        var rows = await RunQueryAsync(query);

        rows.Should().ContainSingle();
        rows[0]["StudentNumber"].Should().Be("99100005");
    }

    [Fact]
    public async Task Query_ShouldMapNullColumnValue_ToClrNullNotDbNull()
    {
        var query = new Query().From("Student2").Where("StudentNumber", "99100005");
        var rows = await RunQueryAsync(query);

        rows.Should().ContainSingle();
        rows[0]["Grade"].Should().BeNull();
    }
}