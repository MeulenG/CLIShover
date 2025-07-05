using System.Reflection.Emit;

namespace CLIShover
{
    public class ILInstruction
    {
        public int Offset { get; set; }
        public OpCode OpCode { get; set; }
        public object? Operand { get; set; }
    }
}
