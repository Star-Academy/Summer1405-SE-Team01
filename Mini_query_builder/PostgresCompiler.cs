using System.Collections.Generic;
using System.Linq;

namespace SqlBuilderLibrary
{
    public class PostgresCompiler : QueryGenerator
    {
        public CompileResult Compile(Query query)
        {
            var result = new CompileResult();
        
            string columnsStr = query.SelectedColumns.Count > 0 
                ? string.Join(", ", query.SelectedColumns.Select(c => $"\"{c}\""))
                : "*";
            
            string sql_query_string = $"SELECT {columnsStr} FROM \"{query.TableName}\"";

            if (query.Conditions.Count > 0)
            {
                var whereClauses = new List<string>();
                int paramIndex = 1;

                foreach (var condition in query.Conditions)
                {
                    whereClauses.Add($"\"{condition.Column}\" = ${paramIndex}");
                    result.Bindings.Add(condition.Value);
                    paramIndex++;
                }

                sql_query_string += " WHERE " + string.Join(" AND ", whereClauses);
            }

            result.Sql = sql_query_string;
            return result;
        }
    }
}