namespace CLIShover.ILOpCodes
{
    public class Call_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("call ");
        }
    }
}
