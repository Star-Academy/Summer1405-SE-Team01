using System.Collections.Generic;
using System.Linq;

namespace SqlBuilderLibrary
{
    public class SqlServerCompiler
    {
        public IQueryParts parameters = new SqlServerGenerator();
        public IQueryGenerator Compiler = new RDBMSgenerator();
    }
}