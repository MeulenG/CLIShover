using System.Text;


namespace CLIShover
{

    public class EmitterContext
    {
         public StringBuilder Output { get; } = new StringBuilder();
    
        public Dictionary<int, string> Labels { get; } = new(); // IL offset → label
        public Stack<string> EvaluationStack { get; } = new();   // Simulated eval stack
        public int LabelCounter { get; set; } = 0;
        
        public void WriteLine(string line)
        {
            Output.AppendLine("    " + line);
        }

        public string GetNewLabel()
        {
            return $"label_{LabelCounter++}";
        }

        public void AddLabel(string methodName)
        {
            Output.AppendLine($"{methodName}:");
        }

        // Call needs method name
        public string returnMethodName(string methodName)
        {
            return methodName;
        }

        public override string ToString() => Output.ToString();
    }

}
