using System.Collections.Generic;
using System.Reflection.Emit;


namespace CLIShover
{
    public static class InstructionEmitterRegistry
    {
        public static Dictionary<OpCode, Interfaces.IEmitter> Create()
        {
            return new Dictionary<OpCode, Interfaces.IEmitter>
            {
                { OpCodes.Add,      new ILOpCodes.Add_Emitter() },
                { OpCodes.Ret,      new ILOpCodes.Ret_Emitter() },
                { OpCodes.Nop,      new ILOpCodes.Nop_Emitter() },
                { OpCodes.Ldc_I4_S, new ILOpCodes.Ldc_I4_S_Emitter() },

                // IL mnemonic: ldc = load constant, i4 = 4-byte integer, 0-8 constant pushed to the stack
                { OpCodes.Ldc_I4_0, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_1, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_2, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_3, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_4, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_5, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_6, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_7, new ILOpCodes.Ldc_I4_N_Emitter() },
                { OpCodes.Ldc_I4_8, new ILOpCodes.Ldc_I4_N_Emitter() },
                // IL mnemonic: stloc = store local variable, loc = local variable, 0-3 local variables
                { OpCodes.Stloc_0,  new ILOpCodes.Stloc_N_Emitter() },
                { OpCodes.Stloc_1,  new ILOpCodes.Stloc_N_Emitter() },
                { OpCodes.Stloc_2,  new ILOpCodes.Stloc_N_Emitter() },
                { OpCodes.Stloc_3,  new ILOpCodes.Stloc_N_Emitter() },
                { OpCodes.Ldloc_0,  new ILOpCodes.Ldloc_N_Emitter() },
                { OpCodes.Ldloc_1,  new ILOpCodes.Ldloc_N_Emitter() },
                { OpCodes.Ldarg_0,  new ILOpCodes.Ldarg_0_Emitter() },
                { OpCodes.Call,     new ILOpCodes.Call_Emitter() },
                { OpCodes.Stloc_2,  new ILOpCodes.Stloc_N_Emitter() },
                { OpCodes.Ldloc_2,  new ILOpCodes.Ldloc_N_Emitter() },
                { OpCodes.Ldloc_3,  new ILOpCodes.Ldloc_N_Emitter() },
                { OpCodes.Callvirt, new ILOpCodes.Callvirt_Emitter() },
                { OpCodes.Ldarg_1,  new ILOpCodes.Ldarg_1_Emitter() },
                { OpCodes.Ceq,      new ILOpCodes.Ceq_Emitter() },
                { OpCodes.Br_S,     new ILOpCodes.Br_S_Emitter() },
                { OpCodes.Brfalse_S, new ILOpCodes.Brfalse_S_Emitter() }
            };
        }
    }
}
