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
        string ExecuteOnPostgres(CompileResult result, string connectionString);
    }

}