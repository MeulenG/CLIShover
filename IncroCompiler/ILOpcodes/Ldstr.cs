using IncroCompiler;
using System.Text;

namespace IncroCompiler.ILOpCodes
{
    public class Ldstr_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            string label = $"str_{ctx.StringCounter++}";
            string value = instr.StringValue ?? string.Empty;

            // Escape quotes and backslashes for NASM
            var escaped = new StringBuilder();
            foreach (char c in value)
            {
                if (c == '\"' || c == '\\')
                    escaped.Append('\\').Append(c);
                else if (c == '\n')
                    escaped.Append("\", 10, \"");
                else if (c == '\r')
                    escaped.Append("\", 13, \"");
                else
                    escaped.Append(c);
            }

            // Add string to .data section
            ctx.WriteData($"{label}: db \"{escaped}\", 0");

            // Load address of the string into RAX and push it
            ctx.WriteText($"    lea rax, [{label}]");
            ctx.EvaluationStack.Push("rax");
        }
    }
}
