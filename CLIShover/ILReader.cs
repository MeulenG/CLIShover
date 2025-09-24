using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Buffers.Binary;

namespace CLIShover
{
    public static class ILReader
    {
        private static readonly OpCode[] SingleByteOpCodes = new OpCode[0x100];
        private static readonly OpCode[] MultiByteOpCodes = new OpCode[0x100];

        static ILReader()
        {
            foreach (var fi in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (fi.GetValue(null) is OpCode op)
                {
                    if (op.Size == 1)
                        SingleByteOpCodes[op.Value] = op;
                    else
                        MultiByteOpCodes[op.Value & 0xff] = op;
                }
            }
        }

        public static List<ILInstruction> ReadInstructions(MethodInfo method)
        {
            var body = method.GetMethodBody();
            if (body == null)
                return new List<ILInstruction>();

            var il = body.GetILAsByteArray();
            var instructions = new List<ILInstruction>();

            int pos = 0;
            while (pos < il?.Length)
            {
                int offset = pos;
                OpCode opcode;
                byte code = il[pos++];

                if (code == 0xFE)
                {
                    opcode = MultiByteOpCodes[il[pos++]];
                }
                else
                {
                    opcode = SingleByteOpCodes[code];
                }
                Console.WriteLine($"Opcode: {opcode}, OperandType: {opcode.OperandType}, pos: {pos:X4}");
                object? operand = ReadOperand(il, ref pos, opcode.OperandType, method, new ILInstruction { Offset = offset, OpCode = opcode });
                instructions.Add(new ILInstruction { Offset = offset, OpCode = opcode, Operand = operand });
            }

            return instructions;
        }

        private static object? ReadOperand(byte[] il, ref int pos, OperandType type, MethodInfo method, ILInstruction instr)
        {
            switch (type)
            {
                case OperandType.InlineNone:
                    return null;
                case OperandType.ShortInlineI:
                    return (sbyte)il[pos++];
                case OperandType.InlineI:
                    var i = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    return i;
                case OperandType.InlineI8:
                    var i8 = BitConverter.ToInt64(il, pos);
                    pos += 8;
                    return i8;
                case OperandType.ShortInlineR:
                    var f32 = BitConverter.ToSingle(il, pos);
                    pos += 4;
                    return f32;
                case OperandType.InlineR:
                    var f64 = BitConverter.ToDouble(il, pos);
                    pos += 8;
                    return f64;
                case OperandType.ShortInlineVar:
                    return il[pos++];
                case OperandType.InlineVar:
                    var varIndex = BitConverter.ToUInt16(il, pos);
                    pos += 2;
                    return varIndex;
                case OperandType.InlineString:
                    int stringToken = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    return method.Module.ResolveString(stringToken);
                case OperandType.InlineField:
                    int token = BinaryPrimitives.ReadInt32LittleEndian(il.AsSpan(pos));
                    pos += 4;
                    return method.Module.ResolveField(token);
                case OperandType.InlineType:
                    int typeToken = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    return method.Module.ResolveType(typeToken);
                case OperandType.InlineMethod:
                    int methodToken = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    return method.Module.ResolveMethod(methodToken);
                case OperandType.InlineTok:
                    int tokToken = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    return method.Module.ResolveMember(tokToken);
                case OperandType.InlineSig:
                    var metadataToken = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    return metadataToken;
                case OperandType.InlineSwitch:
                    int count = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    int[] deltas = new int[count];
                    for (int j = 0; j < count; j++)
                    {
                        deltas[j] = BitConverter.ToInt32(il, pos);
                        pos += 4;
                    }
                    return deltas;
                case OperandType.InlineBrTarget:
                    int relTarget = BitConverter.ToInt32(il, pos);
                    pos += 4;
                    return relTarget + pos;
                case OperandType.ShortInlineBrTarget:
                    sbyte sRel = (sbyte)il[pos++];
                    return sRel + pos;
                default:
                    throw new NotSupportedException($"Unsupported operand type: {type}");
            }
        }
    }
}
