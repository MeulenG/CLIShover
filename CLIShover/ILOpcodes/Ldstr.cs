using CLIShover;

namespace CLIShover.ILOpCodes
{
    public class Ldstr_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("mov rax, " + instr.StringValue);
            ctx.WriteLine("push rax");
        }
    }
}
