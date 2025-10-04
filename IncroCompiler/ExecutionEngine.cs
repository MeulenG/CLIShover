using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.IO;

namespace IncroCompiler
{
    public class ExecutionEngine
    {
        string MakeMethodScopedLabel(string methodName, int offset)
            => $"{Sanitize(methodName)}_L_{offset:X4}";

        string MakeMethodEntryLabel(MethodInfo m)
            => $"{Sanitize(m.DeclaringType.Name)}_{Sanitize(m.Name)}_{m.MetadataToken:X}";

        // Assign labels for branch targets and preserve IL labels, method-scoped
        void AssignBranchLabels(List<ILInstruction> instructions, EmitterContext ctx, string methodName)
        {
            // Branch opcodes that have an inline target
            var branchOpcodes = new HashSet<OpCode>
            {
                OpCodes.Br, OpCodes.Br_S,
                OpCodes.Brtrue, OpCodes.Brtrue_S,
                OpCodes.Brfalse, OpCodes.Brfalse_S,
                OpCodes.Blt, OpCodes.Blt_S,
                OpCodes.Ble, OpCodes.Ble_S,
                OpCodes.Bgt, OpCodes.Bgt_S,
                OpCodes.Bge, OpCodes.Bge_S,
                OpCodes.Beq, OpCodes.Beq_S,
                OpCodes.Bne_Un, OpCodes.Bne_Un_S
            };

            // 1) Preserve IL-provided labels (if your IL reader sets instr.Label)
            foreach (var instr in instructions)
            {
                if (!string.IsNullOrEmpty(instr.Label))
                {
                    // Create a method-scoped label prefixed by methodName (avoid collisions)
                    var lbl = $"{Sanitize(methodName)}_{instr.Label}";
                    ctx.AddLabel(methodName, instr.Offset, lbl);
                }
            }

            // 2) For any instruction that has an operand that is an IL offset, ensure a label exists
            foreach (var instr in instructions)
            {
                if (instr.Operand is int target)
                {
                    // Create method-scoped label (if missing)
                    ctx.GetOrCreateLabel(methodName, target);
                }
            }

            // Note: this is idempotent and cheap; DetectLoopsFixed will add loop-specific labels too.
        }

        // Detect backward branches (loops) using the real next-instruction offset
        void DetectLoopsFixed(List<ILInstruction> instructions, EmitterContext ctx, string methodName)
        {
            var loopBranches = new HashSet<OpCode>
            {
                OpCodes.Br_S, OpCodes.Br,
                OpCodes.Brtrue, OpCodes.Brtrue_S,
                OpCodes.Brfalse, OpCodes.Brfalse_S,
                OpCodes.Blt, OpCodes.Blt_S,
                OpCodes.Bge, OpCodes.Bge_S,
                OpCodes.Ble, OpCodes.Ble_S,
                OpCodes.Bgt, OpCodes.Bgt_S
            };

            for (int i = 0; i < instructions.Count; i++)
            {
                var instr = instructions[i];
                if (!loopBranches.Contains(instr.OpCode)) continue;
                if (!(instr.Operand is int targetOffset)) continue;

                // backward branch -> loop
                if (targetOffset < instr.Offset)
                {
                    // ensure loop start label exists (method-scoped)
                    ctx.GetOrCreateLabel(methodName, targetOffset);

                    // compute the offset of the instruction after this branch:
                    int nextInstrOffset;
                    if (i + 1 < instructions.Count)
                        nextInstrOffset = instructions[i + 1].Offset;
                    else
                        nextInstrOffset = instr.Offset + 1; // fallback; unusual

                    // create a loop-end label (method-scoped) if missing
                    // use a readable name but keep method scoping
                    var endLabel = $"{Sanitize(methodName)}_LOOP_END_{nextInstrOffset:X4}";
                    if (!ctx.TryGetLabel(methodName, nextInstrOffset, out _))
                        ctx.AddLabel(methodName, nextInstrOffset, endLabel);

                    Console.WriteLine($"[LOOP] detected {instr.Offset:X4} -> {targetOffset:X4} (start={ctx.GetOrCreateLabel(methodName, targetOffset)}, end={endLabel})");
                }
                else
                {
                    // forward branch -> ensure a label exists for target (already handled by AssignBranchLabels normally)
                    ctx.GetOrCreateLabel(methodName, targetOffset);
                }
            }
        }

