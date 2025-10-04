using System;
using System.Reflection;

namespace IncroCompiler.ILOpCodes
{
    public class Callvirt_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            if (!(instr.Operand is MethodInfo methodToCall))
                throw new InvalidOperationException("MissingMethodException: callvirt requires a MethodInfo operand");

            var parameters = methodToCall.GetParameters();
            for (int i = parameters.Length - 1; i >= 0; i--)
            {
                ctx.EvaluationStack.Pop();
            }

            if (!methodToCall.IsStatic)
            {
                ctx.EvaluationStack.Pop();
            }

            string methodLabel = $"{Sanitize(methodToCall.DeclaringType!.Name)}_{methodToCall.Name}_{methodToCall.MetadataToken:X}";
            ctx.WriteText($"    ; callvirt {methodToCall.DeclaringType.FullName}::{methodToCall.Name}");
            ctx.WriteText($"    call {methodLabel}");

            if (methodToCall.ReturnType != typeof(void))
            {
                ctx.EvaluationStack.Push("rax");
            }
        }

        private static string Sanitize(string s)
        {
            if (string.IsNullOrEmpty(s)) return "M";
            var arr = s.Select(c => char.IsLetterOrDigit(c) || c == '_' ? c : '_').ToArray();
            return new string(arr);
        }
    }
}
