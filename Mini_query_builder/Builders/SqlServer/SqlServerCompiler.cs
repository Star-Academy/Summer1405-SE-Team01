using System.Collections.Generic;
using System.Linq;

namespace SqlBuilder
{
    public class SqlServerCompiler
    {
        public IQueryTranslator parameters = new SqlServerGenerator();
        public IQueryGenerator Compiler = new QueryCompiler();
    }
}