        // Simple sanitizer for label-safe names
        private static string Sanitize(string s)
        {
            if (string.IsNullOrEmpty(s)) return "M";
            var arr = s.Select(c => (char.IsLetterOrDigit(c) || c == '_' ? c : '_')).ToArray();
            return new string(arr);
        }

        // --- Main Execute method (uses the fixed label system)
        public void Execute(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Please provide a valid file path.");
                return;
            }

            var asm = Assembly.LoadFile(filePath);
            Logging.Log("IL → NASM translator", true);

            var classes = ClassInfoBuilder.Build(asm);
            var emitters = InstructionEmitterRegistry.Create(classes, new Dictionary<MethodInfo, Action<EmitterContext>>());
            var context = new EmitterContext();

            // Data section
            context.WriteData("section .data");
            foreach (var kvp in StringPool.GetAll())
                context.WriteData($"{kvp.Key}: db \"{kvp.Value}\", 0");

            // Text section
            context.WriteText("section .text");
            context.WriteText("global main");

            var emittedMethods = new HashSet<MethodInfo>();

            foreach (var type in asm.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    if (method.DeclaringType != type) continue;
                    if (emittedMethods.Contains(method)) continue;

                    emittedMethods.Add(method);

                    // Create a unique method label (include metadata token to avoid overload collisions)
                    string methodLabel = MakeMethodEntryLabel(method);
                    context.CurrentMethodName = methodLabel;

                    // Read IL
                    var instructions = ILReader.ReadInstructions(method).ToList();

                    // FIRST: label assignment and loop detection (both add method-scoped labels)
                    AssignBranchLabels(instructions, context, methodLabel);
                    DetectLoopsFixed(instructions, context, methodLabel);

                    // compute stack size (coarse)
                    var body = method.GetMethodBody();
                    int localVarCount = body?.LocalVariables.Count ?? 0;
                    int stackSize = ((localVarCount * 8 + 15) / 16) * 16;

                    // Emit method prologue once
                    context.WriteText($"{methodLabel}:");
                    context.WriteText("    push rbp");
                    context.WriteText("    mov rbp, rsp");
                    if (stackSize > 0)
                        context.WriteText($"    sub rsp, {stackSize}");

                    // Emit instructions in order, emitting method-scoped labels before instructions when present
                    for (int i = 0; i < instructions.Count; i++)
                    {
                        var instr = instructions[i];

                        // If a method-scoped label exists for this offset, emit it
                        if (context.TryGetLabel(methodLabel, instr.Offset, out var label))
                            context.WriteText($"{label}:");

                        // Emit the instruction via registered emitter or fallback comment
                        if (emitters.TryGetValue(instr.OpCode, out var emitter))
                        {
                            Logging.Log($"Emitting {instr.OpCode.Name} at {instr.Offset:X4}", true);
                            emitter.Emit(instr, context);
                        }
                        else
                        {
                            Logging.Log($"No emitter for opcode: {instr.OpCode}", false);
                            context.WriteText($"    ; unhandled opcode {instr.OpCode}");
                        }
                    }

                    // Emit method epilogue
                    context.WriteText("    mov rsp, rbp");
                    context.WriteText("    pop rbp");
                    context.WriteText("    ret");
                    context.WriteText("");
                }
            }

            File.WriteAllText("output.asm", context.DataSection + context.ToString());
            Logging.Log("NASM code generated: output.asm", true);
        }
    }
}
