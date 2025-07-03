using CLIShover;

namespace Unit_Test
{
    public class AddTests
    {
        [Fact]
        public void Test1()
        {
            string sourceCode = @"
            public class Test
            {
                public int Add(int a, int b)
                {
                    return a + b;
                }
            }";

            var compParms = new CompilerParameters{
                GenerateExecutable = false, 
                GenerateInMemory = true
            };

            var csProvider = new CSharpCodeProvider();
            CompilerResults compilerResults = 
                    csProvider.CompileAssemblyFromSource(compParms, sourceCode);
            object typeInstance = 
                    compilerResults.CompiledAssembly.CreateInstance("Test");
            MethodInfo mi = typeInstance.GetType().GetMethod("Hello");
            mi.Invoke(typeInstance, null); 
            Console.ReadLine();



            CLIShover.ExecutionEngine engine = new CLIShover.ExecutionEngine();

        }
    }
}