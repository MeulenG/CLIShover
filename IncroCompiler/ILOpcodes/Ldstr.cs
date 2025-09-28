using IncroCompiler;

namespace IncroCompiler.ILOpCodes
{
    public class Ldstr_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            string label = $"str_{ctx.StringCounter++}";
            string value = instr.StringValue!;

            ctx.DataSection.AppendLine($"{label}: db \"{value}\", 0");

            // Push its address
            ctx.WriteText($"lea rax, [{label}]");
            ctx.EvaluationStack.Push("rax");
        }
    }
}
