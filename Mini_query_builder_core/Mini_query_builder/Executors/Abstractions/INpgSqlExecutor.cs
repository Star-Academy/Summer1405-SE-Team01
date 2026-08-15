using System;
using System.Data.Common;
using Npgsql;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using System.Data;

namespace SqlBuilder.Executors.Abstractions
{
    public interface INpgSqlExecutor
    {
        List<Dictionary<string, object>> ExecuteOnPostgres(CompileResult result, string connectionString);
    }

}