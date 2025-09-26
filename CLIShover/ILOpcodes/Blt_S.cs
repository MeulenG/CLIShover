namespace CLIShover.ILOpCodes
{
    public class Blt_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var right = ctx.EvaluationStack.Pop();
            var left = ctx.EvaluationStack.Pop();

            ctx.WriteText($"cmp {left}, {right}");

            ctx.WriteText($"jl {instr.Label}");
        }
    }
}
