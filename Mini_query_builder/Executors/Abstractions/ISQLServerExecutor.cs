using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.Executors.Abstractions
{
    public interface ISQLServerExecutor
    {
        void ExecuteOnSqlServer(CompileResult result, string connectionString);
    }
}