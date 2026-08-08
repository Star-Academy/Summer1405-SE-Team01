using System.Data.Common;
using Npgsql;

namespace SqlBuilder.Executors.Abstractions
{
    public interface INpgSqlExecutorConnection
    {
        NpgsqlConnection OpenConnection(NpgsqlConnection connection);
    }
}