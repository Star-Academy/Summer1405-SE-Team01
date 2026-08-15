using System.Data.Common;
using Npgsql;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.Executors.Abstractions
{
    public interface INpgSqlExecutorAddParameter
    {
        NpgsqlCommand AddParameters(NpgsqlCommand command, CompileResult result);
    }
}