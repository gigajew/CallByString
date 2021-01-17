# CallByString 

Call a C# function by string by searching in mscorlib for the right method.
```c#
            // find Console.WriteLine(string); in mscorlib 
            var writeline = GetMethod("Console.WriteLine", new Type[] { typeof(string) });
            // Call this function
            writeline.Invoke(null, new object[] { "Hello world" });
```
