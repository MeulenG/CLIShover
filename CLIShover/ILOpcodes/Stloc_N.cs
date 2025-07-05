using System.Reflection.Emit;
using CLIShover.Helpers;

namespace CLIShover.ILOpCodes
{
    public class Stloc_N_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            int localIndex = ExtractIndexFromOpCode(instr.OpCode);
            int offset = StackLayoutHelper.GetLocalOffset(localIndex);
            ctx.WriteLine("pop rax");
            ctx.WriteLine($"mov [rbp{offset}], rax");
        }

        private int ExtractIndexFromOpCode(OpCode opcode)
        {
            return opcode.Name switch
            {
                "stloc.0" => 0,
                "stloc.1" => 1,
                "stloc.2" => 2,
                "stloc.3" => 3,
                _ => throw new NotSupportedException($"Unsupported opcode: {opcode}")
            };
        }
    }
}
