using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlBuilder
{
    public class QueryContext
    {
        public string TableName { get; set; } = string.Empty;
        public List<string> SelectedColumns { get; set; } = new();
        public List<(string Column, object Value)> Conditions { get; set; } = new(); 
    }
    public class Query
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