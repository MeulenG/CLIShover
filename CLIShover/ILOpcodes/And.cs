using CLIShover;

namespace CLIShover.ILOpCodes
{
    public class And_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var right = ctx.EvaluationStack.Pop();
            var left  = ctx.EvaluationStack.Pop();

            ctx.WriteText($"and {left}, {right}");

            ctx.EvaluationStack.Push(left);
        }
    }
}
