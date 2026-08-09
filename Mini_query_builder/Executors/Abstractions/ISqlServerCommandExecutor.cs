using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.Executors.Abstractions
{
    public interface ISqlServerCommandExecutor
    {
        IDataReader ExecuteReader(SqlCommand command);
    }
}

