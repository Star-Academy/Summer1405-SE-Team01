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
        ISQLServerExecutorConnection executorConnection = new SQLServerExecutorConnection();
        ISQLServerExecutorPrintResult executorPrintResult = new SQLServerExecutorPrintResult();
        ISQLServerExecutorAddParameters executorAddParameter = new SQLServerExecutorAddParameters();
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