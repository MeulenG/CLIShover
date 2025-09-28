namespace IncroCompiler.ILOpCodes
{
    public class Ldarg_1_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteText("mov rax, rsi");
            ctx.EvaluationStack.Push("rax");
        }
    }
}
