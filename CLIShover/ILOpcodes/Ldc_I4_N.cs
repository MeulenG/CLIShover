

using System.Reflection.Emit;

namespace CLIShover.ILOpCodes
{
    public class Ldc_I4_N_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            int constantValue = ExtractConstantFromOpCode(instr.OpCode);
            ctx.WriteLine($"mov rax, {constantValue}");
            ctx.WriteLine("push rax");
        }

        private int ExtractConstantFromOpCode(OpCode opcode)
        {
            return opcode.Name switch
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
}
