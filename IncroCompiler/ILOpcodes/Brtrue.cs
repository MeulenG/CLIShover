using System;
using System.Reflection;
namespace IncroCompiler.ILOpCodes
{
    public class Brtrue_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            if (!(instr.Operand is int targetOffset))
            {
                ctx.WriteText("; brtrue.s missing target");
                return;
            }

            var value = ctx.EvaluationStack.Pop();

            if (value != "rax") ctx.WriteText($"mov rax, {value}");
            string lbl = ctx.GetOrCreateLabel(ctx.CurrentMethodName, targetOffset);

            ctx.WriteText("cmp rax, 0");
            ctx.WriteText($"jne {lbl}");
        }
    }
}
