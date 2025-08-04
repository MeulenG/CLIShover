namespace CLIShover.ILOpCodes
{
    public class Conv_U1_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");
            ctx.WriteLine("movsxd rax, eax");
            ctx.WriteLine("push rax");
        }
    }
}
