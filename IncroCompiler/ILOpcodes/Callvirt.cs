using System.Reflection;

namespace IncroCompiler.ILOpCodes
{
 public class Callvirt_Emitter : Interfaces.IEmitter
    {
        private readonly Dictionary<Type, ClassInfo> _classes;

        // Optional: table of OS intrinsics
        private readonly Dictionary<MethodInfo, Action<EmitterContext>> _intrinsics;

        public Callvirt_Emitter(Dictionary<Type, ClassInfo> classes, Dictionary<MethodInfo, Action<EmitterContext>> intrinsics)
        {
            _classes = classes;
            _intrinsics = intrinsics ?? new Dictionary<MethodInfo, Action<EmitterContext>>();
        }

        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            if (!(instr.Operand is MethodInfo methodToCall))
                throw new InvalidOperationException("Callvirt operand must be a MethodInfo.");

            var objectType = methodToCall.DeclaringType!;

            if (_intrinsics.TryGetValue(methodToCall, out var impl))
            {
                impl(ctx);
                return;
            }

            if (!_classes.TryGetValue(objectType, out var ci))
            {
                ctx.WriteText($"; skipping callvirt to external type {objectType.FullName}");
                return;
            }

            if (!ci.VTableSlots.TryGetValue(methodToCall, out int slot))
                throw new InvalidOperationException($"Method {methodToCall.Name} not found in vtable for {objectType.Name}");

            ctx.WriteText("pop rdi                ; this pointer");

            ctx.WriteText("test rdi, rdi");
            ctx.WriteText("je __throw_nullref     ; throw if null");

            ctx.WriteText("mov rax, [rdi]        ; load vtable pointer");
            ctx.WriteText($"mov rax, [rax + {slot * 8}] ; load virtual method slot");
            ctx.WriteText("call rax");

            ctx.EvaluationStack.Push("rax");
        }
    }
}
