namespace CLIShover.ILOpCodes
{
    public class Callvirt_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");                   // load 'this' pointer (could be ignored for static)
            ctx.WriteLine("; call method goes here");   // comment placeholder
            ctx.WriteLine("; maybe null-check?");       // reminder to check for null
        }
    }
}
