using System.Data.Common;
using Npgsql;
using System.Data;

namespace SqlBuilder.Executors.Abstractions
{
    public interface INpgSqlExecutorPrintResult
    {
        List<string> PrintQueryResult(IDataReader reader);
    }
}