using IncroCompiler;

namespace IncroCompiler.ILOpCodes
{
    // ldsfld  (push value of static field)
    // NOTE: Without type info, we assume a 64-bit value load; adjust if you know the field type.
    public class Ldsfld_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            // Expect instr.Operand to be System.Reflection.FieldInfo
            if (instr.Operand is System.Reflection.FieldInfo fi)
            {
                var lbl = ctx.InternField(fi);            // <-- needs small helper on ctx
                ctx.WriteText($"mov rax, [rel {lbl}]");   // load field value
                ctx.EvaluationStack.Push("rax");
            }
            else
            {
                // Fallback if your ILReader put something else in .FieldValue
                ctx.WriteText($"; ldsfld unsupported operand: {instr.Operand}");
            }
        }
    }
}
