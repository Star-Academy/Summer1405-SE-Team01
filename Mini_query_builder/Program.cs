using System;
using SqlBuilder;

namespace SqlBuilder
{
    class Program
    {
        static void Main()
        {
            IDatabaseRunner dbRunner = new DatabaseRunner();

            var query = new Query()
                .From("Student2")
                .Select("StudentNumber", "FirstName", "LastName")
                .Where("IsMale", true)
                .Where("Grade", 12);

            string postgreUser = Environment.GetEnvironmentVariable("PG_USERNAME") ?? "postgres";
            string postgrePass = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? "postgres";

            var postgresCompiler = new PostgresCompiler();
            var PostgresQueryResult = postgresCompiler.Compiler.Compile(query, postgresCompiler.parameters);
            var PostgresConnection = $"Host=localhost;Username={postgreUser};Password={postgrePass};Database=mohaymen";
            var NpgSql = new NpgsqlExecutor();
            dbRunner.ExecuteDatabase(
                "PostgreSQL",
                PostgresQueryResult,
                () => NpgSql.ExecuteOnPostgres(PostgresQueryResult, PostgresConnection)
            );

            string mssqlUser = Environment.GetEnvironmentVariable("SQL_USERNAME") ?? "sa";
            string mssqlPass = Environment.GetEnvironmentVariable("SQL_PASSWORD") ?? "Your_strong_Password123";

            var sqlServerCompiler = new SqlServerCompiler();
            var SqlServerQueryResult = sqlServerCompiler.Compiler.Compile(query, sqlServerCompiler.parameters);
            var SqlServerConnection = $"Server=localhost;Database=mohaymen;User Id={mssqlUser};Password={mssqlPass};TrustServerCertificate=True;";
            var SqlServer = new SQLServerExecutor();
            dbRunner.ExecuteDatabase(
                "SQL Server",
                SqlServerQueryResult,
                () => SqlServer.ExecuteOnSqlServer(SqlServerQueryResult, SqlServerConnection)
            );
        }
    }
}