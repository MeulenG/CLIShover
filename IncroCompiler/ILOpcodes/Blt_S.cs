namespace IncroCompiler.ILOpCodes
{
    public class Blt_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            if (!(instr.Operand is int targetOffset))
            {
                ctx.WriteText("; blt.s missing target");
                return;
            }

            var right = ctx.EvaluationStack.Pop();
            var left  = ctx.EvaluationStack.Pop();

            if (right != "rax") ctx.WriteText($"mov rax, {right}");
            if (left  != "rbx") ctx.WriteText($"mov rbx, {left}");

            ctx.WriteText("cmp rbx, rax");
            string targetLabel = ctx.GetOrCreateLabel(ctx.CurrentMethodName, targetOffset);
            ctx.WriteText($"jl {targetLabel}");
        }
    }
}
