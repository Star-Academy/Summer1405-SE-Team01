using Npgsql;
using System;

namespace SqlBuilder
{
    public class NpgsqlExecutor : INpgSqlExecutor
    {
        public void ExecuteOnPostgres(CompileResult result, string connectionString)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            OpeningConnection(connection);

            using var command = new NpgsqlCommand(result.RawQuery, connection);

            AddParameters(command, result);

            using var reader = command.ExecuteReader();

            PrintQueryResult(reader);
        }
        public void OpeningConnection(NpgsqlConnection connection)
        {
            connection.Open();
        }
        public void AddParameters(NpgsqlCommand command, CompileResult result)
        {
            foreach (var binding in result.Bindings)
            {
                command.Parameters.Add(new NpgsqlParameter { Value = binding });
            }
        }
        public void PrintQueryResult(NpgsqlDataReader reader)
        {
            while (reader.Read())
            {
                var rowData = new List<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);

                    var value = reader.GetValue(i);

                    rowData.Add($"{columnName}: {value}");
                }

                Console.WriteLine(string.Join(" | ", rowData));
            }
        }
    }
}