using System;
using System.Collections.Generic;
using Npgsql;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using System.Data;

namespace SqlBuilder.Executors.Implementations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class NpgSqlCommandExecutor : INpgSqlCommandExecutor
    {
        public IDataReader ExecuteReader(NpgsqlCommand command) => command.ExecuteReader();
    }
}