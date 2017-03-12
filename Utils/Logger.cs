using System;
#if WINDOWS_UAP
using System.Diagnostics;
#endif

namespace BareKit
{
    public static class Logger
    {
        public static void Info(Type sender, string message)
        {
            message = $"[{DateTime.Now.ToString("HH:mm:ss")}][{sender.Name}]> {message}";
#if WINDOWS_UAP
            Debug.WriteLine(message);
#else
            Console.WriteLine(message);
#endif
        }

        public static void Warn(Type sender, string message)
        {
            message = $"[{DateTime.Now.ToString("HH:mm:ss")}][{sender.Name.ToUpper()}]> {message}";
#if WINDOWS_UAP
            Debug.WriteLine(message);
#else
            Console.WriteLine(message);
#endif
        }
    }
}
