using System;

namespace SqlBuilder
{
    public class UsernamePass : IUsernamePass
    {
        public string PostgresUser { get; } = Environment.GetEnvironmentVariable("PG_USERNAME") ?? "postgres";
        public string PostgresPass { get; } = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? "postgres";

        public string MssqlUser { get; } = Environment.GetEnvironmentVariable("SQL_USERNAME") ?? "sa";
        public string MssqlPass { get; } = Environment.GetEnvironmentVariable("SQL_PASSWORD") ?? "Your_strong_Password123";
    }
}