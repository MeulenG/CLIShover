namespace IncroCompiler.ILOpCodes
{
    public class Br_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            if (!(instr.Operand is int targetOffset))
            {
                ctx.WriteText("; br.s with no operand?");
                return;
            }

            string label = ctx.GetOrCreateLabel(ctx.CurrentMethodName,targetOffset);

            ctx.WriteText($"    jmp {label}");
        }
    }

}
