namespace CLIShover.ILOpCodes
{
    public class Ldarg_1_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("mov rax, rsi");
            ctx.WriteLine("push rax");
        }
    }
}
