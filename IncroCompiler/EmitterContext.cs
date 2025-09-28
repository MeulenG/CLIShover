using System.Text;

namespace IncroCompiler
{

    public class EmitterContext
    {
        private StringBuilder _text = new();
        private StringBuilder _data = new();
        public StringBuilder DataSection { get; } = new StringBuilder();
        public int StringCounter { get; set; } = 0;
        
        public Dictionary<int, string> Labels { get; } = new();
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

        public void WriteText(string line)
        {
            _text.AppendLine("    " + line);
        }

        public void WriteData(string line)
        {
            _data.AppendLine(line);
        }

        public string GetNewLabel() => $"label_{LabelCounter++}";

        public void AddLabel(string methodName) => _text.AppendLine($"{methodName}:");

        public string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("global main");
            sb.AppendLine("section .data");
            sb.Append(_data.ToString());
            sb.AppendLine("section .text");
            sb.Append(_text.ToString());
            return sb.ToString();
        }
    }
}
