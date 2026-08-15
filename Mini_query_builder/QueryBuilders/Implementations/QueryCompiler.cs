using System.Collections.Generic;
using System.Linq;
using SqlBuilder.QueryBuilders.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using System;


namespace SqlBuilder.QueryBuilders.Implementations
{
    internal sealed class QueryCompiler : IQueryCompiler
    {
        readonly IQueryDecomposer parameters;
        public QueryCompiler(IQueryDecomposer parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);
            this.parameters = parameters;
        }
        public CompileResult Compile(Query query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var result = new CompileResult();
            var selectPart = parameters.selectClause(query);
            var fromPart = parameters.fromClause(query);
            var wherePart = parameters.whereClause(query);

            var sql_query_string = $"{selectPart} {fromPart} {wherePart?.RawQuery}".TrimEnd();

            result.Bindings = wherePart?.Bindings ?? [];
            result.RawQuery = sql_query_string;

            return result;
        }
    }
}