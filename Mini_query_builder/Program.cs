using System;
using SqlBuilder;

namespace SqlBuilder
{
    static class Program
    {
        static void Main()
        {

            var localizer = new ErrorLocalizer();
            localizer.LoadLanguage("en");

            string errorKey = "DatabaseTimeout";
            Console.WriteLine(localizer.Get(errorKey));

            IDatabaseRunner dbRunner = new DatabaseRunner();

            var query = new Query()
                .From("Student2")
                .Select("StudentNumber", "FirstName", "LastName")
                .Where("IsMale", true)
                .Where("Grade", 12);

            IUsernamePass usernamePass = new UsernamePass();

            var postgresCompiler = new PostgresCompiler();
            var postgresQueryResult = postgresCompiler.Compiler.Compile(query, postgresCompiler.parameters);
            var postgresConnection = $"Host=localhost;Username={usernamePass.PostgresUser};Password={usernamePass.PostgresPass};Database=mohaymen";
            var NpgSql = new NpgsqlExecutor();

            dbRunner.Execute(
                "PostgreSQL",
                postgresQueryResult,
                () => NpgSql.ExecuteOnPostgres(postgresQueryResult, postgresConnection)
            );

            var sqlServerCompiler = new SqlServerCompiler();
            var sqlServerQueryResult = sqlServerCompiler.Compiler.Compile(query, sqlServerCompiler.parameters);
            var sqlServerConnection = $"Server=localhost;Database=mohaymen;User Id={usernamePass.MssqlUser};Password={usernamePass.MssqlPass};TrustServerCertificate=True;";
            var sqlServer = new SQLServerExecutor();

            dbRunner.Execute(
                "SQL Server",
                sqlServerQueryResult,
                () => sqlServer.ExecuteOnSqlServer(sqlServerQueryResult, sqlServerConnection)
            );
        }
    }
}