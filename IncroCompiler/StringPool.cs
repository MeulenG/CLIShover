using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncroCompiler
{
    public static class StringPool
    {
        private static readonly Dictionary<string, string> _pool = new(StringComparer.Ordinal);
        private static int _counter = 0;
        private static readonly object _lock = new();

        public static string GetLabel(string s)
        {
            lock (_lock)
            {
                if (_pool.TryGetValue(s, out var lbl)) return lbl;
                lbl = $"__str_{_counter++}";
                _pool[s] = lbl;
                return lbl;
            }
        }

        public static IReadOnlyCollection<KeyValuePair<string, string>> GetAll()
        {
            lock (_lock)
            {
                // Return a snapshot
                return new List<KeyValuePair<string, string>>(_pool);
            }
        }

        public static void Clear()
        {
            lock (_lock)
            {
                _pool.Clear();
                _counter = 0;
            }
        }

        public static string EscapeForAsm(string s)
        {
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
