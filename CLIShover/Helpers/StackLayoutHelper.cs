
namespace CLIShover.Helpers
{
    public static class StackLayoutHelper
    {
        public static int GetLocalOffset(int localIndex)
        {
            // Each int is 4 bytes. Local 0 = [rbp - 4], local 1 = [rbp - 8], etc.
            return -4 * (localIndex + 1);
        }
    }
}
