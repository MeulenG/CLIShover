using System.Reflection.Emit;
using IncroCompiler.Helpers;

namespace IncroCompiler.ILOpCodes
{
    public class Ldloc_N_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            int idx = ExtractIndexFromOpCode(instr.OpCode);
            int offset = StackLayoutHelper.GetLocalOffset(idx); // NOTE: should be 8-byte slots on x64
            ctx.WriteText($"mov rax, [rbp{offset}]");
            ctx.EvaluationStack.Push("rax");
        }

        private static int ExtractIndexFromOpCode(OpCode opcode) => opcode.Name switch
        {
            "ldloc.0" => 0,
            "ldloc.1" => 1,
            "ldloc.2" => 2,
            "ldloc.3" => 3,
            _ => throw new NotSupportedException($"Unsupported opcode: {opcode}")
        };
    }
}
