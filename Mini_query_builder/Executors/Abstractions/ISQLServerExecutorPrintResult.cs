using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace SqlBuilder.Executors.Abstractions
{
    public interface ISQLServerExecutorPrintResult
    {
        List<string> PrintQueryResult(SqlDataReader reader);
    }
}