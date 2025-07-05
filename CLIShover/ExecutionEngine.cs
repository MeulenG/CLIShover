using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics.Tracing;
using System.Diagnostics;

namespace CLIShover
{
    public class ExecutionEngine
    {
        public void Execute(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Please provide a valid file path to the assembly file.");
                return;
            }
            var asm = Assembly.LoadFile(filePath);
            Console.WriteLine("IL → NASM translator");

            var emitters = InstructionEmitterRegistry.Create();
            var context = new EmitterContext();

            foreach (var type in asm.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    if (method.DeclaringType != type)
                    {
                        continue; // Skip methods not declared in this type
                    }
                    Console.WriteLine($"Disassembling {type.FullName}.{method.Name}");

                    if (method.Name == "Main")
                    {
                        // Create a label for the method
                        context.AddLabel("_start");
                    }

                    var instructions = ILReader.ReadInstructions(method);
                    var body = method.GetMethodBody();
                    int localVarCount = body?.LocalVariables.Count ?? 0;
                    int stackSize = localVarCount * 8;
                    // Initialize stack frame
                    context.WriteLine($"push rbp");
                    context.WriteLine($"mov rbp, rsp");
                    context.WriteLine($"sub rsp, {stackSize}"); // Allocate space for local variables

                    foreach (var instr in instructions)
                    {
                        if (method.Name == "Main" && instr.OpCode.Name == "ret")
                        {
                            context.WriteLine("mov rax, 60");
                            context.WriteLine("xor rdi, rdi");
                            context.WriteLine("syscall");
                            continue;
                        }
                        // Label handling (if any jumps point to this offset)
                        if (context.Labels.TryGetValue(instr.Offset, out var label))
                            context.Output.AppendLine($"{label}:");

                        if (emitters.TryGetValue(instr.OpCode, out var emitter))
                        {
                            Debug.Print($"Emitting {instr.OpCode.Name} for {type.FullName}.{method.Name} at offset {instr.Offset:X4}");
                            emitter.Emit(instr, context);
                        }
                        else
                        {
                            Logging.Logging.Log($"No emitter for opcode: {instr.OpCode}", false);
                        }
                    }
                    context.WriteLine("");
                }
            }
            // Dump generated NASM
            if (context.ToString().Length != 0)
            {
                Logging.Logging.Log("Successfully generated NASM code to output.asm", true);
                File.WriteAllText("output.asm", context.ToString());
            }
            else
            {
                Logging.Logging.Log("No instructions generated.", false);
            }
        }
    }
}