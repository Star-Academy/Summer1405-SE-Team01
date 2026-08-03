using System;
using SqlBuilderLibrary;

class Program
{
    static void Main()
    {
        var query = new Query()
            .From("Student2")
            .Select("StudentNumber", "FirstName", "LastName")
            .Where("IsMale", true)
            .Where("Grade", 12);

        var pg = new PostgresCompiler();
        var pgResult = pg.Compiler.Compile(query, pg.parameters);
        var pgConn = "Host=localhost;Username=postgres;Password=postgres;Database=mohaymen";
        
        DatabaseHelper.ExecuteDatabase(
            "PostgreSQL", 
            pgResult, 
            () => NPG_sql.ExecuteOnPostgres(pgResult, pgConn)
        );

        var sql = new SqlServerCompiler();
        var sqlResult = sql.Compiler.Compile(query, sql.parameters);
        var sqlConn = "Server=localhost;Database=mohaymen;User Id=sa;Password=Your_strong_Password123;TrustServerCertificate=True;";
        
        DatabaseHelper.ExecuteDatabase(
            "SQL Server", 
            sqlResult, 
            () => SQLServer_sql.ExecuteOnSqlServer(sqlResult, sqlConn)
        );
    }
}