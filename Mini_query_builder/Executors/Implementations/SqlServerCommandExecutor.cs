using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.Executors.Implementations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class SqlServerCommandExecutor : ISqlServerCommandExecutor
    {
        public IDataReader ExecuteReader(SqlCommand command) => command.ExecuteReader();
    }
}