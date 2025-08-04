namespace CLIShover.ILOpCodes
{
    public class Stind_I_8_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");
            ctx.WriteLine("pop rcx");
            ctx.WriteLine("mov [rcx], rax");
        }
    }
}
