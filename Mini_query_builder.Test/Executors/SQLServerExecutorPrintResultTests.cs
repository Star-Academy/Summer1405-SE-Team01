using System.Data;
using SqlBuilder.Executors.Implementations;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.ResultRecords;
using SqlBuilder.Querying;
using FluentAssertions;
using Xunit;

namespace Mini_query_builder.Tests.Executors
{
    public class SQLServerExecutorPrintResultTests
    {
        private readonly SQLServerExecutorPrintResult _sut = new();

        [Fact]
        public void PrintQueryResult_ShouldReturnEmptyList_WhenReaderHasNoRows()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            using var reader = table.CreateDataReader();

            // Act
            var result = _sut.PrintQueryResult(reader);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void PrintQueryResult_ShouldReturnOneFormattedLine_WhenReaderHasOneRow()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Rows.Add(1, "Ali");
            using var reader = table.CreateDataReader();

            // Act
            var result = _sut.PrintQueryResult(reader);

            // Assert
            result.Should().Equal("Id: 1 | Name: Ali");
        }

        [Fact]
        public void PrintQueryResult_ShouldReturnOneLinePerRow_WhenReaderHasMultipleRows()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Rows.Add(1);
            table.Rows.Add(2);
            using var reader = table.CreateDataReader();

            // Act
            var result = _sut.PrintQueryResult(reader);

            // Assert
            result.Should().Equal("Id: 1", "Id: 2");
        }
    }
}