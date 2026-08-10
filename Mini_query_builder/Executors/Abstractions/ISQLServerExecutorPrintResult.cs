using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Data;


namespace SqlBuilder.Executors.Abstractions
{
    public interface ISQLServerExecutorPrintResult
    {
        List<string> PrintQueryResult(IDataReader reader);
    }
}