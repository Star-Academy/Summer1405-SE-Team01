using System;

namespace SqlBuilder.UsernamePass.Abstractions
{
    public interface IUsernamePass
    {
        string GetUserInfo();
		string GetPassInfo();
    }
}