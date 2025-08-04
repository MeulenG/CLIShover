namespace CLIShover.ILOpCodes
{
    public class Stind_I_2_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");       // value
            ctx.WriteLine("pop rcx");       // address
            ctx.WriteLine("mov word [rcx], ax"); // store lowest 16 bits
        }
    }
}
