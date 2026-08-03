using Microsoft.Data.SqlClient;
using System;

namespace SqlBuilder
{
    public class SQLServerExecutor : ISQLServerExecutor
    {
        public void ExecuteOnSqlServer(CompileResult result, string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            OpeningConnection(connection);

            using var command = new SqlCommand(result.RawQuery, connection);
            AddParameters(command, result);

            using var reader = command.ExecuteReader();

            PrintQueryResult(reader);
        }
        public void OpeningConnection(SqlConnection connection)
        {
            connection.Open();
        }
        public void AddParameters(SqlCommand command, CompileResult result)
        {
            for (int i = 0; i < result.Bindings.Count; i++)
            {
                command.Parameters.AddWithValue($"@p{i + 1}", result.Bindings[i]);
            }
        }
        public void PrintQueryResult(SqlDataReader reader)
        {
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