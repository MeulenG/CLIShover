namespace IncroCompiler.ILOpCodes
{
    public class Br_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteText($"jmp {instr.Label}");
        }
    }
}
