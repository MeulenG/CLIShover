using System.Text;

namespace CLIShover
{

    public class EmitterContext
    {
        private StringBuilder _text = new();
        private StringBuilder _data = new();
        
        public Dictionary<int, string> Labels { get; } = new();
        public Stack<string> EvaluationStack { get; } = new();
        public int LabelCounter { get; set; } = 0;
        public string CurrentMethodName = string.Empty;

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
