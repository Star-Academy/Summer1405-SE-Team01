using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlBuilder.Querying
{
    public sealed class Query
    {
        public QueryContext Context { get; private set; } = new QueryContext();

        public Query From(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                throw new ArgumentException("Table name cannot be null or empty.", nameof(table));
            }

            Context.TableName = table;
            return this;
        }
        public Query Select(params string[] columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }
            if (columns.Any(c => string.IsNullOrWhiteSpace(c)))
            {
                throw new ArgumentException("Column names cannot be null or whitespace.", nameof(columns));
            }
            if (columns.Length > 0)
            {
                Context.SelectedColumns.AddRange(columns);
            }

            return this;
        }
        public Query Where(string column, object value)
        {
            if (string.IsNullOrWhiteSpace(column))
            {
                throw new ArgumentException("Column name cannot be null or whitespace.", nameof(column));
            }

            Context.Conditions.Add((column, value));
            return this;
        }
    }
}