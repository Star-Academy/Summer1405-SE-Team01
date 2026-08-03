public static class DatabaseHelper
{
    public static void ExecuteDatabase(string dbName, dynamic result, Action executeAction)
    {
        Console.WriteLine($"{dbName}\nSQL: {result.Sql}\nBindings: [{string.Join(", ", result.Bindings)}]");
        
        try 
        {
            executeAction();
            Console.WriteLine($"{dbName} Execution Successful!\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] {dbName} Error: {ex.Message}\n");
        }
    }
}