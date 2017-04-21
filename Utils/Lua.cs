#if !NOSCRIPT
using System;
using System.Reflection;

using MoonSharp.Interpreter;

namespace BareKit
{
    [MoonSharpUserData]
    public static class Lua
    {
        static bool initialized;
        static Script environement;

        /// <summary>
        /// Initalizes the Lua environment.
        /// </summary>
        public static void Initialize()
        {
            if (initialized) return;

            const CoreModules core = 
                CoreModules.Basic          |                          
                CoreModules.Coroutine      |                    
                CoreModules.ErrorHandling  |                                     
                CoreModules.GlobalConsts   |                                     
                CoreModules.LoadMethods    |                                     
                CoreModules.Math           |                                    
                CoreModules.Metatables     |                                     
                CoreModules.OS_Time        |                                      
                CoreModules.String         |                      
                CoreModules.Table          | 
                CoreModules.TableIterators;
            environement = new Script(core);

            UserData.RegisterAssembly(Assembly.GetExecutingAssembly());
            UserData.RegisterType(typeof(EventArgs));

            Global.Set("_INTERNAL", UserData.CreateStatic(typeof(Lua)));
            DoString(@"
                alloc = _INTERNAL.alloc
                dealloc = _INTERNAL.dealloc
                init = _INTERNAL.init
                static = _INTERNAL.static
                enum = _INTERNAL.static
                call = _INTERNAL.call
                dostring = _INTERNAL.doString
                require = _INTERNAL.require
                print = _INTERNAL.print
            ", "internal");
            Global.Set("_INTERNAL", DynValue.Nil);

            Global.Set("_DEFAULT", DynValue.NewString(Assembly.GetExecutingAssembly().GetName().Name));
            Global.Set("bare", DynValue.NewTable(environement));

            Require($"{Assembly.GetExecutingAssembly().GetName().Name}.Utils.Scripts.boot", true);
            initialized = true;
        }

        /// <summary>
        /// Allocates types to the lua environment.
        /// </summary>
        /// <param name="typeName">Absolute path to the types class.</param>
        /// <param name="assembly">Name of the assembly containing the type.</param>
        public static Type Alloc(string typeName, string assembly)
        {
            var allocateType = Type.GetType($"{typeName}, {assembly}");
            if (allocateType == null) return null;

            if (UserData.IsTypeRegistered(allocateType)) return allocateType;
            UserData.RegisterType(allocateType);
            Logger.Info($"Allocated '{typeName.Split('.')[typeName.Split('.').Length - 1]}' module.", typeof(Lua));

            return allocateType;
        }

        /// <summary>
        /// Deallocates types from the lua environment.
        /// </summary>
        /// <param name="type">The type to deallocate.</param>
        public static void Dealloc(Type type)
        {
            if (!UserData.IsTypeRegistered(type)) return;
            UserData.UnregisterType(type);
            Logger.Info($"Deallocated '{type.FullName.Split('.')[type.FullName.Split('.').Length - 1].Split(',')[0]}' module.", typeof(Lua));
        }

        /// <summary>
        /// Instanciates objects as UserData in the lua environment.
        /// </summary>
        /// <param name="type">The type to instanciate.</param>
        /// <param name="args">Constructor arguments.</param>
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

        /// <summary>
        /// References class methods as UserData in the lua environment.
        /// </summary>
        /// <param name="type">The type to instanciate.</param>
        /// <returns></returns>
        public static DynValue Static(Type type)
        {
            return UserData.CreateStatic(type);
        }

        /// <summary>
        /// Savecalls functions in the lua environment.
        /// </summary>
        /// <param name="function">The function to call.</param>
        /// <param name="args">Arguments passed to the specefied function.</param>
        /// <returns></returns>
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
            catch (Exception e)
            {
                Logger.Warn(e.Message, typeof(Lua));
            }
            return DynValue.Nil;
        }

        /// <summary>
        /// Executes a string of code in the lua evironment.
        /// </summary>
        /// <param name="instructions">The code to execute.</param>
        /// <param name="name">A code friendly name for easy discovery of errors.</param>
        /// <returns></returns>
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
            catch (Exception e)
            {
                Logger.Warn(e.Message, typeof(Lua));
            }
            return DynValue.Nil;
        }

        /// <summary>
        /// Requires a seperate script to the lua environment.
        /// </summary>
        /// <param name="path">The path to the script relative to the root directory.</param>
        /// <param name="absolute">Whether the path should be absolute.</param>
        /// <returns></returns>
        public static DynValue Require(string path, bool absolute = false)
        {
            var resourceName = path.Replace("/", ".").Split('.')[path.Replace("/", ".").Split('.').Length - 1];
            var resource = Storage.EmbeddedResource(
                                absolute ? 
                                $"{path.Replace(".lua", "")}.lua" : 
                                $"{Assembly.GetExecutingAssembly().GetName().Name}.{RootDirectory}.{path.Replace(".lua", "")}.lua"
                           );
            if (resource == null) return DynValue.Nil;
            try
            {
                return environement.DoStream(resource, null, resourceName);
            }
            catch (InterpreterException e)
            {
                Exeption(e);
            }
            catch (Exception e)
            {
                Logger.Warn(e.Message, typeof(Lua));
            }
            return DynValue.Nil;;
        }

        /// <summary>
        /// Prints a message using the standard output.
        /// </summary>
        /// <param name="message">The message to print</param>
        /// <param name="module">The module the message is called in.</param>
        public static void Print(string message, Type module = null) 
        {
            Logger.Info($"{message ?? "(nil)"}", module != null ? module : typeof(Lua));
        }

        static void Exeption(InterpreterException e)
        {
            Logger.Warn($"({e.DecoratedMessage.Split(':')[1].Replace("(", "").Replace(")", "")}->'{e.DecoratedMessage.Split(':')[0]}.lua'): {e.Message.Substring(0, 1).ToUpper()}{e.Message.Substring(1)}.", typeof(Lua));
        }

        /// <summary>
        /// Gets or sets the scripts root directory.
        /// </summary>
        public static string RootDirectory { get; set; }

        /// <summary>
        /// Gets the global table of the lua enviroenment.
        /// </summary>
        public static Table Global => environement?.Globals;
    }
}
#endif