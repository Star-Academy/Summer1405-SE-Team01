using System;
using System.Collections.Generic;
using System.Data;
using SqlBuilder.Executors.Abstractions;

namespace SqlBuilder.Executors.Implementations
{
    internal sealed class NpgSqlExecutorPrintResult : INpgSqlExecutorPrintResult
    {
        public List<string> PrintQueryResult(IDataReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            var resultDataPrint = new List<string>();

            while (reader.Read())
            {
                var rowData = new List<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    var value = reader.GetValue(i);

                    var displayValue = value is DBNull or null ? string.Empty : value.ToString();

                    rowData.Add($"{columnName}: {displayValue}");
                }

                resultDataPrint.Add(string.Join(" | ", rowData));
            }

            return resultDataPrint;
        }
    }
}
