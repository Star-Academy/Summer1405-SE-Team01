using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;

namespace SqlBuilder
{
    public interface IQueryTranslator
    {
        string selectClause(Query query);
        string fromClause(Query query);
        CompileResult whereClause(Query query);
    }
}