using System;
using System.Data.Common;
using Npgsql;

namespace SqlBuilder
{
    public interface INpgSqlExecutor: INpgSqlExecutorHandler
    {
        void ExecuteOnPostgres(CompileResult result, string connectionString);
    }
    
}