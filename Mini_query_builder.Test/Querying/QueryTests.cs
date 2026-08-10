using System;
using Xunit;
using SqlBuilder.Querying;
using System.Linq;
using FluentAssertions;

namespace SqlBuilder.Tests.Querying
{
    public class QueryTests
    {
        private readonly Query _sut;
        public QueryTests()
        {
            _sut = new Query();
        }

        [Fact]
        public void From_ShouldSetTableNameInContext_WhenCalled()
        {
            // Arrange
            string expectedTable = "Users";

            // Act
            _sut.From(expectedTable);

            // Assert
            _sut.Context.TableName.Should().Be(expectedTable);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void From_ShouldThrowArgumentException_WhenTableNameIsNullOrWhitespace(string? invalidTable)

        {
            // Arrange
            Action act = () => _sut.From(invalidTable!);

            // Act
            // Assert
            act.Should().Throw<ArgumentException>()
            .WithParameterName("table");
        }

        [Fact]
        public void Select_ShouldAddColumnsToContext_WhenCalledWithColumns()
        {
            // Arrange
            var expectedColumns = new[] { "Id", "FirstName", "LastName" };

            // Act
            _sut.Select(expectedColumns);

            // Assert
            _sut.Context.SelectedColumns.Should().BeEquivalentTo(expectedColumns);
        }

        [Theory]
        [MemberData(nameof(InvalidColumnsData))]
        public void Select_ShouldThrowArgumentException_WhenAnyColumnIsNullOrWhitespace(string[] invalidColumns)
        {
            // Arrange
            Action act = () => _sut.Select(invalidColumns);

            // Act
            //ََ Assert
            act.Should().Throw<ArgumentException>();
        }
        public static IEnumerable<object[]> InvalidColumnsData =>
            new List<object[]>
            {
                new object[] { new string[] { "Id", null! } },
                new object[] { new string[] { "Id", "" } },
                new object[] { new string[] { "Id", "   " } }
            };

        [Fact]
        public void Select_ShouldNotAddAnyColumns_WhenCalledWithEmptyArray()
        {
            // Arrange
            var emptyColumns = Array.Empty<string>();

            // Act
            _sut.Select(emptyColumns);

            // Assert
            _sut.Context.SelectedColumns.Should().BeEmpty();
        }

        [Fact]
        public void Where_ShouldAddConditionToContext_WhenCalled()
        {
            // Arrange
            string column = "Age";
            int value = 18;

            // Act
            _sut.Where(column, value);

            // Assert
            _sut.Context.Conditions.Should().ContainSingle()
                .Which.Should().Be((column, value));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Where_ShouldThrowArgumentException_WhenColumnIsNullOrWhitespace(string? invalidColumn)
        {
            // Arrange
            Action act = () => _sut.Where(invalidColumn!, 18);

            // Act
            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void FluentMethods_ShouldReturnSameQueryInstanceAndUpdatesContext_WhenChained()
        {
            // Act
            var result = _sut
                .From("Products")
                .Select("Name", "Price")
                .Where("IsActive", true);

            // Assert

            result.Should().BeSameAs(_sut);
            _sut.Context.TableName.Should().Be("Products");
            _sut.Context.SelectedColumns.Should().HaveCount(2)
                .And.ContainInOrder("Name", "Price");
            _sut.Context.Conditions.Should().ContainSingle()
                .Which.Should().Be(("IsActive", (object)true));
        }
    }
}