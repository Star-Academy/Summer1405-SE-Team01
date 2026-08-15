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
        public IExecutorGetResult executorGetResult;
        public INpgSqlExecutorAddParameter executorAddParameter;
        public INpgSqlCommandExecutor commandExecutor;
        public NpgsqlExecutor(INpgSqlExecutorConnection executorConnection, IExecutorGetResult executorGetResult, INpgSqlExecutorAddParameter executorAddParameter, INpgSqlCommandExecutor commandExecutor)
        {
            this.executorConnection = executorConnection;
            this.executorGetResult = executorGetResult;
            this.executorAddParameter = executorAddParameter;
            this.commandExecutor = commandExecutor;
        }
        public List<Dictionary<string, object>> ExecuteOnPostgres(CompileResult result, string connectionString)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            executorConnection.OpenConnection(connection);

            using var command = new NpgsqlCommand(result.RawQuery, connection);

            executorAddParameter.AddParameters(command, result);

            using var reader = commandExecutor.ExecuteReader(command);
            
            return executorGetResult.GetQueryResult(reader);
        }
    }
}