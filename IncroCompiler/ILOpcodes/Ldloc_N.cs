using System.Reflection.Emit;
using IncroCompiler.Helpers;

namespace IncroCompiler.ILOpCodes
{
    public class Ldloc_N_Emitter : Interfaces.IEmitter
    {
        private int _slotSize = 8;

        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            int idx = instr.Operand is int i ? i : 0;
            int offset = 8 + idx * _slotSize;
            ctx.WriteText($"mov rax, [rbp-{offset}]");
            ctx.EvaluationStack.Push("rax");
        }
    }
}
