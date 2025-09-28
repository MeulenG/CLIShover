using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace IncroCompiler
{
    public class ExecutionEngine
    {
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

            context.WriteData("section .data");
            foreach (var kvp in StringPool.GetAll())
            {
                context.WriteData($"{kvp.Key}: db \"{kvp.Value}\", 0");
            }

            context.WriteText("section .text");
            context.WriteText("global main");

            HashSet<MethodInfo> emittedMethods = new HashSet<MethodInfo>();

            foreach (var type in asm.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    if (method.DeclaringType != type) continue;
                    if (emittedMethods.Contains(method)) continue;

                    emittedMethods.Add(method);

                    string methodLabel = $"{type.Name}_{method.Name}";
                    context.AddLabel(methodLabel);
                    context.CurrentMethodName = methodLabel;

                    var instructions = ILReader.ReadInstructions(method);
                    var body = method.GetMethodBody();
                    int localVarCount = body?.LocalVariables.Count ?? 0;

                    int stackSize = ((localVarCount * 8 + 15) / 16) * 16;

                    context.WriteText("push rbp");
                    context.WriteText("mov rbp, rsp");
                    if (stackSize > 0)
                        context.WriteText($"sub rsp, {stackSize}");

                    foreach (var instr in instructions)
                    {
                        if (!string.IsNullOrEmpty(instr.Label) && !context.Labels.ContainsKey(instr.Offset))
                        {
                            context.Labels[instr.Offset] = instr.Label;
                        }

                        if (!string.IsNullOrEmpty(instr.NextLabel) && !context.Labels.ContainsKey(instr.Offset + 1))
                        {
                            context.Labels[instr.Offset + 1] = instr.NextLabel;
                        }
                    }

                    foreach (var instr in instructions)
                    {
                        if (context.Labels.TryGetValue(instr.Offset, out var label))
                            context.WriteText($"{label}:");

                        if (emitters.TryGetValue(instr.OpCode, out var emitter))
                        {
                            Logging.Log($"Emitting {instr.OpCode.Name} at {instr.Offset:X4}", true);
                            emitter.Emit(instr, context);
                        }
                        else
                        {
                            Logging.Log($"No emitter for opcode: {instr.OpCode}", false);
                            context.WriteText($"; unhandled opcode {instr.OpCode}");
                        }
                    }

                    context.WriteText("mov rsp, rbp");
                    context.WriteText("pop rbp");
                    context.WriteText("ret");
                    context.WriteText("");
                }
            }

            File.WriteAllText("output.asm", context.DataSection + context.ToString());
            Logging.Log("NASM code generated: output.asm", true);
        }
    }
}
