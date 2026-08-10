using System;
using System.Collections.Generic;
using Npgsql;
using SqlBuilder.Executors.Abstractions;
using System.Data;

namespace SqlBuilder.Executors.Implementations
{
    internal sealed class NpgSqlExecutorPrintResult : INpgSqlExecutorPrintResult
    {
        public List<string> PrintQueryResult(IDataReader reader)
        {
            var ResultDataPrint = new List<string>();
            while (reader.Read())
            {

                var RowData = new List<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);

                    var value = reader.GetValue(i);

                    RowData.Add($"{columnName}: {value}");
                }
                ResultDataPrint.Add(string.Join(" | ", RowData));
            }
            return ResultDataPrint;
        }
    }
}