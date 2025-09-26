using System.Reflection;

namespace CLIShover.ILOpCodes
{
    public class Callvirt_Emitter : Interfaces.IEmitter
    {
        private readonly Dictionary<Type, ClassInfo> _classes;

        public Callvirt_Emitter(Dictionary<Type, ClassInfo> classes)
        {
            _classes = classes;
        }

        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            var methodToCall = (MethodInfo)instr.Operand!;
            var objectType = methodToCall.DeclaringType!;

            if (!_classes.TryGetValue(objectType, out var ci))
            {
                Logging.Log($"Warning: Class info not found for {objectType.FullName}. Skipping callvirt.", false);
                ctx.WriteLine($"; callvirt {methodToCall.Name} on external type {objectType.FullName} (skipped)");
                return;
            }

            int slot = ci.VTableSlots[methodToCall];

            // pop 'this' from IL eval stack into rdi (first arg register)
            ctx.WriteLine("pop rdi                ; this pointer");

            // null check
            ctx.WriteLine("test rdi, rdi");
            ctx.WriteLine("je __throw_nullref     ; throw if null");

            // load vtable pointer and method pointer
            ctx.WriteLine("mov rax, [rdi]        ; load vtable pointer");
            ctx.WriteLine($"mov rax, [rax + {slot * 8}] ; load virtual method slot");
            ctx.WriteLine("call rax");
        }
    }
}
