namespace CLIShover.ILOpCodes
{
    public class Ret_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("ret");
        }
    }
}
