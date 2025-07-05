using System.Reflection.Emit;
using CLIShover.Helpers;

namespace CLIShover.ILOpCodes
{
    public class Ldloc_N_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            int localIndex = ExtractIndexFromOpCode(instr.OpCode);
            int offset = StackLayoutHelper.GetLocalOffset(localIndex);
            ctx.WriteLine($"mov rax, [rbp{offset}]");
            ctx.WriteLine("push rax");
        }

        private int ExtractIndexFromOpCode(OpCode opcode)
        {
            return opcode.Name switch
            {
                "ldloc.0" => 0,
                "ldloc.1" => 1,
                "ldloc.2" => 2,
                "ldloc.3" => 3,
                _ => throw new NotSupportedException($"Unsupported opcode: {opcode}")
            };
        }
    }
}
