using IncroCompiler;

namespace IncroCompiler.ILOpCodes
{
    public class Add_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var right = ctx.EvaluationStack.Pop();
            var left  = ctx.EvaluationStack.Pop();

            ctx.WriteText($"add {left}, {right}");

            ctx.EvaluationStack.Push(left);
        }
    }
}
