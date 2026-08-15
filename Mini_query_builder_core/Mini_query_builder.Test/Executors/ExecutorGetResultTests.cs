using System.Data;
using SqlBuilder.Executors.Implementations;
using FluentAssertions;
using Xunit;

namespace Mini_query_builder.Tests.Executors
{
    public class ExecutorGetResultTests
    {
        private readonly ExecutorGetResult _sut = new();

        [Fact]
        public void GetQueryResult_ShouldReturnEmptyList_WhenReaderHasNoRows()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            using var reader = table.CreateDataReader();
            // Act
            var result = _sut.GetQueryResult(reader);
            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetQueryResult_ShouldMapColumnsByNameAndValue_WhenReaderHasOneRow()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Rows.Add(1, "Ali");
            using var reader = table.CreateDataReader();
            // Act
            var result = _sut.GetQueryResult(reader);
            // Assert
            result.Should().ContainSingle();
            result[0]["Id"].Should().Be(1);
            result[0]["Name"].Should().Be("Ali");
        }

        [Fact]
        public void GetQueryResult_ShouldReturnOneRowPerRecord_WhenReaderHasMultipleRows()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Rows.Add(1);
            table.Rows.Add(2);
            using var reader = table.CreateDataReader();
            // Act
            var result = _sut.GetQueryResult(reader);
            // Assert
            result.Should().HaveCount(2);
            result[0]["Id"].Should().Be(1);
            result[1]["Id"].Should().Be(2);
        }

        [Fact]
        public void GetQueryResult_ShouldMapDbNullToClrNull_WhenReaderContainsNull()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Rows.Add(1, DBNull.Value);
            using var reader = table.CreateDataReader();
            // Act
            var result = _sut.GetQueryResult(reader);
            // Assert
            result.Should().ContainSingle();
            result[0]["Name"].Should().BeNull();
        }

        [Fact]
        public void GetQueryResult_ShouldThrowArgumentNullException_WhenReaderIsNull()
        {
            // Arrange
            IDataReader reader = null!;
            // Act
            var act = () => _sut.GetQueryResult(reader);
            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("reader");
        }
    }
}