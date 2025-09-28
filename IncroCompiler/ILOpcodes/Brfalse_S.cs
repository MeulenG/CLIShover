namespace IncroCompiler.ILOpCodes
{
    public class Brfalse_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var v = ctx.EvaluationStack.Pop();
            ctx.WriteText($"cmp {v}, 0");
            ctx.WriteText($"je {instr.Label}");
        }
    }
}
