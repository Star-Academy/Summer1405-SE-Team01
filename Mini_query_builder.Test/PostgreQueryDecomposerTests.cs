using SqlBuilder.QueryBuilders.Implementations;
using SqlBuilder.QueryBuilders.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using FluentAssertions;
using Xunit;


namespace Mini_query_builder.Tests;

public class PostgreQueryDecomposerTests
{
    private readonly PostgreQueryDecomposer _sut;
    public PostgreQueryDecomposerTests()
    {
        _sut = new PostgreQueryDecomposer();
    }


    [Fact]
    public void selectClause_ShouldReturnWildcard_WhenNoColumnsAreSelected()
    {
        // Arrange
        var query = new Query().From("Student");

        // Act
        var result = _sut.selectClause(query);

        // Assert
        result.Should().Be("SELECT *");
    }

    [Fact]
    public void selectClause_ShouldReturnSingleDoubleQuotedColumn_WhenOneColumnIsSelected()
    {
        // Arrange
        var query = new Query().From("Student").Select("FirstName");

        // Act
        var result = _sut.selectClause(query);

        // Assert
        result.Should().Be("SELECT \"FirstName\"");
    }

    [Fact]
    public void selectClause_ShouldReturnCommaSeparatedDoubleQuotedColumns_WhenMultipleColumnsAreSelected()
    {
        // Arrange
        var query = new Query().From("Student").Select("FirstName", "LastName");

        // Act
        var result = _sut.selectClause(query);

        // Assert
        result.Should().Be("SELECT \"FirstName\", \"LastName\"");
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void fromClause_ShouldThrowArgumentException_WhenTableNameIsNullOrWhitespace(string? invalidTable)
    {
        // Arrange
        var query = new Query();
        query.Context.TableName = invalidTable!;

        // Act
        Action act = () => _sut.fromClause(query);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void fromClause_ShouldReturnDoubleQuotedTableName_WhenTableNameIsSet()
    {
        // Arrange
        var query = new Query().From("Student");

        // Act
        var result = _sut.fromClause(query);

        // Assert
        result.Should().Be("FROM \"Student\"");
    }



    [Fact]
    public void whereClause_ShouldReturnEmptyResult_WhenNoConditionsExist()
    {
        // Arrange
        var query = new Query().From("Student");

        // Act
        var result = _sut.whereClause(query);

        // Assert
        result.Should().BeEquivalentTo(new CompileResult
        {
            RawQuery = string.Empty,
            Bindings = new List<object>()
        });
    }

    [Fact]
    public void whereClause_ShouldReturnSingleParameterizedCondition_WhenOneConditionExists()
    {
        // Arrange
        var query = new Query().From("Student").Where("LastName", "Smith");

        // Act
        var result = _sut.whereClause(query);

        // Assert
        result.Should().BeEquivalentTo(new CompileResult
        {
            RawQuery = " WHERE \"LastName\" = $1",
            Bindings = new List<object> { "Smith" }
        });
    }

    [Fact]
    public void whereClause_ShouldJoinConditionsWithAnd_WhenMultipleConditionsExist()
    {
        // Arrange
        var query = new Query()
            .From("Student")
            .Where("LastName", "Smith")
            .Where("Grade", 12);

        // Act
        var result = _sut.whereClause(query);

        // Assert
        result.Should().BeEquivalentTo(new CompileResult
        {
            RawQuery = " WHERE \"LastName\" = $1 AND \"Grade\" = $2",
            Bindings = new List<object> { "Smith", 12 }
        });
    }

    [Fact]
    public void whereClause_ShouldKeepBooleanValueUnconverted_WhenConditionValueIsBoolean()
    {
        // Arrange
        // Unlike SqlServerQueryDecomposer, Postgres does NOT convert bool -> 1/0.
        var query = new Query().From("Student").Where("IsMale", true);

        // Act
        var result = _sut.whereClause(query);

        // Assert
        result.Should().BeEquivalentTo(new CompileResult
        {
            RawQuery = " WHERE \"IsMale\" = $1",
            Bindings = new List<object> { true }
        });
    }
}
