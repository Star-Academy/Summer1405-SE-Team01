using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using SqlBuilder.Querying;
using System.Data;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.Executors.Abstractions
{
    public interface ISQLServerExecutor
    {
        string ExecuteOnSqlServer(CompileResult result, string connectionString);
    }
}