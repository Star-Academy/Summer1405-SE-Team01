using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SqlBuilder.Executors.Abstractions
{
    public interface IExecutorGetResult
    {
        List<Dictionary<string, object>> GetQueryResult(IDataReader reader);
    }
}