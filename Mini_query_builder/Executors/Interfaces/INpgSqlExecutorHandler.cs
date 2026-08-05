using System.Data.Common;
using Npgsql;

namespace SqlBuilder
{
    public interface INpgSqlExecutorHandler
    {
        void OpeningConnection(NpgsqlConnection connection);
        void AddParameters(NpgsqlCommand command, CompileResult result);
        void PrintQueryResult(NpgsqlDataReader reader);
    }
}