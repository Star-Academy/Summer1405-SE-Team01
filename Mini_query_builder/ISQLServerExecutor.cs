using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace SqlBuilder
{
    public interface ISQLServerExecutor: ISQLServerExecutorHandler
    {
        void ExecuteOnSqlServer(CompileResult result, string connectionString);
    }
}