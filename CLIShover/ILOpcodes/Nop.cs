namespace CLIShover.ILOpCodes
{
    public class Nop_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("nop");
        }
    }
}
