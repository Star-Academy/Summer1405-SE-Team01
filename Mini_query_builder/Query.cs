using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlBuilderLibrary
{
    public class Query
    {
        public string TableName { get; private set; } = string.Empty;
        public List<string> SelectedColumns { get; private set; } = new();

        public List<(string Column, object Value)> Conditions { get; private set; } = new();

        public Query From(string table)
        {
            TableName = table;
            return this;
        }
        public Query Select(params string[] columns)
        {
            if (columns != null && columns.Length > 0)
            {
                SelectedColumns.AddRange(columns);
            }
            return this;
        }
        public Query Where(string column, object value)
        {
            Conditions.Add((column, value));
            return this;
        }
    }
}