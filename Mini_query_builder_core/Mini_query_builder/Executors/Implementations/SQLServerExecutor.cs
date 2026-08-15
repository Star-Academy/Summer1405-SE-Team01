using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.Executors.Implementations
{
    internal sealed class SQLServerExecutor : ISQLServerExecutor
    {
        public ISQLServerExecutorConnection executorConnection;
        public IExecutorGetResult executorGetResult;
        public ISQLServerExecutorAddParameters executorAddParameter;
        public ISqlServerCommandExecutor commandExecutor;

        public SQLServerExecutor(
            ISQLServerExecutorConnection executorConnection,
            IExecutorGetResult executorGetResult,
            ISQLServerExecutorAddParameters executorAddParameter,
            ISqlServerCommandExecutor commandExecutor)
        {
            this.executorConnection = executorConnection;
            this.executorGetResult = executorGetResult;
            this.executorAddParameter = executorAddParameter;
            this.commandExecutor = commandExecutor;
        }
        public List<Dictionary<string, object>> ExecuteOnSqlServer(CompileResult result, string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            executorConnection.OpeningConnection(connection);

            using var command = new SqlCommand(result.RawQuery, connection);
            executorAddParameter.AddParameters(command, result);

            using var reader = commandExecutor.ExecuteReader(command);

            return executorGetResult.GetQueryResult(reader);
        }
    }
}