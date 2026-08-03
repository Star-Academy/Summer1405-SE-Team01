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

    public interface IQueryGenerator
    {
        CompileResult Compile(Query query, IQueryTranslator parameters);
    }

    public interface IQueryTranslator
    {
        string selectClause(Query query);
        string fromClause(Query query);

        CompileResult whereClause(Query query);
    }
}