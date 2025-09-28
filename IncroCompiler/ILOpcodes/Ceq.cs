namespace IncroCompiler.ILOpCodes
{
    public class Ceq_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var right = ctx.EvaluationStack.Pop();
            var left = ctx.EvaluationStack.Pop();

            ctx.WriteText($"cmp {left}, {right}");
            ctx.WriteText("sete al");
            ctx.WriteText("movzx rax, al");

            ctx.EvaluationStack.Push("rax");
        }
    }
}
