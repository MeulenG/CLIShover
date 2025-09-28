using System.Reflection;

namespace IncroCompiler
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
            int nextSlot = 0;
            if (BaseClass != null)
            {
                foreach (var kv in BaseClass.VTableSlots)
                {
                    VTableSlots[kv.Key] = kv.Value;
                    nextSlot = Math.Max(nextSlot, kv.Value + 1);
                }
            }

            foreach (var method in VirtualMethods)
            {
                var baseDef = method.GetBaseDefinition();

                if (BaseClass != null && BaseClass.VTableSlots.TryGetValue(baseDef, out var inheritedSlot))
                {
                    VTableSlots[method] = inheritedSlot;
                }
                else
                {
                    if (!VTableSlots.ContainsKey(method))
                    {
                        VTableSlots[method] = nextSlot++;
                    }
                }
            }
        }
    }
}
