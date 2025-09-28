using System.Reflection.Emit;

public class Logging
{
    // DONE or FAILED using VT100, Format will be [ DONE ] or [ FAILED ]
    public static void Log(string message, bool success)
    {
        var status = success ? " DONE " : " FAILED ";
        switch (success)
        {
            case true:
                Console.WriteLine($"[\x1b[32m{status}\x1b[0m] {message}"); // Green
                break;
            case false:
                Console.WriteLine($"[\x1b[31m{status}\x1b[0m] {message}"); // Red
                break;
        }
    }
}
