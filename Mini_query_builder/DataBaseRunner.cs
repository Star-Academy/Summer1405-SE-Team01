using System.Data.Common;

namespace SqlBuilder
{
    public class DatabaseRunner : IDatabaseRunner
    {
        public void Execute(string dbName, dynamic result, Action executeAction)
        {
            Console.WriteLine($"{dbName}\nSQL: {result.RawQuery}\nBindings: [{string.Join(", ", result.Bindings)}]");

            try
            {
                executeAction();
                Console.WriteLine($"{dbName} Execution Successful!\n");
            }
            catch (DbException ex)
            {
                Console.WriteLine($"[!] {dbName} Database Error: {ex.Message}\n");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"[!] {dbName} Timeout Error: Operation took too long.\n");
            }
        }
    }
}