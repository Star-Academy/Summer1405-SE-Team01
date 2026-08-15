using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using SqlBuilder.Executors.Abstractions;
using System.Data;

namespace SqlBuilder.Executors.Implementations
{
    internal sealed class SQLServerExecutorPrintResult : ISQLServerExecutorPrintResult
    {
        public List<string> PrintQueryResult(IDataReader reader)
        {
            var ResultDataPrint = new List<string>();

            while (reader.Read())
            {
                var rowData = new System.Collections.Generic.List<string>();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    var value = reader.GetValue(i);
                    rowData.Add($"{columnName}: {value}");
                }
                ResultDataPrint.Add(string.Join(" | ", rowData));
            }
            return ResultDataPrint;
        }
    }
}