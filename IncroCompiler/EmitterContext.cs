using System.Text;

namespace IncroCompiler
{

    public class EmitterContext
    {
        private readonly List<string> _dataLines = new List<string>();
        private readonly List<string> _textLines = new List<string>();
        private readonly Dictionary<(string Method, int Offset), string> _labels
            = new Dictionary<(string, int), string>();
        public int StringCounter { get; set; } = 0;
        
        public Dictionary<string, string> Labels = new Dictionary<string,string>();
        public Stack<string> EvaluationStack { get; } = new();
        public int LabelCounter { get; set; } = 0;
        public string CurrentMethodName = string.Empty;
        private readonly Dictionary<string, string> _stringLiterals = new();
        private int _strId = 0;

        public string InternString(string s)
        {
            if (_stringLiterals.TryGetValue(s, out var lbl)) return lbl;
            lbl = $"__str_{_strId++}";
            var escaped = s.Replace("\"", "\"\"");
            WriteData($"{lbl}: db \"{escaped}\", 0");  // NUL-terminated
            _stringLiterals[s] = lbl;
            return lbl;
        }

        private readonly Dictionary<System.Reflection.FieldInfo, string> _fieldLabels = new();
        public string InternField(System.Reflection.FieldInfo f)
        {
            if (_fieldLabels.TryGetValue(f, out var lbl)) return lbl;
            lbl = $"__fld_{f.DeclaringType!.Name}_{f.Name}";
            WriteData($"{lbl}: dq 0");
            _fieldLabels[f] = lbl;
            return lbl;
        }

        public string GetNewLabel() => $"label_{LabelCounter++}";

        public string MakeLabelKey(string methodName, int offset) => $"{methodName}:{offset:X4}";

        public string GetOrCreateLabel(string methodName, int offset)
        {
            var key = MakeLabelKey(methodName, offset);
            if (!Labels.TryGetValue(key, out var label))
            {
                label = $"{methodName}_L_{offset:X4}";
                Labels[key] = label;
            }
            return label;
        }

        public bool TryGetLabel(string methodName, int offset, out string label)
        {
            return Labels.TryGetValue(MakeLabelKey(methodName, offset), out label);
        }

        public void AddLabel(string methodName, int offset, string labelName)
        {
            _labels[(methodName, offset)] = labelName;
        }

        public void WriteData(string line) => _dataLines.Add(line);
        public void WriteText(string line) => _textLines.Add(line);

        public string DataSection => string.Join(Environment.NewLine, _dataLines) + Environment.NewLine;
        public override string ToString() => string.Join(Environment.NewLine, _textLines);

        private static string Sanitize(string s)
        {
            if (string.IsNullOrEmpty(s)) return "M";
            // allow alphanumerics and underscores
            var arr = s.Select(c => (char.IsLetterOrDigit(c) || c == '_' ? c : '_')).ToArray();
            return new string(arr);
        }
    }
}
