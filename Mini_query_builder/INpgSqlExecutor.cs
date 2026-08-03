using System;
using System.Data.Common;
using Npgsql;

namespace SqlBuilder
{
    public interface INpgSqlExecutor
    {
        void ExecuteOnPostgres(CompileResult result, string connectionString);
        void OpeningConnection(NpgsqlConnection connection);
        void AddParameters(NpgsqlCommand command, CompileResult result);
        void PrintQueryResult(NpgsqlDataReader reader);
    }
}