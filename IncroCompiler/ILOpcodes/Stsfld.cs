using IncroCompiler;

namespace IncroCompiler.ILOpCodes
{
    public class Stsfld_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var value = ctx.EvaluationStack.Pop();
            var field = instr.FieldValue;

            ctx.WriteText($"mov [{field}], {value}");
            // No push — storing ends evaluation
        }
    }
}
