using System;
using SqlBuilder;
using SqlBuilder.ResultRecords;
using System.Data.Common;
using SqlBuilder.Executors.Implementations;
using SqlBuilder.Executors.Abstractions;
using SqlBuilder.Querying;
using SqlBuilder.QueryBuilders.Implementations;
using SqlBuilder.QueryBuilders.Abstractions;



namespace SqlBuilder
{
    internal static class Program
    {
        static void Main()
        {

            var localizer = new ErrorLocalizer();
            localizer.LoadLanguage("en");


            var query = new Query()
                .From("Student2")
                .Select("StudentNumber", "FirstName", "LastName")
                .Where("IsMale", true)
                .Where("Grade", 12);

            IUsernamePass pgUsernamePass = new PostgresUsernamePass();
            IUsernamePass sqlServerUsernamePass = new SqlServerUsernamePass();

            var postgresQueryResult =  (new QueryCompiler(new PostgreQueryDecomposer())).Compile(query);
            var postgresConnection = $"Host=localhost;Username={pgUsernamePass.UserInfo};Password={pgUsernamePass.PassInfo};Database=mohaymen";
            var NpgSql = new NpgsqlExecutor();

            Console.WriteLine($"{"PostgreSQL"}\nSQL: {postgresQueryResult.RawQuery}\nBindings: [{string.Join(", ", postgresQueryResult.Bindings)}]");
            Console.WriteLine($"{"PostgreSQL"}");
            try
            {
                Console.WriteLine(NpgSql.ExecuteOnPostgres(postgresQueryResult, postgresConnection));
                Console.WriteLine($" Execution Successful!\n");
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
            var sqlServerConnection = $"Server=localhost;Database=mohaymen;User Id={sqlServerUsernamePass.UserInfo};Password={sqlServerUsernamePass.PassInfo};TrustServerCertificate=True;";
            var sqlServer = new SQLServerExecutor();

            Console.WriteLine($"{"SQL Server"}");
            try
            {
                sqlServer.ExecuteOnSqlServer(sqlServerQueryResult, sqlServerConnection);
                Console.WriteLine($" Execution Successful!\n");
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