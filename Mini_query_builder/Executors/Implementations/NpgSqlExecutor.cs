using System;
using System.Collections.Generic;
using Npgsql;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;



namespace SqlBuilder.Executors.Implementations
{
    internal sealed class NpgsqlExecutor : INpgSqlExecutor
    {
        public INpgSqlExecutorConnection executorConnection;
        public INpgSqlExecutorPrintResult executorPrintResult;
        public INpgSqlExecutorAddParameter executorAddParameter;
        public NpgsqlExecutor(INpgSqlExecutorConnection executorConnection, INpgSqlExecutorPrintResult executorPrintResult, INpgSqlExecutorAddParameter executorAddParameter)
        {
            this.executorConnection = executorConnection;
            this.executorPrintResult = executorPrintResult;
            this.executorAddParameter = executorAddParameter;
        }
        public string ExecuteOnPostgres(CompileResult result, string connectionString)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            executorConnection.OpenConnection(connection);

            using var command = new NpgsqlCommand(result.RawQuery, connection);

            executorAddParameter.AddParameters(command, result);

            using var reader = command.ExecuteReader();

            var ResultData = executorPrintResult.PrintQueryResult(reader);

            return string.Join("\n", ResultData);
        }
    }
}