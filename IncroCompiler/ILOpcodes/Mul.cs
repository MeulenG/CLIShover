using IncroCompiler;

namespace IncroCompiler.ILOpCodes
{
    public class Mul_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var right = ctx.EvaluationStack.Pop();
            var left = ctx.EvaluationStack.Pop();
            ctx.WriteText($"mov rax, {left}");
            ctx.WriteText($"mov rbx, {right}");
            ctx.WriteText("imul rax, rbx");   // 2-operand IMUL, result in rax (wrap semantics)
            ctx.EvaluationStack.Push("rax");
        }
    }
}
