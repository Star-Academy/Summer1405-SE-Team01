using System.Collections.Generic;
using System.Linq;

namespace SqlBuilderLibrary
{
    public class PostgresCompiler
    {
        public IQueryParts parameters = new PostgreGenerator();
        public IQueryGenerator Compiler = new RDBMSgenerator();

    }
}