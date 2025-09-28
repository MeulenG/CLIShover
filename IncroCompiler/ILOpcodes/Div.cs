using IncroCompiler;

namespace IncroCompiler.ILOpCodes
{
    public class Div_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            // IL 'div' is integer division (wrap semantics). Use signed idiv by default.
            var right = ctx.EvaluationStack.Pop();
            var left = ctx.EvaluationStack.Pop();
            ctx.WriteText($"mov rax, {left}");
            ctx.WriteText("cqo");               // sign-extend rax -> rdx:rax
            ctx.WriteText($"mov rbx, {right}");
            ctx.WriteText("idiv rbx");          // quotient in rax
            ctx.EvaluationStack.Push("rax");
        }
    }
}
