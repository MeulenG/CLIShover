using System.Reflection.Emit;
using IncroCompiler.Helpers;

namespace IncroCompiler.ILOpCodes
{
    public class Stloc_N_Emitter : Interfaces.IEmitter
    {
        private int _slotSize = 8;

        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            int idx = instr.Operand is int i ? i : 0;
            int offset = 8 + idx * _slotSize;
            var v = ctx.EvaluationStack.Pop();
            if (v != "rax") ctx.WriteText($"mov rax, {v}");
            ctx.WriteText($"mov [rbp-{offset}], rax");
        }
    }
}
