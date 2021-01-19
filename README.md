# CallByString 

Call a C# function by string by searching in mscorlib for the right method.
```c#
            // find Console.WriteLine(string); in mscorlib 
            var writeline = GetMethod("Console.WriteLine", typeof(string));
            // Call this function
            writeline.Invoke(null, new object[] { "Hello world" });
```

Or Create a text file using System.IO.StreamWriter
```c#
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
  ```

# Donate with Bitcoin
Donations: bitcoin:12FP1JisjYCsgfteTLMQQMLnVBs65wZD8G
