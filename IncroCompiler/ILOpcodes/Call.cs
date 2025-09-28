using System.Reflection;

namespace IncroCompiler.ILOpCodes
{
    public class Call_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var mi = (System.Reflection.MethodInfo)instr.Operand!;
            var target = $"{mi.DeclaringType!.Name}_{mi.Name}";
            ctx.WriteText($"call {target}");
            // push return value if any (assuming in rax)
            if (mi.ReturnType != typeof(void))
                ctx.EvaluationStack.Push("rax");
        }
    }
}
