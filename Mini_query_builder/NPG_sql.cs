using Npgsql;
using System;

namespace SqlBuilderLibrary
{
    public class NPG_sql
    {
        public static void ExecuteOnPostgres(CompileResult result, string connectionString)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand(result.Sql, connection);

            foreach (var binding in result.Bindings)
            {
                command.Parameters.Add(new NpgsqlParameter { Value = binding });
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