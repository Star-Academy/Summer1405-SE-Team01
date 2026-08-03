using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;

namespace SqlBuilderLibrary
{
    public class CompileResult
    {
        public string Sql { get; set; } = string.Empty;
        public List<object> Bindings { get; set; } = new();
    }

    public interface IQueryGenerator
    {
        CompileResult Compile(Query query, IQueryParts parameters);
    }

    public interface IQueryParts
    {
        string selectClause(Query query);
        string fromClause(Query query);

        CompileResult whereClause(Query query);
    }
}