
namespace IncroCompiler.Helpers
{
    public static class StackLayoutHelper
    {
        public static int GetLocalOffset(int localIndex)
        {
            return -8 * (localIndex + 1); // 8-byte slots on x64
        }
    }
}
