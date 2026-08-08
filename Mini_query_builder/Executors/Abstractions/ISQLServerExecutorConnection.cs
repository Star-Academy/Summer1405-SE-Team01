using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace SqlBuilder.Executors.Abstractions
{
    public interface ISQLServerExecutorConnection
    {
        SqlConnection OpeningConnection(SqlConnection connection);
    }
}