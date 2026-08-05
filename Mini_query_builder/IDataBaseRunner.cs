using System;
using System.Data.Common;
namespace SqlBuilder
{
    public interface IDatabaseRunner
    {
        void Execute(string dbName, dynamic result, Action executeAction);
    }
}