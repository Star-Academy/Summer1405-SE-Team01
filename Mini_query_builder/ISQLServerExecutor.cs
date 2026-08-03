using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
namespace SqlBuilder
{
    public interface ISQLServerExecutor
    {
        void ExecuteOnSqlServer(CompileResult result, string connectionString);
        void OpeningConnection(SqlConnection connection);
        void AddParameters(SqlCommand command, CompileResult result);
        void PrintQueryResult(SqlDataReader reader);
    }
}