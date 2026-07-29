using System;
using System.Collections.Generic;
using System.Linq;

namespace BrowserHistory
{
    class Program
    {
        private const int DefaultTopStatsCount = 3;

        static void Main(string[] args)
        {
            var BackSearch = new Stack<string>();
            var ForwardSearch = new Stack<string>();
            var searchStats = new Dictionary<string, int>();

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(input)) continue;

                string[] commandArgs = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string command = commandArgs[0].ToUpper();
                string argument = commandArgs.Length > 1 ? commandArgs[1].Trim() : string.Empty;

                switch (command)
                {
                    case "SEARCH":
                        HandleSearch(argument, BackSearch, ForwardSearch, searchStats);
                        break;

                    case "BACK":
                        HandleBack(BackSearch, ForwardSearch);
                        break;

                    case "FORWARD":
                        HandleForward(BackSearch, ForwardSearch);
                        break;

                    case "CURRENT":
                        PrintCurrent(BackSearch);
                        break;

                    case "STATS":
                        HandleStats(searchStats, DefaultTopStatsCount);
                        break;

                    case "UNIQUE":
                        HandleUnique(searchStats);
                        break;

                    case "EXIT":
                        return;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        static void HandleSearch(string term, Stack<string> BackSearch, Stack<string> ForwardSearch, Dictionary<string, int> searchStats)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                Console.WriteLine("Please provide a search term.");
                return;
            }

            BackSearch.Push(term);
            ForwardSearch.Clear();

            searchStats[term] = searchStats.GetValueOrDefault(term, 0) + 1;

            PrintCurrent(BackSearch);
        }

        static void HandleBack(Stack<string> BackSearch, Stack<string> ForwardSearch)
        {
            if (BackSearch.Count > 1)
            {
                ForwardSearch.Push(BackSearch.Pop());
                PrintCurrent(BackSearch);
            }
            else if (BackSearch.Count == 1)
            {
                Console.WriteLine("You can't use back!");
            }
            else
            {
                Console.WriteLine("History is empty.");
            }
        }

        static void HandleForward(Stack<string> BackSearch, Stack<string> ForwardSearch)
        {
            if (ForwardSearch.Count > 0)
            {
                BackSearch.Push(ForwardSearch.Pop());
                PrintCurrent(BackSearch);
            }
            else
            {
                Console.WriteLine("Cannot go forward.");
            }
        }

        static void HandleStats(Dictionary<string, int> searchStats, int topCount = DefaultTopStatsCount)
        {
            if (searchStats.Count == 0)
            {
                Console.WriteLine("No search history available.");
                return;
            }

            var topSearches = searchStats
                .OrderByDescending(x => x.Value)
                .Take(topCount);

            foreach (var stat in topSearches)
            {
                Console.WriteLine($"{stat.Key}: {stat.Value}");
            }
        }

        static void HandleUnique(Dictionary<string, int> searchStats)
        {
            Console.WriteLine(searchStats.Count);
        }

        static void PrintCurrent(Stack<string> stack)
        {
            if (stack.Count > 0)
            {
                Console.WriteLine($"current: {stack.Peek()}");
            }
            else
            {
                Console.WriteLine("current is empty");
            }
        }
    }
}