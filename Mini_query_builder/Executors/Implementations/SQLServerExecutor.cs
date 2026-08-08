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
        public ISQLServerExecutorPrintResult executorPrintResult;
        public ISQLServerExecutorAddParameters executorAddParameter;

        public SQLServerExecutor(ISQLServerExecutorConnection executorConnection, ISQLServerExecutorPrintResult executorPrintResult, ISQLServerExecutorAddParameters executorAddParameter)
        {
            this.executorConnection = executorConnection;
            this.executorPrintResult = executorPrintResult;
            this.executorAddParameter = executorAddParameter;
        }
        public void ExecuteOnSqlServer(CompileResult result, string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            executorConnection.OpeningConnection(connection);

            using var command = new SqlCommand(result.RawQuery, connection);
            executorAddParameter.AddParameters(command, result);

            using var reader = command.ExecuteReader();

            var ResultData = executorPrintResult.PrintQueryResult(reader);
            Console.WriteLine(string.Join("\n", ResultData));
        }
    }
}