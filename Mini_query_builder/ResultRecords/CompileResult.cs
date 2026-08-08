using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;

namespace SqlBuilder.ResultRecords
{
    public sealed record CompileResult
    {
        public string RawQuery { get; set; } = string.Empty;
        public List<object> Bindings { get; set; } = new();
    }
}