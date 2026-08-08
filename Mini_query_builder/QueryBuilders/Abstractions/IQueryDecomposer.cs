using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;
using SqlBuilder.Querying;
using SqlBuilder.ResultRecords;

namespace SqlBuilder.QueryBuilders.Abstractions
{
    public interface IQueryDecomposer
{
    string selectClause(Query query);
    string fromClause(Query query);
    CompileResult whereClause(Query query);
    }
}