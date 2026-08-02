using Microsoft.Data.SqlClient;
using System;

namespace SqlBuilderLibrary
{
    public class SQLServer_sql
    {
        public static void ExecuteOnSqlServer(CompileResult result, string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand(result.Sql, connection);

            for (int i = 0; i < result.Bindings.Count; i++)
            {
                command.Parameters.AddWithValue($"@p{i}", result.Bindings[i]);
            }

            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                var rowData = new List<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);

                    object value = reader.GetValue(i);

                    rowData.Add($"{columnName}: {value}");
                }

                Console.WriteLine(string.Join(" | ", rowData));
            }
        }
    }
}