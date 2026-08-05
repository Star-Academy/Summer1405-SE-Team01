using System.Collections.Generic;
using System.Linq;

namespace SqlBuilder
{
    public class PostgresCompiler
    {
        public IQueryTranslator parameters = new PostgreGenerator();
        public IQueryGenerator Compiler = new QueryCompiler();
    }
}