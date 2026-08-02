using System;
using SqlBuilderLibrary;

class Program
{
    static void Main()
    {
        var query = new Query()
            .From("Student2")
            .Select("StudentNumber", "FirstName", "LastName")
            .Where("Grade", 12);

        
        QueryGenerator pgCompiler = new PostgresCompiler();
        var pgResult = pgCompiler.Compile(query);

        Console.WriteLine("PostgreSQL\n");
        Console.WriteLine($"SQL: {pgResult.Sql}");
        Console.WriteLine($"Bindings: [{string.Join(", ", pgResult.Bindings)}]");
        
        string pgConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mohaymen";
        
        try 
        {
            Console.WriteLine("\nConnecting and Executing on PostgreSQL...\n");
            NPG_sql.ExecuteOnPostgres(pgResult, pgConnectionString);
            Console.WriteLine("\nPostgreSQL Execution Successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] PostgreSQL Error: {ex.Message}");
        }

        Console.WriteLine("\n\n");

        
        QueryGenerator sqlServerCompiler = new SqlServerCompiler();
        var sqlResult = sqlServerCompiler.Compile(query);

        Console.WriteLine("SQL Server");
        Console.WriteLine($"SQL: {sqlResult.Sql}");
        Console.WriteLine($"Bindings: [{string.Join(", ", sqlResult.Bindings)}]");

        string sqlConnectionString = "Server=localhost;Database=mohaymen;User Id=sa;Password=Your_strong_Password123;TrustServerCertificate=True;";
        
        try 
        {
            Console.WriteLine("\nConnecting and Executing on SQL Server...\n");
            SQLServer_sql.ExecuteOnSqlServer(sqlResult, sqlConnectionString);
            Console.WriteLine("\nSQL Server Execution Successful!");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] SQL Server Error: {ex.Message}");
        }
    }
}