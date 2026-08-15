using System;
using SqlBuilder;
using SqlBuilder.ResultRecords;
using System.Data.Common;
using SqlBuilder.Executors.Implementations;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.QueryBuilders.Implementations;
using SqlBuilder.QueryBuilders.Abstractions;
using SqlBuilder.UsernamePass.Implementations;
using SqlBuilder.UsernamePass.Abstractions;
using SqlBuilder.Exceptions.Implementations;
using SqlBuilder.Exceptions.Abstractions;

namespace SqlBuilder
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Program
    {
        static void Main()
        {
            var localizer = new ErrorLocalizer(new LanguageFileProvider());
            localizer.LoadLanguage("en");


            var query = new Query()
                .From("Student2")
                .Select("StudentNumber", "FirstName", "LastName")
                .Where("IsMale", true)
                .Where("Grade", 12);
            
            var postgresConnection = $"Host=localhost;Port=5433;Username={new PostgresUsernamePass().GetUserInfo()};Password={new PostgresUsernamePass().GetPassInfo()};Database=mohaymen";
            
            var postgresQueryResult = (new QueryCompiler(new PostgreQueryDecomposer())).Compile(query);
            var NpgSql = new NpgsqlExecutor(new NpgSqlExecutorConnection(), new ExecutorGetResult(), new NpgSqlExecutorAddParameter(), new NpgSqlCommandExecutor());

            Console.WriteLine($"{"PostgreSQL"}\nSQL:{postgresQueryResult.RawQuery}\nBindings: [{string.Join(", ", postgresQueryResult.Bindings)}]");
            try
            {
                var pgRows = NpgSql.ExecuteOnPostgres(postgresQueryResult, postgresConnection);
                PrintFormattedRows(pgRows);
                
                Console.WriteLine($"Execution Successful!\n");
            }
            catch (DbException ex)
            {
                Console.WriteLine($"[!] Database Error: {ex.Message}\n");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"[!] Timeout Error: Operation took too long.\n");
            }

            var sqlServerQueryResult = (new QueryCompiler(new SqlServerQueryDecomposer())).Compile(query);
            var sqlServerConnection = $"Server=localhost,14333;Database=mohaymen;User Id={new SqlServerUsernamePass().GetUserInfo()};Password={new SqlServerUsernamePass().GetPassInfo()};TrustServerCertificate=True;";

            var sqlServer = new SQLServerExecutor(new SQLServerExecutorConnection(), new ExecutorGetResult(), new SQLServerExecutorAddParameters(), new SqlServerCommandExecutor());

            Console.WriteLine($"{"SQL Server"}\nSQL:{sqlServerQueryResult.RawQuery}\nBindings: [{string.Join(", ", sqlServerQueryResult.Bindings)}]");

            try
            {
                var sqlRows = sqlServer.ExecuteOnSqlServer(sqlServerQueryResult, sqlServerConnection);
                PrintFormattedRows(sqlRows);
                
                Console.WriteLine($"Execution Successful!\n");
            }
            catch (DbException ex)
            {
                Console.WriteLine($"[!] Database Error: {ex.Message}\n");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"[!] Timeout Error: Operation took too long.\n");
            }
        }
        private static void PrintFormattedRows(List<Dictionary<string, object>> rows)
        {
            if (rows.Count == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            foreach (var row in rows)
            {
                var rowData = new List<string>();
                foreach (var kvp in row)
                {
                    var displayValue = kvp.Value == null ? string.Empty : kvp.Value.ToString();
                    rowData.Add($"{kvp.Key}: {displayValue}");
                }
                
                Console.WriteLine(string.Join(" | ", rowData));
            }
        }
    }
}