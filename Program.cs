using System;
using System.Collections.Generic;
using System.Linq;

namespace BrowserHistory
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<string> BackSearch = new Stack<string>();
            
            Stack<string> ForwardSearch = new Stack<string>();
            
            Dictionary<string, int> searchStats = new Dictionary<string, int>();

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(input)) continue;

                string[] commandArgs = input.Split(' ', 2);
                string command = commandArgs[0].ToUpper();

                switch (command)
                {
                    case "SEARCH":
                        if (commandArgs.Length < 2)
                        {
                            Console.WriteLine("Please provide a search term.");
                            break;
                        }
                        string term = commandArgs[1];
                        
                        BackSearch.Push(term);
                        ForwardSearch.Clear();

                        if (searchStats.ContainsKey(term))
                        {
                            searchStats[term]++;
                        }
                        else
                        {
                            searchStats[term] = 1;
                        }
                        
                        PrintCurrent(BackSearch);
                        break;

                    case "BACK":
                        if (BackSearch.Count > 1) 
                        {
                            ForwardSearch.Push(BackSearch.Pop());
                            PrintCurrent(BackSearch);
                        }
                        else if(BackSearch.Count == 1)
                        {
                            Console.WriteLine("You can't use back!");
                        }
                        else
                        {
                            Console.WriteLine("History is empty.");
                        }
                        break;

                    case "FORWARD":
                        if (ForwardSearch.Count > 0)
                        {
                            BackSearch.Push(ForwardSearch.Pop());
                            PrintCurrent(BackSearch);
                        }
                        else
                        {
                            Console.WriteLine("Cannot go forward.");
                        }
                        break;

                    case "CURRENT":
                        PrintCurrent(BackSearch);
                        break;

                    case "STATS":
                        var topSearches = searchStats
                            .OrderByDescending(x => x.Value)
                            .Take(3);
                            
                        foreach (var stat in topSearches)
                        {
                            Console.WriteLine($"{stat.Key}: {stat.Value}");
                        }
                        break;

                    case "UNIQUE":
                        Console.WriteLine(searchStats.Count);
                        break;

                    case "EXIT":
                        return;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
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
