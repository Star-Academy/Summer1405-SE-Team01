using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;

namespace SqlBuilder
{
    public interface IQueryGenerator
    {
        CompileResult Compile(Query query, IQueryTranslator parameters);
    }
}