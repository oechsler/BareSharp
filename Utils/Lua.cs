#if !NOSCRIPT
using System;
using System.IO;
using System.Reflection;

using MoonSharp.Interpreter;

namespace BareKit
{
    [MoonSharpUserData]
    public static class Lua
    {
        static bool initialized;
        static Script environement;

        public static void Initialize()
        {
            if (initialized) return;

            environement = new Script();

            UserData.RegisterAssembly(Assembly.GetExecutingAssembly());
            UserData.RegisterType(typeof(EventArgs));

            Global.Set("_INTERNAL", UserData.CreateStatic(typeof(Lua)));
            DoString(@"
                alloc = _INTERNAL.alloc
                dealloc = _INTERNAL.dealloc
                init = _INTERNAL.init
                static = _INTERNAL.enum
                enum = _INTERNAL.enum
                call = _INTERNAL.call
                doString = _INTERNAL.doString
                require = _INTERNAL.require
                print = _INTERNAL.print
            ", "internal");
            Global.Set("_INTERNAL", DynValue.Nil);

            Global.Set("_DEFAULT", DynValue.NewString(Assembly.GetExecutingAssembly().GetName().Name));
            Global.Set("bare", DynValue.NewTable(environement));


            Require($"{Assembly.GetExecutingAssembly().GetName().Name}.Utils.Scripts.boot", true);
            initialized = true;
        }

        public static Type Alloc(string typeName, string assembly)
        {
            var allocateType = Type.GetType($"{typeName}, {assembly}");
            if (allocateType == null) return null;

            if (UserData.IsTypeRegistered(allocateType)) return allocateType;
            UserData.RegisterType(allocateType);
            Logger.Info(typeof(Lua), $"Allocated '{typeName.Split('.')[typeName.Split('.').Length - 1]}' module.");

            return allocateType;
        }

        public static void Dealloc(Type type)
        {
            if (!UserData.IsTypeRegistered(type)) return;
            UserData.UnregisterType(type);
            Logger.Info(typeof(Lua), $"Deallocated '{type.FullName.Split('.')[type.FullName.Split('.').Length - 1].Split(',')[0]}' module.");
        }

        public static DynValue Init(Type type, params object[] args)
        {
            var convertedArgs = new object[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] is double && Math.Abs((double)args[i] - Math.Floor((double)args[i])) < 0.1)
                    convertedArgs[i] = (int)(double)args[i];
                else if (args[i] is double)
                    convertedArgs[i] = (float)(double)args[i];
                else
                    convertedArgs[i] = args[i];
            }

            return UserData.Create(Activator.CreateInstance(type, convertedArgs));
        }

        public static DynValue Enum(Type type)
        {
            return UserData.CreateStatic(type);
        }

        public static DynValue Call(DynValue function, params DynValue[] args)
        {
            if (!function.IsNotNil()) return DynValue.Nil;
            try
            {
                return function.Function.Call(args);
            }
            catch (InterpreterException e)
            {
                Exeption(e);
            }
            return DynValue.Nil;
        }

        public static DynValue DoString(string instructions, string name = null)
        {
            try
            {
               return environement.DoString(instructions, null, name);
            }
            catch (InterpreterException e)
            {
                Exeption(e);
            }
            return DynValue.Nil;
        }

        public static DynValue Require(string path, bool absolute = false)
        {
            var resourceName = path.Replace("/", ".").Split('.')[path.Replace("/", ".").Split('.').Length - 1];
            var resource = Storage.EmbeddedResource(absolute ? $"{path}.lua" : $"{Assembly.GetExecutingAssembly().GetName().Name}.{RootDirectory}.{path}.lua");
            if (resource == null) return DynValue.Nil;
            try
            {
                return environement.DoStream(resource, null, resourceName);
            }
            catch (InterpreterException e)
            {
                Exeption(e);
            }
            return DynValue.Nil;;
        }

        public static void Print(string message, Type module = null) 
        {
            Logger.Info(module != null ? module : typeof(Lua), $"{message ?? "(nil)"}");
        }

        static void Exeption(InterpreterException e)
        {
            Logger.Warn(typeof(Lua), $"({e.DecoratedMessage.Split(':')[1].Replace("(", "").Replace(")", "")}->'{e.DecoratedMessage.Split(':')[0]}.lua'): {e.Message.Substring(0, 1).ToUpper()}{e.Message.Substring(1)}.");
        }

        public static string RootDirectory { get; set; }

        public static Table Global => environement?.Globals;
    }
}
#endif