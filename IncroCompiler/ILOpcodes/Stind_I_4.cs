namespace IncroCompiler.ILOpCodes
{
    public class Stind_I_4_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var value = ctx.EvaluationStack.Pop();
            var addr = ctx.EvaluationStack.Pop();
            ctx.WriteText($"mov rcx, {addr}");
            ctx.WriteText($"mov rax, {value}");
            ctx.WriteText("mov dword [rcx], eax");
        }
    }
}
