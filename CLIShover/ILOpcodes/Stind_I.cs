namespace CLIShover.ILOpCodes
{
    public class Stind_I_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");       // value to store (native int, 64-bit)
            ctx.WriteLine("pop rcx");       // address
            ctx.WriteLine("mov [rcx], rax");// store 64-bit value at [rcx]
        }
    }
}
