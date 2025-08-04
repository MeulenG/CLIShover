using CLIShover;

namespace CLIShover.ILOpCodes
{
    public class Stsfld_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");
            ctx.WriteLine("mov [" + instr.FieldValue + "], rax");
        }
    }
}
