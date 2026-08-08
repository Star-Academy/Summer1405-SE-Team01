using System;

namespace SqlBuilder
{
    internal sealed class PostgresUsernamePass : IUsernamePass
    {
        public string UserInfo { get; } = Environment.GetEnvironmentVariable("PG_USERNAME") ?? "postgres";
        public string PassInfo { get; } = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? "postgres";
    }
}