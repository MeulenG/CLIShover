using System.Reflection;
using System.Reflection.Emit;

namespace IncroCompiler
{
    public class ILInstruction
    {
        public int Offset { get; set; }
        public OpCode OpCode { get; set; }
        public object? Operand { get; set; }
        public int Value => Operand is int intValue ? intValue : 0;
        public string StringValue => Operand is string strValue ? strValue : string.Empty;
        public FieldInfo? FieldValue => Operand as FieldInfo;
        public string Label { get; set; } = string.Empty;
        public string NextLabel { get; set; } = string.Empty;
    }
}
