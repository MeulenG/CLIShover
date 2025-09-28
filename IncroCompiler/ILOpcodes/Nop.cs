namespace IncroCompiler.ILOpCodes
{
    public class Nop_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteText("nop");
        }
    }
}
