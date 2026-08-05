using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace SqlBuilder
{
    public interface ISQLServerExecutorHandler
    {
        void OpeningConnection(SqlConnection connection);
        void AddParameters(SqlCommand command, CompileResult result);
        void PrintQueryResult(SqlDataReader reader);
    }
}