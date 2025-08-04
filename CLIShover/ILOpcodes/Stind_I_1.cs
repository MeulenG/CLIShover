namespace CLIShover.ILOpCodes
{
    public class Stind_I_1_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");       // value (in rax)
            ctx.WriteLine("pop rcx");       // address
            ctx.WriteLine("mov byte [rcx], al"); // store lowest byte
        }
    }
}
