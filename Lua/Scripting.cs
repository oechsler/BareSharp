using System;
#if WINDOWS_UAP
using System.Diagnostics;
#endif
using System.Reflection;
using System.IO;

namespace MoonSharp.Interpreter
{
    [MoonSharpUserData]
    public static class Scripting
    {
        static string rootDirectory;

        static Script script;
        static object target;
        static string scriptDirectory;

        public static void Initialize(object target, string path)
        {
            if (script == null)
            {
                script = new Script();
                Scripting.target = target;
                scriptDirectory = $"{target.GetType().Namespace}.{rootDirectory}";

                UserData.RegisterAssembly(target.GetType().GetTypeInfo().Assembly);
                UserData.RegisterType<EventArgs>();

                script.Globals.Set("bare", DynValue.NewTable(script));

                script.Globals.Set("loader", UserData.CreateStatic(typeof(Scripting)));
                script.DoString("require = loader.require");
                script.DoString("instance = loader.instance");
                script.DoString("print = loader.print");
                script.Globals.Set("loader", DynValue.Nil);
            }

            if (Require(path) == DynValue.Nil)
            {
                script = null;
                Print("Disabled lua scripting.");
            }
        }

        public static DynValue Require(string path)
        {
            if (script != null)
            {
                string resourceName = path.Split('/')[path.Split('/').Length - 1];
                Stream resourceStream = target.GetType().GetTypeInfo().Assembly.GetManifestResourceStream($"{scriptDirectory}.{path.Replace("/", ".")}.lua");
                if (resourceStream != null)
                {
                    Print($"Required \"{resourceName}\" module.");
                    return script.DoStream(resourceStream);
                }
                Print($"Module \"{resourceName}\" does not exist.");
                return DynValue.Nil;
            }
            throw new Exception("Loader has not been initialized yet.");
        }

        public static DynValue Instance(string typeName, params object[] args)
        {
            object[] convertedArgs = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].GetType() == typeof(double))
                    convertedArgs[i] = (float)(double)args[i];
                else
                    convertedArgs[i] = args[i];
            }

            UserData.RegisterType(Type.GetType(typeName));
            return UserData.Create(Activator.CreateInstance(Type.GetType(typeName), convertedArgs));
        }

        public static void Print(string message)
        {
            message = $"[{DateTime.Now.ToString("HH:mm:ss")}]: {(message != null ? message : "nil")}";
#if WINDOWS_UAP
            Debug.WriteLine(message);
#else
            Console.WriteLine(message);
#endif
        }

        public static string RootDirectory
        {
            get { return rootDirectory; }
            set { rootDirectory = value; }
        }

        public static Script Script
        {
            get { return script; }
        }

        public static Table Global
        {
            get { return script.Globals.Get("bare").Table; }
        }
    }
}
