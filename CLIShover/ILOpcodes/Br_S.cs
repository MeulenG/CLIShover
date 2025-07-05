namespace CLIShover.ILOpCodes
{
    public class Br_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("br.s ");
        }
    }
}
