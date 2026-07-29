using System;
using System.Linq;
using System.Resources;
using System.Reflection;

namespace BrowserHistory
{
    class Program
    {
        private const int DefaultTopStatsCount = 3;
        private static ResourceManager rm = new ResourceManager("mohaymen.Messages", Assembly.GetExecutingAssembly());

        static void Main(string[] args)
        {
            var history = new HistoryManager();

            while (true)
            {
                Console.Write("> "); 
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(input)) continue;

                var commandArgs = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                var command = commandArgs[0].ToUpper();
                var argument = commandArgs.Length > 1 ? commandArgs[1].Trim() : string.Empty;

                switch (command)
                {
                    case AppCommands.Search:
                        if (string.IsNullOrWhiteSpace(argument))
                        {
                            Console.WriteLine(rm.GetString("ProvideSearchTerm"));
                        }
                        else
                        {
                            history.Search(argument);
                            PrintCurrent(history);
                        }
                        break;

                    case AppCommands.Back:
                        if (history.GoBack())
                        {
                            PrintCurrent(history);
                        }
                        else if (history.IsAtRoot)
                        {
                            Console.WriteLine(rm.GetString("CantUseBack"));
                        }
                        else
                        {
                            Console.WriteLine(rm.GetString("HistoryEmpty"));
                        }
                        break;

                    case AppCommands.Forward:
                        if (history.GoForward())
                        {
                            PrintCurrent(history);
                        }
                        else
                        {
                            Console.WriteLine(rm.GetString("CannotGoForward"));
                        }
                        break;

                    case AppCommands.Current:
                        PrintCurrent(history);
                        break;

                    case AppCommands.Stats:
                        var stats = history.GetTopStats(DefaultTopStatsCount);
                        if (!stats.Any())
                        {
                            Console.WriteLine(rm.GetString("NoHistory"));
                        }
                        else
                        {
                            foreach (var stat in stats)
                            {
                                Console.WriteLine($"{stat.Key}: {stat.Value}");
                            }
                        }
                        break;

                    case AppCommands.Unique:
                        Console.WriteLine(history.GetUniqueCount());
                        break;

                    case AppCommands.Exit:
                        return;

                    default:
                        Console.WriteLine(rm.GetString("UnknownCommand"));
                        break;
                }
            }
        }

        static void PrintCurrent(HistoryManager history)
        {
            var current = history.GetCurrent();
            if (current != null)
            {
                var formatString = rm.GetString("CurrentFormat") ?? "current: {0}";
                Console.WriteLine(string.Format(formatString, current));
            }
            else
            {
                Console.WriteLine(rm.GetString("CurrentEmpty"));
            }
        }
    }
}