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
            Context.TableName = table;
            return this;
        }

        public Query Select(params string[] columns)
        {
            if (columns != null && columns.Length > 0)
            {
                Context.SelectedColumns.AddRange(columns);
            }
            return this;
        }

        public Query Where(string column, object value)
        {
            Context.Conditions.Add((column, value));
            return this;
        }
    }
}