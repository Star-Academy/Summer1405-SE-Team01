using System;
using System.Collections.Generic;
using Npgsql;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using System.Data;



namespace SqlBuilder.Executors.Implementations
{
    internal sealed class NpgSqlCommandExecutor : INpgSqlCommandExecutor
    {
        public IDataReader ExecuteReader(NpgsqlCommand command) => command.ExecuteReader();
    }
}