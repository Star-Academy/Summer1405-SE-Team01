using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.Executors.Implementations
{
    internal sealed class SQLServerExecutorAddParameters : ISQLServerExecutorAddParameters
    {
        public SqlCommand AddParameters(SqlCommand command, CompileResult result)
        {
            for (var i = 0; i < result.Bindings.Count; i++)
            {
                command.Parameters.AddWithValue($"@p{i + 1}", result.Bindings[i]);
            }
            return command;
        }
    }
}