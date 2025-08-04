

using System.Reflection.Emit;

namespace CLIShover.ILOpCodes
{
    public class Ldc_I4_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rax");
            ctx.WriteLine($"mov rax, {instr.Value}");
            ctx.WriteLine("push rax");
        }
    }
}
