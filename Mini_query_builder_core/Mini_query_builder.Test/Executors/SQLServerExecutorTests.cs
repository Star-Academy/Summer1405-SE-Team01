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
        private readonly ISQLServerExecutorConnection _sqlServerExecutorConnection = Substitute.For<ISQLServerExecutorConnection>();
        private readonly IExecutorGetResult _executorGetResult = Substitute.For<IExecutorGetResult>();
        private readonly ISQLServerExecutorAddParameters _sqlServerExecutorAddParameters = Substitute.For<ISQLServerExecutorAddParameters>();
        private readonly ISqlServerCommandExecutor _sqlServerCommandExecutor = Substitute.For<ISqlServerCommandExecutor>();
        private readonly IDataReader _dataReader = Substitute.For<IDataReader>();
        private readonly SQLServerExecutor _sut;

        public SQLServerExecutorTests()
        {
            _sut = new SQLServerExecutor(_sqlServerExecutorConnection, _executorGetResult, _sqlServerExecutorAddParameters, _sqlServerCommandExecutor);

            _sqlServerCommandExecutor.ExecuteReader(Arg.Any<SqlCommand>()).Returns(_dataReader);
            _executorGetResult.GetQueryResult(Arg.Any<IDataReader>()).Returns(new List<Dictionary<string, object>>());        }

        [Fact]
        public void ExecuteOnSqlServer_ShouldOpenTheConnection_Whenever()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnSqlServer(result, "Server=test;Database=test;");

            // Assert
            _sqlServerExecutorConnection.Received(1).OpeningConnection(Arg.Any<SqlConnection>());
        }

        [Fact]
        public void ExecuteOnSqlServer_ShouldAddParametersToTheCommand_Whenever()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object> { 1 } };

            // Act
            _sut.ExecuteOnSqlServer(result, "Server=test;Database=test;");

            // Assert
            _sqlServerExecutorAddParameters.Received(1).AddParameters(Arg.Any<SqlCommand>(), result);
        }

        [Fact]
        public void ExecuteOnSqlServer_ShouldExecuteTheCommand_Whenever()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnSqlServer(result, "Server=test;Database=test;");

            // Assert
            _sqlServerCommandExecutor.Received(1).ExecuteReader(Arg.Any<SqlCommand>());
        }

        [Fact]
        public void ExecuteOnSqlServer_ShouldPrintTheReaderReturnedByCommandExecutor_Whenever()
        {
            // Arrange
            var result = new CompileResult { RawQuery = "SELECT 1", Bindings = new List<object>() };

            // Act
            _sut.ExecuteOnSqlServer(result, "Server=test;Database=test;");

            // Assert
            _executorGetResult.Received(1).GetQueryResult(_dataReader);        }
    }
}