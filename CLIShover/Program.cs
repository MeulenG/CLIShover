using System;
using System.Linq;
using System.Reflection;

namespace CLIShover
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IL → NASM translator");
            
            var asm = Assembly.LoadFile("/mnt/c/Users/Molle/Desktop/CLIShover/CLIShover/bin/Release/net8.0/CLIShover.dll");

            foreach (var type in asm.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    Console.WriteLine($"Disassembling {type.FullName}.{method.Name}");

                    var instructions = ILReader.ReadInstructions(method);
                    foreach (var instr in instructions)
                    {
                        Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
                    }
                }
            }
        }
    }
}
