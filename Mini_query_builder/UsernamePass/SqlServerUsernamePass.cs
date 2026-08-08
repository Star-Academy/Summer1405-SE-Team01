using System;

namespace SqlBuilder
{
    internal sealed class SqlServerUsernamePass : IUsernamePass
    {
        public string UserInfo { get; } = Environment.GetEnvironmentVariable("SQL_USERNAME") ?? "sa";
        public string PassInfo { get; } = Environment.GetEnvironmentVariable("SQL_PASSWORD") ?? "Your_strong_Password123";
    }
}