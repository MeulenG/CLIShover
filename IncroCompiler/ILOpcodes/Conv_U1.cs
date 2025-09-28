namespace IncroCompiler.ILOpCodes
{
    public class Conv_U1_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var value = ctx.EvaluationStack.Pop();
            ctx.WriteText($"mov rax, {value}");
            ctx.WriteText("movsxd rax, eax");
            ctx.EvaluationStack.Push("rax");
        }
    }
}
