using System;

namespace SqlBuilder
{
    public interface IUsernamePass
    {
        string PostgresUser { get; }
        string PostgresPass { get; }
        string MssqlUser { get; }
        string MssqlPass { get; }
    }
}