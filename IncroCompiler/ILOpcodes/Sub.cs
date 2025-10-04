using IncroCompiler;

namespace IncroCompiler.ILOpCodes
{
    public class Sub_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var right = ctx.EvaluationStack.Pop();
            var left = ctx.EvaluationStack.Pop();
            ctx.WriteText($"mov rax, {left}");
            ctx.WriteText($"sub rax, {right}");
            ctx.EvaluationStack.Push("rax");
        }
    }
}
