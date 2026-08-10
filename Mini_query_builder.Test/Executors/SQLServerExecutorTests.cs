// SQLServerExecutorTests.cs
using System.Data;
using Microsoft.Data.SqlClient;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Executors.Implementations;
using SqlBuilder.ResultRecords;
using SqlBuilder.Querying;
using NSubstitute;
using FluentAssertions;

namespace Mini_query_builder.Tests.Executors
{
    public class SQLServerExecutorTests
    {
        private readonly ISQLServerExecutorConnection _executorConnection = Substitute.For<ISQLServerExecutorConnection>();
        private readonly ISQLServerExecutorPrintResult _executorPrintResult = Substitute.For<ISQLServerExecutorPrintResult>();
        private readonly ISQLServerExecutorAddParameters _executorAddParameter = Substitute.For<ISQLServerExecutorAddParameters>();
        private readonly ISqlServerCommandExecutor _commandExecutor = Substitute.For<ISqlServerCommandExecutor>();
        private readonly IDataReader _fakeReader = Substitute.For<IDataReader>();
        private readonly SQLServerExecutor _sut;

        public SQLServerExecutorTests()
        {
            _sut = new SQLServerExecutor(_executorConnection, _executorPrintResult, _executorAddParameter, _commandExecutor);

            _commandExecutor.ExecuteReader(Arg.Any<SqlCommand>()).Returns(_fakeReader);
            _executorPrintResult.PrintQueryResult(Arg.Any<IDataReader>()).Returns(new List<string>());
        }

        [Fact]
        public void ExecuteOnSqlServer_ShouldOpenTheConnection_WhenCalled()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnSqlServer(result, "Server=test;Database=test;");

            // Assert
            _executorConnection.Received(1).OpeningConnection(Arg.Any<SqlConnection>());
        }

        [Fact]
        public void ExecuteOnSqlServer_ShouldAddParametersToTheCommand_WhenCalled()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object> { 1 } };

            // Act
            _sut.ExecuteOnSqlServer(result, "Server=test;Database=test;");

            // Assert
            _executorAddParameter.Received(1).AddParameters(Arg.Any<SqlCommand>(), result);
        }

        [Fact]
        public void ExecuteOnSqlServer_ShouldExecuteTheCommand_WhenCalled()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnSqlServer(result, "Server=test;Database=test;");

            // Assert
            _commandExecutor.Received(1).ExecuteReader(Arg.Any<SqlCommand>());
        }

        [Fact]
        public void ExecuteOnSqlServer_ShouldPrintTheReaderReturnedByCommandExecutor_WhenCalled()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnSqlServer(result, "Server=test;Database=test;");

            // Assert
            _executorPrintResult.Received(1).PrintQueryResult(_fakeReader);
        }
    }
}