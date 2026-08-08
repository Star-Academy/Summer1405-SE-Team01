using System;

namespace SqlBuilder
{
    public interface IUsernamePass
    {
        string UserInfo { get; }
        string PassInfo { get; }
    }
}