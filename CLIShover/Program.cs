﻿namespace CLIShover
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecutionEngine engine = new ExecutionEngine();
            // Test File
            engine.Execute("/mnt/c/Users/Molle/Desktop/CLIShover/CLIShover/Test-Data/HelloWorldOS.dll");
        }

    }
}
