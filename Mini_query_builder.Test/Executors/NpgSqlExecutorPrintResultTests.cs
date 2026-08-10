using System.Data;
using SqlBuilder.Executors.Implementations;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.ResultRecords;
using FluentAssertions;
using Xunit;


namespace Mini_query_builder.Tests.Executors
{
    public class NpgSqlExecutorPrintResultTests
    {
        private readonly NpgSqlExecutorPrintResult _sut = new();

        [Fact]
        public void PrintQueryResult_ShouldReturnEmptyList_WhenReaderHasNoRows()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            using var reader = table.CreateDataReader();

            var result = _sut.PrintQueryResult(reader);

            result.Should().BeEmpty();
        }

        [Fact]
        public void PrintQueryResult_ShouldReturnOneFormattedLine_WhenReaderHasOneRow()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Rows.Add(1, "Ali");
            using var reader = table.CreateDataReader();

            var result = _sut.PrintQueryResult(reader);

            result.Should().Equal("Id: 1 | Name: Ali");
        }

        [Fact]
        public void PrintQueryResult_ShouldReturnOneLinePerRow_WhenReaderHasMultipleRows()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Rows.Add(1);
            table.Rows.Add(2);
            using var reader = table.CreateDataReader();

            var result = _sut.PrintQueryResult(reader);

            result.Should().Equal("Id: 1", "Id: 2");
        }

        [Fact]
        public void PrintQueryResult_ShouldHandleDbNullValues_WhenReaderContainsNull()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Rows.Add(1, DBNull.Value);
            using var reader = table.CreateDataReader();

            // Act
            var result = _sut.PrintQueryResult(reader);

            // Assert
            result.Should().Equal("Id: 1 | Name: ");
        }
        [Fact]
        public void PrintQueryResult_ShouldThrowArgumentNullException_WhenReaderIsNull()
        {
            // Arrange
            IDataReader reader = null!;

            // Act
            var act = () => _sut.PrintQueryResult(reader);

            // Assert
            act.Should().Throw<ArgumentNullException>()
            .WithParameterName("reader");
        }
    }
}