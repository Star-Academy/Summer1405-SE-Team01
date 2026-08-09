using System.Collections.Generic;
using System.Linq;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;
using SqlBuilder.QueryBuilders.Abstractions;


namespace SqlBuilder.QueryBuilders.Implementations
{
    internal sealed class SqlServerQueryDecomposer : IQueryDecomposer
    {
        public string selectClause(Query query)
        {
            var columnsString = query.Context.SelectedColumns.Count > 0
                ? string.Join(", ", query.Context.SelectedColumns.Select(coloumn => $"[{coloumn}]"))
                : "*";
            return $"SELECT {columnsString}";
        }

        public string fromClause(Query query)
        {
            if (string.IsNullOrWhiteSpace(query.Context.TableName))
            {
                throw new ArgumentException("Table name cannot be null or empty.", nameof(query));
            }
            return $"FROM [{query.Context.TableName}]";
        }

        public CompileResult whereClause(Query query)
        {
            var result = new CompileResult();
            if (query.Context.Conditions.Count > 0)
            {
                var whereClauses = new List<string>();
                int paramIndex = 1;

                foreach (var condition in query.Context.Conditions)
                {
                    whereClauses.Add($"[{condition.Column}] = @p{paramIndex}");

                    var value = condition.Value;
                    if (value is bool boolValue)
                    {
                        value = boolValue ? 1 : 0;
                    }

                    result.Bindings.Add(value);
                    paramIndex++;
                }
                result.RawQuery = " WHERE " + string.Join(" AND ", whereClauses);
            }
            return result;
        }
    }
}