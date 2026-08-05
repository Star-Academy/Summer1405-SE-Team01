using System.Collections.Generic;
using System.Linq;

namespace SqlBuilder
{
    public class QueryCompiler : IQueryGenerator
    {
        public CompileResult Compile(Query query, IQueryTranslator parameters)
        {
            var result = new CompileResult();

            var sql_query_string = $"{parameters.selectClause(query)} {parameters.fromClause(query)} {parameters.whereClause(query).RawQuery}";
            result.Bindings = parameters.whereClause(query).Bindings;
            result.RawQuery = sql_query_string;
            return result;
        }
    }
}