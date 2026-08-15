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
        private readonly INpgSqlExecutorConnection _npgSqlExecutorConnection = Substitute.For<INpgSqlExecutorConnection>();
        private readonly IExecutorGetResult _executorGetResult = Substitute.For<IExecutorGetResult>();
        private readonly INpgSqlExecutorAddParameter _npgSqlExecutorAddParameter = Substitute.For<INpgSqlExecutorAddParameter>();
        private readonly INpgSqlCommandExecutor _npgSqlCommandExecutor = Substitute.For<INpgSqlCommandExecutor>();
        private readonly IDataReader _dataReader = Substitute.For<IDataReader>();
        private readonly NpgsqlExecutor _sut;

        public NpgsqlExecutorTests()
        {
            _sut = new NpgsqlExecutor(_npgSqlExecutorConnection, _executorGetResult, _npgSqlExecutorAddParameter, _npgSqlCommandExecutor);

            _npgSqlCommandExecutor.ExecuteReader(Arg.Any<NpgsqlCommand>()).Returns(_dataReader);
            _executorGetResult.GetQueryResult(Arg.Any<IDataReader>()).Returns(new List<Dictionary<string, object>>());        }

        [Fact]
        public void ExecuteOnPostgres_ShouldOpenTheConnection_Whenever()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnPostgres(result, "Host=test;Database=test;");

            // Assert
            _npgSqlExecutorConnection.Received(1).OpenConnection(Arg.Any<NpgsqlConnection>());
        }

        [Fact]
        public void ExecuteOnPostgres_ShouldAddParametersToTheCommand_Whenever()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object> { 1 } };

            // Act
            _sut.ExecuteOnPostgres(result, "Host=test;Database=test;");

            // Assert
            _npgSqlExecutorAddParameter.Received(1).AddParameters(Arg.Any<NpgsqlCommand>(), result);
        }

        [Fact]
        public void ExecuteOnPostgres_ShouldExecuteTheCommand_Whenever()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnPostgres(result, "Host=test;Database=test;");

            // Assert
            _npgSqlCommandExecutor.Received(1).ExecuteReader(Arg.Any<NpgsqlCommand>());
        }

        [Fact]
        public void ExecuteOnPostgres_ShouldReturnJoinedResultLines_WhenPrintResultReturnsMultipleLines()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };
            var expectedRows = new List<Dictionary<string, object>>
            {
                new() { ["Id"] = 1 },
                new() { ["Id"] = 2 }
            };
            _executorGetResult.GetQueryResult(Arg.Any<IDataReader>()).Returns(expectedRows);
            
            // Act
            var output = _sut.ExecuteOnPostgres(result, "Host=test;Database=test;");
            
            // Assert
            output.Should().BeEquivalentTo(expectedRows);
        }
    }
}