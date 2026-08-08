using System;
using System.Collections.Generic;

namespace SqlBuilder.Querying
{
    public sealed record QueryContext
    {
        public string TableName { get; set; } = string.Empty;
        public List<string> SelectedColumns { get; set; } = new();
        public List<(string Column, object Value)> Conditions { get; set; } = new(); 
    }
}