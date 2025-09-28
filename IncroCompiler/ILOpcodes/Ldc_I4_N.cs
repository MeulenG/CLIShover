

using System.Reflection.Emit;

namespace IncroCompiler.ILOpCodes
{
    public class Ldc_I4_N_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            int v = ExtractConstantFromOpCode(instr.OpCode);
            ctx.WriteText($"mov eax, {v}");
            ctx.WriteText("cdqe");
            ctx.EvaluationStack.Push("rax");
        }

        private static int ExtractConstantFromOpCode(OpCode opcode) => opcode.Name switch
        {
            "ldc.i4.0" => 0,
            "ldc.i4.1" => 1,
            "ldc.i4.2" => 2,
            "ldc.i4.3" => 3,
            "ldc.i4.4" => 4,
            "ldc.i4.5" => 5,
            "ldc.i4.6" => 6,
            "ldc.i4.7" => 7,
            "ldc.i4.8" => 8,
            _ => throw new NotSupportedException($"Unsupported opcode: {opcode}")
        };
    }
}
