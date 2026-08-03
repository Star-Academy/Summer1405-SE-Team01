using System.Collections.Generic;
using System.Linq;

namespace SqlBuilderLibrary
{
    public class RDBMSgenerator : IQueryGenerator
    {
        public CompileResult Compile(Query query, IQueryParts parameters)
        {
            var result = new CompileResult();

            string sql_query_string = $"{parameters.selectClause(query)} {parameters.fromClause(query)} {parameters.whereClause(query).Sql}";
            result.Bindings = parameters.whereClause(query).Bindings;
            result.Sql = sql_query_string;
            return result;
        }
    }
}