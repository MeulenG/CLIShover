namespace CLIShover.ILOpCodes
{
    public class Blt_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");
            ctx.WriteLine("pop rbx");
            ctx.WriteLine("cmp rax, rbx");
            ctx.WriteLine("jl " + instr.Label);
        }
    }
}
