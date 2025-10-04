using System.Reflection.Emit;

namespace IncroCompiler.Interfaces
{
    public interface IEmitter
    {
        void Emit(ILInstruction instruc, EmitterContext ctx);
    }
}
