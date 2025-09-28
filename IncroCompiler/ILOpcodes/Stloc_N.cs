using System.Reflection.Emit;
using IncroCompiler.Helpers;

namespace IncroCompiler.ILOpCodes
{
    public class Stloc_N_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            int idx = ExtractIndexFromOpCode(instr.OpCode);
            int offset = StackLayoutHelper.GetLocalOffset(idx);
            var v = ctx.EvaluationStack.Pop();
            ctx.WriteText($"mov rax, {v}");
            ctx.WriteText($"mov [rbp{offset}], rax");
        }

        private static int ExtractIndexFromOpCode(OpCode opcode) => opcode.Name switch
        {
            "stloc.0" => 0,
            "stloc.1" => 1,
            "stloc.2" => 2,
            "stloc.3" => 3,
            _ => throw new NotSupportedException($"Unsupported opcode: {opcode}")
        };
    }
}
