using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;

namespace SqlBuilder
{
    public class CompileResult
    {
        public string RawQuery { get; set; } = string.Empty;
        public List<object> Bindings { get; set; } = new();
    }
}