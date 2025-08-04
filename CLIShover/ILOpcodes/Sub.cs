using CLIShover;

namespace CLIShover.ILOpCodes
{
    public class Sub_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");
            ctx.WriteLine("pop rbx");
            ctx.WriteLine("sub rax, rbx");
            ctx.WriteLine("push rax");
        }
    }
}
