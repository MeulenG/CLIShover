using System.Reflection.Emit;

namespace CLIShover.Interfaces
{
    public interface IEmitter
    {
        void Emit(ILInstruction instruc, EmitterContext ctx);
    }
}
