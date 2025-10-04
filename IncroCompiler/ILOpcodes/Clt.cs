namespace IncroCompiler.ILOpCodes
{
    public class Clt_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var right = ctx.EvaluationStack.Pop();
            var left = ctx.EvaluationStack.Pop();

            ctx.WriteText($"cmp {left}, {right}");
            // if value1 < value2, set flag to 1 and push to stack, else push 0
            ctx.WriteText("setl al");
            ctx.WriteText("movzx rax, al");

            ctx.EvaluationStack.Push("rax");
        }
    }
}
