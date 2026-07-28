using System;
using System.Collections.Generic;
using System.Linq;

namespace BrowserHistory
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<string> backStack = new Stack<string>();
            
            Stack<string> forwardStack = new Stack<string>();
            
            Dictionary<string, int> searchStats = new Dictionary<string, int>();

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(input)) continue;

                string[] parts = input.Split(' ', 2);
                string command = parts[0].ToUpper();

                switch (command)
                {
                    case "SEARCH":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Please provide a search term.");
                            break;
                        }
                        string term = parts[1];
                        
                        backStack.Push(term);
                        forwardStack.Clear();

                        if (searchStats.ContainsKey(term))
                        {
                            searchStats[term]++;
                        }
                        else
                        {
                            searchStats[term] = 1;
                        }
                        
                        PrintCurrent(backStack);
                        break;

                    case "BACK":
                        if (backStack.Count >= 1) 
                        {
                            forwardStack.Push(backStack.Pop());
                            PrintCurrent(backStack);
                        }
                        
                        else
                        {
                            Console.WriteLine("History is empty.");
                        }
                        break;

                    case "FORWARD":
                        if (forwardStack.Count > 0)
                        {
                            backStack.Push(forwardStack.Pop());
                            PrintCurrent(backStack);
                        }
                        else
                        {
                            Console.WriteLine("Cannot go forward.");
                        }
                        break;

                    case "CURRENT":
                        PrintCurrent(backStack);
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