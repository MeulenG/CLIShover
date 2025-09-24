using System.Reflection;

namespace CLIShover
{
    public class ClassInfo
    {
        public Type Type { get; }
        public ClassInfo? BaseClass { get; set; }
        public List<MethodInfo> VirtualMethods { get; } = new List<MethodInfo>();
        public Dictionary<MethodInfo, int> VTableSlots { get; } = new Dictionary<MethodInfo, int>();

        public ClassInfo(Type type)
        {
            Type = type;
        }

        public void BuildVTable()
        {
            int slot = 0;
            // Start from base class vtable
            if (BaseClass != null)
            {
                foreach (var kv in BaseClass.VTableSlots)
                {
                    VTableSlots[kv.Key] = kv.Value;
                    slot++;
                }
            }

            // Add/override virtual methods
            foreach (var method in VirtualMethods)
            {
                if (!VTableSlots.ContainsKey(method))
                {
                    VTableSlots[method] = slot++;
                }
            }
        }
    }
}
