namespace CLIShover.ILOpCodes
{
    public class Stind_I_4_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");       // value
            ctx.WriteLine("pop rcx");       // address
            ctx.WriteLine("mov dword [rcx], eax"); // store lowest 32 bits
        }
    }
}
