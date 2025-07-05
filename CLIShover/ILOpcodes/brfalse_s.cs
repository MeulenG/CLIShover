namespace CLIShover.ILOpCodes
{
    public class Brfalse_S_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("cmp rax, 0");
        }
    }
}
