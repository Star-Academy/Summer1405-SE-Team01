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
        IQueryDecomposer parameters;
        public QueryCompiler(IQueryDecomposer parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);
            this.parameters = parameters;
        }
        public CompileResult Compile(Query query)
        {
            ArgumentNullException.ThrowIfNull(query);
            var result = new CompileResult();
            var sql_query_string = $"{parameters.selectClause(query)} {parameters.fromClause(query)} {parameters.whereClause(query).RawQuery}";
            result.Bindings = parameters.whereClause(query).Bindings;
            result.RawQuery = sql_query_string;
            return result;
        }
    }
}