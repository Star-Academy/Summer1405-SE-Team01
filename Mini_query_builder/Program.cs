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

            var postgresQueryResult = (new QueryCompiler(new PostgreQueryDecomposer())).Compile(query);
            var postgresConnection = $"Host=localhost;Username={new PostgresUsernamePass().GetUserInfo()};Password={new PostgresUsernamePass().GetPassInfo()};Database=mohaymen";

            var NpgSql = new NpgsqlExecutor(new NpgSqlExecutorConnection(), new NpgSqlExecutorPrintResult(), new NpgSqlExecutorAddParameter(), new NpgSqlCommandExecutor());

            Console.WriteLine($"{"PostgreSQL"}\nSQL:{postgresQueryResult.RawQuery}\nBindings: [{string.Join(", ", postgresQueryResult.Bindings)}]");
            try
            {
                Console.WriteLine(NpgSql.ExecuteOnPostgres(postgresQueryResult, postgresConnection));
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
            var sqlServerConnection = $"Server=localhost;Database=mohaymen;User Id={new SqlServerUsernamePass().GetUserInfo()};Password={new SqlServerUsernamePass().GetPassInfo()};TrustServerCertificate=True;";

            var sqlServer = new SQLServerExecutor(new SQLServerExecutorConnection(), new SQLServerExecutorPrintResult(), new SQLServerExecutorAddParameters(), new SqlServerCommandExecutor());

            Console.WriteLine($"{"SQL Server"}\nSQL:{sqlServerQueryResult.RawQuery}\nBindings: [{string.Join(", ", sqlServerQueryResult.Bindings)}]");

            try
            {
                Console.WriteLine(sqlServer.ExecuteOnSqlServer(sqlServerQueryResult, sqlServerConnection));
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
    }
}