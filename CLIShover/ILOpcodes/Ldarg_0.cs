namespace CLIShover.ILOpCodes
{
    public class Ldarg_0_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteText("mov rax, rdi");
            ctx.EvaluationStack.Push("rax"); // track value on our virtual stack
        }
    }
}
