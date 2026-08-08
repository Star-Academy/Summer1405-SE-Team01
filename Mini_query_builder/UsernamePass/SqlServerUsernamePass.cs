using System;

namespace SqlBuilder
{
    internal sealed class SqlServerUsernamePass : IUsernamePass
    {
        public string GetUserInfo() { return Environment.GetEnvironmentVariable("SQL_USERNAME") ?? "sa"; }
        public string GetPassInfo() { return Environment.GetEnvironmentVariable("SQL_PASSWORD") ?? "Your_strong_Password123"; }
    }
}