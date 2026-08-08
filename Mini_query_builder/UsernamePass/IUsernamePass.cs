using System;

namespace SqlBuilder
{
    public interface IUsernamePass
    {
        string GetUserInfo();
		string GetPassInfo();
    }
}