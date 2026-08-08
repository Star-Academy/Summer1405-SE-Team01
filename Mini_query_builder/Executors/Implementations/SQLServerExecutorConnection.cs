using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using SqlBuilder.Executors.Abstractions;

namespace SqlBuilder.Executors.Implementations
{
    internal sealed class SQLServerExecutorConnection : ISQLServerExecutorConnection
    {
        public SqlConnection OpeningConnection(SqlConnection connection)
        {
            connection.Open();
            return connection;
        }
    }
}