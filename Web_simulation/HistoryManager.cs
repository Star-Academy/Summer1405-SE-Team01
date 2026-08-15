using System;
using System.Collections.Generic;
using System.Linq;

namespace BrowserHistory
{
    public class HistoryManager
    {
        private Stack<string> BackSearch = new Stack<string>();
        private Stack<string> ForwardSearch = new Stack<string>();
        private Dictionary<string, int> searchStats = new Dictionary<string, int>();

        public void Search(string term)
        {
            BackSearch.Push(term);
            ForwardSearch.Clear();
            searchStats[term] = searchStats.GetValueOrDefault(term, 0) + 1;
        }

        public bool GoBack()
        {
            if (BackSearch.Count > 1)
            {
                ForwardSearch.Push(BackSearch.Pop());
                return true;
            }
            return false;
        }

        public bool GoForward()
        {
            if (ForwardSearch.Count > 0)
            {
                BackSearch.Push(ForwardSearch.Pop());
                return true;
            }
            return false;
        }

        public string? GetCurrent()
        {
            return BackSearch.Count > 0 ? BackSearch.Peek() : null;
        }

        public bool IsEmpty => BackSearch.Count == 0;
        public bool IsAtRoot => BackSearch.Count == 1;

        public int GetUniqueCount() => searchStats.Count;

        public IEnumerable<KeyValuePair<string, int>> GetTopStats(int topCount)
        {
            return searchStats.OrderByDescending(stat => stat.Value).Take(topCount);
        }
    }
}