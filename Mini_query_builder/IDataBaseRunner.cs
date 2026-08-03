using System;
using System.Data.Common;
namespace SqlBuilder
{
    public interface IDatabaseRunner
    {
        void ExecuteDatabase(string dbName, dynamic result, Action executeAction);
    }
}