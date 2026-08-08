using System;

namespace SqlBuilder
{
    internal sealed class PostgresUsernamePass : IUsernamePass
    {
        public string GetUserInfo() { return Environment.GetEnvironmentVariable("PG_USERNAME") ?? "postgres"; }
        public string GetPassInfo() { return Environment.GetEnvironmentVariable("PG_PASSWORD") ?? "postgres"; }

    }
}