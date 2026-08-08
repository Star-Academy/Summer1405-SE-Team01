using System;
using System.Collections.Generic;
using Npgsql;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;



namespace SqlBuilder.Executors.Implementations
{
    internal sealed class NpgSqlExecutorAddParameter : INpgSqlExecutorAddParameter
    {
        public NpgsqlCommand AddParameters(NpgsqlCommand command, CompileResult result)
        {
            foreach (var binding in result.Bindings)
            {
                command.Parameters.Add(new NpgsqlParameter { Value = binding });
            }
            return command;
        }
    }
}