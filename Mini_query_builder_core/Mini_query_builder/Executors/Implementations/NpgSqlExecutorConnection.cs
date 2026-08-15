using System;
using System.Collections.Generic;
using Npgsql;
using SqlBuilder.Executors.Abstractions;

namespace SqlBuilder.Executors.Implementations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class NpgSqlExecutorConnection : INpgSqlExecutorConnection
    {
        public NpgsqlConnection OpenConnection(NpgsqlConnection connection)
        {
            connection.Open();
            return connection;
        }
    }
}