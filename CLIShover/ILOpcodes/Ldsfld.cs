using CLIShover;

namespace CLIShover.ILOpCodes
{
    public class Ldsfld_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("mov rax, " + instr.FieldValue);
            ctx.WriteLine("push rax");
        }
    }
}
