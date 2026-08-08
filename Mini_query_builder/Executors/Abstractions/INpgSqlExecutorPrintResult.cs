using System.Data.Common;
using Npgsql;

namespace SqlBuilder.Executors.Abstractions
{
    public interface INpgSqlExecutorPrintResult
    {
        List<string> PrintQueryResult(NpgsqlDataReader reader);
    }
}