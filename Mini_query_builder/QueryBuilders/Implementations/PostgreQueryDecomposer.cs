using System.Collections.Generic;
using System.Linq;
using SqlBuilder.QueryBuilders.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.QueryBuilders.Implementations
{
    internal sealed class PostgreQueryDecomposer : IQueryDecomposer {
        public string selectClause(Query query)
        {
            string columnsString = query.Context.SelectedColumns.Count > 0
                ? string.Join(", ", query.Context.SelectedColumns.Select(c => $"\"{c}\""))
                : "*";
            return $"SELECT {columnsString}";
        }

        public string fromClause(Query query)
        {
            return $"FROM \"{query.Context.TableName}\"";
        }

        public CompileResult whereClause(Query query)
        {
            var result = new CompileResult();
            if (query.Context.Conditions.Count > 0)
            {
                var whereClauses = new List<string>();
                var paramIndex = 1;
                
                foreach (var condition in query.Context.Conditions)
                {
                    whereClauses.Add($"\"{condition.Column}\" = ${paramIndex}");
                    result.Bindings.Add(condition.Value);
                    paramIndex++;
                }
                result.RawQuery = " WHERE " + string.Join(" AND ", whereClauses);
            }
            return result;
        }
    }
}