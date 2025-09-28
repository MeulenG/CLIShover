

using System.Reflection.Emit;

namespace IncroCompiler.ILOpCodes
{
    public class Ldc_I4_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            if (instr.Operand is int constantValue)
            {
                ctx.WriteText($"mov eax, {constantValue}");
                ctx.WriteText("cdqe");                 // sign-extend eax -> rax
                ctx.EvaluationStack.Push("rax");
            }
            else
            {
                throw new InvalidOperationException($"Expected int operand for ldc.i4, got: {instr.Operand}");
            }
        }
    }
}
