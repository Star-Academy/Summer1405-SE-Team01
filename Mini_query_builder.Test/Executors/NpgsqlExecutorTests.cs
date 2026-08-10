using System.Data;
using Npgsql;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Executors.Implementations;
using SqlBuilder.ResultRecords;

using NSubstitute;
using FluentAssertions;

namespace Mini_query_builder.Tests.Executors
{
    public class NpgsqlExecutorTests
    {
        private readonly INpgSqlExecutorConnection _executorConnection = Substitute.For<INpgSqlExecutorConnection>();
        private readonly INpgSqlExecutorPrintResult _executorPrintResult = Substitute.For<INpgSqlExecutorPrintResult>();
        private readonly INpgSqlExecutorAddParameter _executorAddParameter = Substitute.For<INpgSqlExecutorAddParameter>();
        private readonly INpgSqlCommandExecutor _commandExecutor = Substitute.For<INpgSqlCommandExecutor>();
        private readonly IDataReader _fakeReader = Substitute.For<IDataReader>();
        private readonly NpgsqlExecutor _sut;

        public NpgsqlExecutorTests()
        {
            _sut = new NpgsqlExecutor(_executorConnection, _executorPrintResult, _executorAddParameter, _commandExecutor);

            _commandExecutor.ExecuteReader(Arg.Any<NpgsqlCommand>()).Returns(_fakeReader);
            _executorPrintResult.PrintQueryResult(Arg.Any<IDataReader>()).Returns(new List<string>());
        }

        [Fact]
        public void ExecuteOnPostgres_Should_OpenTheConnection_When_Called()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnPostgres(result, "Host=test;Database=test;");

            // Assert
            _executorConnection.Received(1).OpenConnection(Arg.Any<NpgsqlConnection>());
        }

        [Fact]
        public void ExecuteOnPostgres_Should_AddParametersToTheCommand_When_Called()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object> { 1 } };

            // Act
            _sut.ExecuteOnPostgres(result, "Host=test;Database=test;");

            // Assert
            _executorAddParameter.Received(1).AddParameters(Arg.Any<NpgsqlCommand>(), result);
        }

        [Fact]
        public void ExecuteOnPostgres_Should_ExecuteTheCommand_When_Called()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnPostgres(result, "Host=test;Database=test;");

            // Assert
            _commandExecutor.Received(1).ExecuteReader(Arg.Any<NpgsqlCommand>());
        }

        [Fact]
        public void ExecuteOnPostgres_Should_ReturnJoinedResultLines_When_PrintResultReturnsMultipleLines()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };
            _executorPrintResult.PrintQueryResult(Arg.Any<IDataReader>())
                .Returns(new List<string> { "Id: 1", "Id: 2" });

            // Act
            var output = _sut.ExecuteOnPostgres(result, "Host=test;Database=test;");

            // Assert
            output.Should().Be("Id: 1\nId: 2");
        }
    }
}