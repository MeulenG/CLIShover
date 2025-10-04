namespace IncroCompiler.ILOpCodes
{
    public class Ldc_I4_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            if (instr.Operand is sbyte value)
            {
                ctx.WriteText($"mov eax, {value}"); // mov with signed imm works (e.g., -5)
                ctx.WriteText("cdqe");
                ctx.EvaluationStack.Push("rax");
            }
            else
            {
                throw new InvalidOperationException($"Expected sbyte operand for ldc.i4.s, got: {instr.Operand}");
            }
        }
    }
}
