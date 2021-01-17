using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp165
{
    class Program
    {
        static void Main(string[] args)
        {
            // find Console.WriteLine(string); in mscorlib 
            var writeline = GetMethod("Console.WriteLine", new Type[] { typeof(string) });

            // Call this function
            writeline.Invoke(null, new object[] { "Hello world" });

            Console.ReadLine();
        }

        static MethodInfo GetMethod(string func, Type[] parameterTypes = null, BindingFlags flags = BindingFlags.Public | BindingFlags.Static)
        {
            foreach (var type in GetExportedMscorlibTypes())
            {
                foreach (var function in type.GetMethods(flags))
                {
                    if (func == function.Name ||
                        func == type.Name + "." + function.Name || 
                        func == type.FullName + "." + function.Name)
                    {
                       // Console.WriteLine(type.Name + "." + function.Name);
                        if(parameterTypes != null)
                        {
                            var parameters = function.GetParameters();
                            if (ValidateParameters(parameters,  parameterTypes ))
                            {
                                return function;
                            }
                            continue;
                        } else
                        {
                            return function;
                        }
                    }
                }
            }
            return default(MethodInfo);
        }

        static bool ValidateParameters(ParameterInfo[] infos, Type[] types)
        {
            if (infos.Length == types.Length)
            {
                for (int i = 0; i < types.Length; i++)
                {
                    if (infos[i].ParameterType!= types[i])
                    {
                        return false;
                    }

                    return true;
                       
                }
                   
            }
            return false;
               
        }

        static Type[] GetExportedMscorlibTypes()
        {
            var mscorlib = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "mscorlib.dll");
            var module = Assembly.LoadFile(mscorlib);
            return module.GetExportedTypes();
        }
    }
}
