using System;
using System.Linq;
using System.Reflection;

namespace TinyIoC
{
    public static class AssemblyExtensions
    {
        public static Type[] SafeGetTypes(this Assembly assembly)
        {
            Type[] assemblies;

            try
            {
                assemblies = assembly.GetTypes();
            }
            catch (System.IO.FileNotFoundException)
            {
                assemblies = new Type[] { };
            }
            catch (NotSupportedException)
            {
                assemblies = new Type[] { };
            }
#if !NETFX_CORE
            catch (ReflectionTypeLoadException e)
            {
                assemblies = e.Types.Where(t => t != null).ToArray();
            }
#endif
            return assemblies;
        }
    }
}