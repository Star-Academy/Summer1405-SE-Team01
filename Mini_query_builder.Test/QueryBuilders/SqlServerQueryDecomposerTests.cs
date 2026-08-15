using SqlBuilder.QueryBuilders.Implementations;
using SqlBuilder.QueryBuilders.Abstractions;
using System;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using FluentAssertions;
using Xunit;


namespace Mini_query_builder.Tests.QueryBuilders
{
    public class SqlServerQueryDecomposerTests
    {
        private readonly SqlServerQueryDecomposer _sut;
        public SqlServerQueryDecomposerTests()
        {
            _sut = new SqlServerQueryDecomposer();
        }

        [Fact]
        public void SelectClause_Should_ReturnWildcard_WhenNoColumnsAreSelected()
        {
            // Arrange
            var query = new Query().From("Student");

            // Act
            var result = _sut.selectClause(query);

            // Assert
            result.Should().Be("SELECT *");
        }

        [Fact]
        public void SelectClause_Should_ReturnSingleBracketedColumn_WhenOneColumnIsSelected()
        {
            // Arrange
            var query = new Query().From("Student").Select("FirstName");

            // Act
            var result = _sut.selectClause(query);

            // Assert
            result.Should().Be("SELECT [FirstName]");
        }

        [Fact]
        public void SelectClause_Should_ReturnCommaSeparatedBracketedColumns_WhenMultipleColumnsAreSelected()
        {
            // Arrange
            var query = new Query().From("Student").Select("FirstName", "LastName");

            // Act
            var result = _sut.selectClause(query);

            // Assert
            result.Should().Be("SELECT [FirstName], [LastName]");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void FromClause_ShouldThrowArgumentException_WhenTableNameIsNullOrWhitespace(string? invalidTable)
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
        public void FromClause_Should_ReturnBracketedTableName_WhenTableNameIsSet()
        {
            // Arrange
            var query = new Query().From("Student");

            // Act
            var result = _sut.fromClause(query);

            // Assert
            result.Should().Be("FROM [Student]");
        }

        [Fact]
        public void WhereClause_Should_ReturnEmptyResult_WhenNoConditionsExist()
        {
            // Arrange
            var query = new Query().From("Student");

            // Act
            var result = _sut.whereClause(query);

            // Assert
            result.Should().BeEquivalentTo(new CompileResult
            {
                RawQuery = string.Empty,
                Bindings = []
            });
        }

        [Fact]
        public void WhereClause_Should_ReturnSingleParameterizedCondition_WhenOneConditionExists()
        {
            // Arrange
            var query = new Query().From("Student").Where("LastName", "Smith");

            // Act
            var result = _sut.whereClause(query);

            // Assert
            result.Should().BeEquivalentTo(new CompileResult
            {
                RawQuery = " WHERE [LastName] = @p1",
                Bindings = ["Smith"]
            });
        }

        [Fact]
        public void WhereClause_Should_JoinConditionsWithAnd_WhenMultipleConditionsExist()
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
                RawQuery = " WHERE [LastName] = @p1 AND [Grade] = @p2",
                Bindings = ["Smith", 12]
            });
        }

        [Fact]
        public void WhereClause_Should_ConvertBooleanTrueToOne_WhenConditionValueIsTrue()
        {
            // Arrange
            var query = new Query().From("Student").Where("IsMale", true);

            // Act
            var result = _sut.whereClause(query);

            // Assert
            result.Should().BeEquivalentTo(new CompileResult
            {
                RawQuery = " WHERE [IsMale] = @p1",
                Bindings = [1]
            });
        }

        [Fact]
        public void WhereClause_Should_ConvertBooleanFalseToZero_WhenConditionValueIsFalse()
        {
            // Arrange
            var query = new Query().From("Student").Where("IsMale", false);

            // Act
            var result = _sut.whereClause(query);

            // Assert
            result.Should().BeEquivalentTo(new CompileResult
            {
                RawQuery = " WHERE [IsMale] = @p1",
                Bindings = [0]
            });
        }

        [Fact]
        public void WhereClause_Should_HandleNullValueCorrectly_WhenConditionValueIsNull()
        {
            // Arrange
            var query = new Query().From("Student").Where("Email", null!);

            // Act
            var result = _sut.whereClause(query);

            // Assert
            result.Should().BeEquivalentTo(new CompileResult
            {
                RawQuery = " WHERE [Email] IS NULL",
                Bindings = []
            });
        }
    }
}
