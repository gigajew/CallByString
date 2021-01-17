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
            CreateATextFile();
            PrintToConsole("Whats up doc");

            Console.Read();
        }

        static void PrintToConsole(string text)
        {
            // Find Console.WriteLine(string); in mscorlib 
            var writeline = GetMethod("Console.WriteLine", BindingFlags.Public | BindingFlags.Static, typeof(string));

            // Call this function
            writeline.Invoke(null, new object[] { text });
        }

        static void CreateATextFile()
        {
            // Find the StreamWriter type
            var streamWriterType = GetExportedMscorlibTypes().Where(item => item.FullName.Contains("StreamWriter")).First();

            // Create a StreamWriter instance with .ctor parameter as a string (to create a file)
            var streamWriterInstance = Activator.CreateInstance(streamWriterType, "MyFile.txt");

            // Get WriteLine method
            var writeLineMethod = GetMethod("System.IO.StreamWriter.WriteLine", BindingFlags.Public | BindingFlags.Instance, typeof(string));

            // Get Flush method
            var flushMethod = GetMethod("System.IO.StreamWriter.Flush", BindingFlags.Public | BindingFlags.Instance);

            // call StreamWriter.WriteLine(string);
            writeLineMethod.Invoke(streamWriterInstance, new object[] { "Hello world" });

            // call StreamWriter.Flush();
            flushMethod.Invoke(streamWriterInstance, null);
        }

        static MethodInfo GetMethod(string func, BindingFlags flags = BindingFlags.Public | BindingFlags.Static, params Type[] parameterTypes)
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
                        if (parameterTypes.Length > 0)
                        {
                            var parameters = function.GetParameters();
                            if (ValidateParameters(parameters, parameterTypes))
                            {
                                return function;
                            }
                            continue;
                        }
                        else
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
                    if (infos[i].ParameterType != types[i])
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
