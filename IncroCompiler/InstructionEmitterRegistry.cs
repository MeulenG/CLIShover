using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace IncroCompiler
{
    public static class InstructionEmitterRegistry
    {
        public static Dictionary<OpCode, Interfaces.IEmitter> Create(Dictionary<Type, ClassInfo> classes, Dictionary<MethodInfo, Action<EmitterContext>> intrinsics)
        {
            return new Dictionary<OpCode, Interfaces.IEmitter>
            {
                { OpCodes.Add,      new ILOpCodes.Add_Emitter() },
                { OpCodes.Sub,      new ILOpCodes.Sub_Emitter() },
                { OpCodes.Mul,      new ILOpCodes.Mul_Emitter() },
                { OpCodes.Div,      new ILOpCodes.Div_Emitter() },
                { OpCodes.And,      new ILOpCodes.And_Emitter() },
                { OpCodes.Ret,      new ILOpCodes.Ret_Emitter() },
                { OpCodes.Nop,      new ILOpCodes.Nop_Emitter() },
                { OpCodes.Blt_S,    new ILOpCodes.Blt_S_Emitter() },
                { OpCodes.Conv_I,   new ILOpCodes.Conv_I_Emitter() },
                { OpCodes.Conv_U1,  new ILOpCodes.Conv_U1_Emitter() },
                { OpCodes.Ldsfld,   new ILOpCodes.Ldsfld_Emitter() },
                { OpCodes.Ldstr,    new ILOpCodes.Ldstr_Emitter() },
                { OpCodes.Stsfld,   new ILOpCodes.Stsfld_Emitter() },
                { OpCodes.Ldc_I4,   new ILOpCodes.Ldc_I4_Emitter() },
                { OpCodes.Ldc_I4_0, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_1, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_2, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_3, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_4, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_5, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_6, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_7, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_8, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_S, new ILOpCodes.Ldc_I4_S_Emitter() },
                { OpCodes.Stloc_0,  new ILOpCodes.Stloc_N_Emitter() },
                { OpCodes.Stloc_1,  new ILOpCodes.Stloc_N_Emitter() },
                { OpCodes.Stloc_2,  new ILOpCodes.Stloc_N_Emitter() },
                { OpCodes.Stloc_3,  new ILOpCodes.Stloc_N_Emitter() },
                { OpCodes.Stind_I,  new ILOpCodes.Stind_I_Emitter() },
                { OpCodes.Stind_I1, new ILOpCodes.Stind_I_1_Emitter() },
                { OpCodes.Stind_I2, new ILOpCodes.Stind_I_2_Emitter() },
                { OpCodes.Stind_I4, new ILOpCodes.Stind_I_4_Emitter() },
                { OpCodes.Stind_I8, new ILOpCodes.Stind_I_8_Emitter() },
                { OpCodes.Ldloc_0,  new ILOpCodes.Ldloc_N_Emitter() },
                { OpCodes.Ldloc_1,  new ILOpCodes.Ldloc_N_Emitter() },
                { OpCodes.Ldloc_2,  new ILOpCodes.Ldloc_N_Emitter() },
                { OpCodes.Ldloc_3,  new ILOpCodes.Ldloc_N_Emitter() },
                { OpCodes.Ldarg_0,  new ILOpCodes.Ldarg_0_Emitter() },
                { OpCodes.Call,     new ILOpCodes.Call_Emitter() },
                // Pass classes dictionary to Callvirt_Emitter
                { OpCodes.Callvirt, new ILOpCodes.Callvirt_Emitter(classes, intrinsics) },
                { OpCodes.Ldarg_1,  new ILOpCodes.Ldarg_1_Emitter() },
                { OpCodes.Ceq,      new ILOpCodes.Ceq_Emitter() },
                { OpCodes.Br_S,     new ILOpCodes.Br_S_Emitter() },
                { OpCodes.Brfalse_S, new ILOpCodes.Brfalse_S_Emitter() }
            };
        }
    }
}
