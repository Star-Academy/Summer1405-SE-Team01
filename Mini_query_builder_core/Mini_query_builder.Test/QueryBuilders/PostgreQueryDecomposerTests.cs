using System;
using System.Collections.Generic;
using SqlBuilder.QueryBuilders.Implementations;
using SqlBuilder.QueryBuilders.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using FluentAssertions;
using Xunit;

namespace Mini_query_builder.Tests.QueryBuilders
{
    public class PostgreQueryDecomposerTests
    {
        private readonly PostgreQueryDecomposer _sut;

        public PostgreQueryDecomposerTests()
        {
            _sut = new PostgreQueryDecomposer();
        }

        [Fact]
        public void SelectClause_ShouldReturnWildcard_WhenNoColumnsAreSelected()
        {
            // Arrange
            var query = new Query().From("Student");

            // Act
            var result = _sut.selectClause(query);

            // Assert
            result.Should().Be("SELECT *");
        }

        [Fact]
        public void SelectClause_ShouldReturnSingleDoubleQuotedColumn_WhenOneColumnIsSelected()
        {
            // Arrange
            var query = new Query().From("Student").Select("FirstName");

            // Act
            var result = _sut.selectClause(query);

            // Assert
            result.Should().Be("SELECT \"FirstName\"");
        }

        [Fact]
        public void SelectClause_ShouldReturnCommaSeparatedDoubleQuotedColumns_WhenMultipleColumnsAreSelected()
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
        public void FromClause_ShouldReturnDoubleQuotedTableName_WhenTableNameIsSet()
        {
            // Arrange
            var query = new Query().From("Student");

            // Act
            var result = _sut.fromClause(query);

            // Assert
            result.Should().Be("FROM \"Student\"");
        }

        [Fact]
        public void WhereClause_ShouldReturnEmptyResult_WhenNoConditionsExist()
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
        public void WhereClause_ShouldReturnSingleParameterizedCondition_WhenOneConditionExists()
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
        public void WhereClause_ShouldJoinConditionsWithAnd_WhenMultipleConditionsExist()
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
        public void WhereClause_ShouldKeepBooleanValueUnconverted_WhenConditionValueIsBoolean()
        {
            // Arrange
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

        [Fact]
        public void WhereClause_ShouldHandleNullValueCorrectly_WhenConditionValueIsNull()
        {
            // Arrange
            var query = new Query().From("Student").Where("Email", null!);

            // Act
            var result = _sut.whereClause(query);

            // Assert
            result.Should().BeEquivalentTo(new CompileResult
            {
                RawQuery = " WHERE \"Email\" IS NULL",
                Bindings = new List<object>()
            });
        }
    }
}