using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.QueryBuilders.Abstractions
{
    public interface IQueryCompiler
    {

           CompileResult Compile(Query query);
    }
}