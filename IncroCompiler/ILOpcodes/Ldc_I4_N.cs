

using System.Reflection.Emit;

namespace IncroCompiler.ILOpCodes
{
    public class Ldc_I4_N_Emitter : Interfaces.IEmitter
    {
         public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            int val = instr.Operand is int i ? i : 0;
            ctx.WriteText($"mov rax, {val}");
            ctx.EvaluationStack.Push("rax");
        }
    }
}
