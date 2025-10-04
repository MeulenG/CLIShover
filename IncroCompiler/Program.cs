namespace IncroCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecutionEngine engine = new ExecutionEngine();
            // Test File
            engine.Execute("/mnt/c/Users/Molle/Desktop/IncroCompiler/IncroCompiler/Test-Data/HelloWorldOS.dll");
        }

    }
}
