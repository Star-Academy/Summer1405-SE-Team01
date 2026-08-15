using System;
using System.Collections.Generic;
using System.Data;
using SqlBuilder.Executors.Abstractions;

namespace SqlBuilder.Executors.Implementations
{
    internal sealed class ExecutorGetResult : IExecutorGetResult
    {
        public List<Dictionary<string, object>> GetQueryResult(IDataReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            var rows = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    var value = reader.GetValue(i);

                    row[columnName] = value is DBNull ? null : value;
                }

                rows.Add(row);
            }

            return rows;
        }
    }
}
