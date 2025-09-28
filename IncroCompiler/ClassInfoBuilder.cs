using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IncroCompiler
{
    public static class ClassInfoBuilder
    {
        public static Dictionary<Type, ClassInfo> Build(Assembly asm)
        {
            var dict = new Dictionary<Type, ClassInfo>();

            var allTypes = asm.GetTypes();
            foreach (var t in allTypes)
            {
                dict[t] = new ClassInfo(t);
            }

            foreach (var ci in dict.Values)
            {
                var baseType = ci.Type.BaseType;
                if (baseType != null && dict.TryGetValue(baseType, out var baseCi))
                    ci.BaseClass = baseCi;
            }

            foreach (var kv in dict.ToArray())
            {
                var t = kv.Key;
                var ci = kv.Value;

                var declaredMethods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                foreach (var m in declaredMethods)
                {
                    if (m.IsVirtual && !m.IsStatic)
                        ci.VirtualMethods.Add(m);
                }
            }

            var ordered = dict.Values.OrderBy(c => GetInheritanceDepth(c.Type)).ToList();
            foreach (var ci in ordered)
                ci.BuildVTable();

            return dict;
        }

        private static int GetInheritanceDepth(Type t)
        {
            int depth = 0;
            var curr = t;
            while (curr.BaseType != null)
            {
                depth++;
                curr = curr.BaseType;
            }
            return depth;
        }
    }
}
