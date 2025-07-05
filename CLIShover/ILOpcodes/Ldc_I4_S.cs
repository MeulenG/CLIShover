namespace CLIShover.ILOpCodes
{
    public class Ldc_I4_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            if (instr.Operand is sbyte value)
            {
                ctx.WriteLine($"mov rax, {value}");
                ctx.WriteLine("push rax");
            }
            else
            {
                throw new InvalidOperationException($"Expected sbyte operand for ldc.i4.s, got: {instr.Operand}");
            }
        }
    }
}
