using System.Collections.Generic;
using System.Linq;

namespace SqlBuilderLibrary
{
    public class SqlServerCompiler : QueryGenerator
    {
        public CompileResult Compile(Query query)
        {
            var result = new CompileResult();

            var columnsStr = query.SelectedColumns.Count > 0
                ? string.Join(", ", query.SelectedColumns.Select(coloumn => $"[{coloumn}]"))
                : "*";

            var sql = $"SELECT {columnsStr} FROM [{query.TableName}]";

            if (query.Conditions.Count > 0)
            {
                var whereClauses = new List<string>();
                var paramIndex = 0;

                foreach (var condition in query.Conditions)
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

                sql += " WHERE " + string.Join(" AND ", whereClauses);
            }

            result.Sql = sql;
            return result;
        }
    }
}