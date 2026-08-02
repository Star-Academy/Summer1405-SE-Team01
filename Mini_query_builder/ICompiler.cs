using System.Collections.Generic;

namespace SqlBuilderLibrary
{
    public class CompileResult
    {
        public string Sql { get; set; } = string.Empty;
        public List<object> Bindings { get; set; } = new();
    }

    public interface QueryGenerator
    {
        CompileResult Compile(Query query);
    }
}