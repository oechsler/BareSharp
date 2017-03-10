using System;
#if WINDOWS_UAP
using System.Diagnostics;
#endif
using System.Reflection;
using System.IO;

#if !WINDOWS_UAP
namespace BareKit.Lua
#else
namespace MoonSharp.Interpreter
#endif
{
    [MoonSharpUserData]
    public static class ScriptLoader
    {
        static string rootDirectory;

        static Script script;
        static object target;
        static string scriptDirectory;

        public static void Initialize(Script script, object target, string path)
        {
            if (ScriptLoader.script == null)
            {
                ScriptLoader.script = script;
                ScriptLoader.target = target;
                scriptDirectory = $"{target.GetType().Namespace}.{rootDirectory}";

                script.Globals.Set("bare", DynValue.NewTable(script));

                UserData.RegisterAssembly(target.GetType().GetTypeInfo().Assembly);

                script.Globals.Set("loader", UserData.CreateStatic(typeof(ScriptLoader)));
                script.DoString("require = loader.require");
                script.DoString("print = loader.print");
                script.Globals.Set("loader", DynValue.Nil);
            }

            Require(path);
        }

        public static DynValue Require(string path)
        {
            string resourceName = path.Split('/')[path.Split('/').Length -1];
            Stream resourceStream = target.GetType().GetTypeInfo().Assembly.GetManifestResourceStream($"{scriptDirectory}.{path.Replace("/", ".")}.lua");
            if (resourceStream != null)
            {
                Print($"Load module: {resourceName}");
                return script.DoStream(resourceStream);
            }
            return DynValue.Nil;
        }

        public static void Print(string message)
        {
            message = $"[{DateTime.Now.ToString("HH:mm:ss")}]: {message}";
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
    }
}
