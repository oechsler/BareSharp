using System;
#if DEBUG
using System.Diagnostics;
#endif

namespace BareKit
{
    public static class Logger
    {
        public static void Info(Type sender, string message)
        {
            message = $"[{DateTime.Now:HH:mm:ss}][{sender.Name}]: {message}";
#if DEBUG
            Debug.WriteLine(message);
#elif !WINDOWS_UAP
            Console.WriteLine(message);
#endif
        }

        public static void Warn(Type sender, string message)
        {
            message = $"[{DateTime.Now:HH:mm:ss}][{sender.Name.ToUpper()}]: {message}";
#if DEBUG
            Debug.WriteLine(message);
#elif !WINDOWS_UAP
            Console.WriteLine(message);
#endif
        }
    }
}
