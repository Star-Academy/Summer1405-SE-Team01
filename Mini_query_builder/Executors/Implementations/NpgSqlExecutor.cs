using System;
using System.Collections.Generic;
using Npgsql;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using System.Data;

namespace SqlBuilder.Executors.Implementations
{
    internal sealed class NpgsqlExecutor : INpgSqlExecutor
    {
        public INpgSqlExecutorConnection executorConnection;
        public INpgSqlExecutorPrintResult executorPrintResult;
        public INpgSqlExecutorAddParameter executorAddParameter;
        public INpgSqlCommandExecutor commandExecutor;
        public NpgsqlExecutor(INpgSqlExecutorConnection executorConnection, INpgSqlExecutorPrintResult executorPrintResult, INpgSqlExecutorAddParameter executorAddParameter, INpgSqlCommandExecutor commandExecutor)
        {
            this.executorConnection = executorConnection;
            this.executorPrintResult = executorPrintResult;
            this.executorAddParameter = executorAddParameter;
            this.commandExecutor = commandExecutor;
        }
        public string ExecuteOnPostgres(CompileResult result, string connectionString)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            executorConnection.OpenConnection(connection);

            using var command = new NpgsqlCommand(result.RawQuery, connection);

            executorAddParameter.AddParameters(command, result);

            using var reader = commandExecutor.ExecuteReader(command);

            var ResultData = executorPrintResult.PrintQueryResult(reader);

            return string.Join("\n", ResultData);
        }
    }
}