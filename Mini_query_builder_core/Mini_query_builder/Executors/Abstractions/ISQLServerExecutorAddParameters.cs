using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.Executors.Abstractions
{
    public interface ISQLServerExecutorAddParameters
    {
        SqlCommand AddParameters(SqlCommand command, CompileResult result);

    }
}