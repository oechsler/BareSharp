using System;
#if DEBUG
using System.Diagnostics;
#endif

namespace BareKit
{
    public static class Logger
    {
        /// <summary>
        /// Outputs a info message to the standard output.
        /// </summary>
        /// <param name="message">The message to output.</param>
        /// <param name="sender">The type of the class the message originates from.</param>
        public static void Info(object message, Type sender = null)
        {
            if (sender == null)
                sender = typeof(Logger);
            message = $"[{DateTime.Now:HH:mm:ss}][{sender.Name}]: {message}";
#if DEBUG
            Debug.WriteLine(message);
#endif
#if !WINDOWS_UAP
            Console.WriteLine(message);
#endif
        }

        /// <summary>
        /// Outputs a warning message to the standard output.
        /// </summary>
        /// <param name="message">The message to output.</param>
        /// <param name="sender">The type of the class the message originates from.</param>
        public static void Warn(object message, Type sender = null)
        {
            if (sender == null)
                sender = typeof(Logger);
            message = $"[{DateTime.Now:HH:mm:ss}][{sender.Name.ToUpper()}]: {message}";
#if DEBUG
            Debug.WriteLine(message);
#endif
#if !WINDOWS_UAP
            Console.WriteLine(message);
#endif
        }
    }
}
