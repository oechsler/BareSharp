using System;
using System.Reflection;
using System.IO;

using MoonSharp.Interpreter;

namespace BareKit.Lua
{
    [MoonSharpUserData]
    public static class Scripting
    {
        static string rootDirectory;

        static Script script;
        static object target;
        static string scriptDirectory;

        /// <summary>
        /// Initializes the Lua scripting interpreter.
        /// </summary>
        /// <param name="target">Object of the assembly the interpreter is executing on.</param>
        /// <param name="path">The relative path to the main script.</param>
        public static void Initialize(Entrypoint target, string path)
        {
            if (script == null)
            {
                script = new Script();
                Scripting.target = target;
                scriptDirectory = $"{target.GetType().Namespace}.{rootDirectory}";

                UserData.RegisterAssembly(target.GetType().GetTypeInfo().Assembly);
                UserData.RegisterType<EventArgs>();

                script.Globals.Set("loader", UserData.CreateStatic(typeof(Scripting)));
                script.DoString(@"
                    require = loader.require
                    alloc = loader.alloc
                    dealloc = loader.dealloc
                    init = loader.init
                    enum = loader.enum
                    print = loader.print
                ");
                script.Globals.Set("loader", DynValue.Nil);

                script.Globals.Set("_DEFAULT", DynValue.NewString(target.GetType().GetTypeInfo().Assembly.GetName().Name));
                script.Globals.Set("bare", DynValue.NewTable(script));
                script.DoString($@"
                    alloc('Microsoft.Xna.Framework.Game', 'MonoGame.Framework')
                    alloc('Microsoft.Xna.Framework.GraphicsDeviceManager', 'MonoGame.Framework')
                    alloc('Microsoft.Xna.Framework.GameWindow', 'MonoGame.Framework')
                    bare.vector2 = alloc('Microsoft.Xna.Framework.Vector2', 'MonoGame.Framework')
                    bare.vector3 = alloc('Microsoft.Xna.Framework.Vector3', 'MonoGame.Framework')
                    bare.color = alloc('Microsoft.Xna.Framework.Color', 'MonoGame.Framework')
                    bare.blendMode = alloc('Microsoft.Xna.Framework.Graphics.BlendState', 'MonoGame.Framework')
                    alloc('Microsoft.Xna.Framework.Graphics.Effect', 'MonoGame.Framework')
                    alloc('Microsoft.Xna.Framework.Content.ContentManager', 'MonoGame.Framework')
                    bare.rotatedRectangle = alloc('Microsoft.Xna.Framework.RotatedRectangle', _DEFAULT)

                    bare.sound = alloc('BareKit.Audio.Sound', _DEFAULT)
                    alloc('BareKit.Audio.SoundManager', _DEFAULT)

                    bare.container = alloc('BareKit.Graphics.Container', _DEFAULT)
                    alloc('BareKit.Graphics.Drawable', _DEFAULT)
                    bare.label = alloc('BareKit.Graphics.Label', _DEFAULT)
                    bare.rect = alloc('BareKit.Graphics.Rect', _DEFAULT)
                    alloc('BareKit.Graphics.ScalingManager', _DEFAULT)
                    bare.shader = alloc('BareKit.Graphics.Shader', _DEFAULT)
                    bare.scene = alloc('BareKit.Graphics.Scene', _DEFAULT)
                    bare.sprite = alloc('BareKit.Graphics.Sprite', _DEFAULT)

                    bare.gamepadInput = alloc('BareKit.Input.GamepadInput', _DEFAULT)
                    bare.buttons = alloc('Microsoft.Xna.Framework.Input.Buttons', 'MonoGame.Framework')
                    alloc('BareKit.Input.Input', _DEFAULT)
                    bare.inputState = alloc('BareKit.Input.InputState', _DEFAULT)
                    alloc('BareKit.Input.InputManager', _DEFAULT)
                    bare.keyInput = alloc('BareKit.Input.KeyInput', _DEFAULT)
                    bare.keys = alloc('Microsoft.Xna.Framework.Input.Keys', 'MonoGame.Framework')
                    bare.touchInput = alloc('BareKit.Input.TouchInput', _DEFAULT)
                    bare.finger = enum(alloc('BareKit.Input.Finger', _DEFAULT))
                ");
            }

            if (Require(path) != DynValue.Nil)
                Logger.Info(typeof(Scripting), "Enabled Lua scripting.");
        }

        /// <summary>
        /// Loads a script file into memory.
        /// </summary>
        /// <param name="path">The relative path to the script.</param>
        public static DynValue Require(string path)
        {
            if (script != null)
            {
                string resourceName = path.Split('/')[path.Split('/').Length - 1];
                Stream resourceStream = target.GetType().GetTypeInfo().Assembly.GetManifestResourceStream($"{scriptDirectory}.{path.Replace("/", ".")}.lua");
                if (resourceStream != null)
                {
                    try
                    {
                        Logger.Info(typeof(Scripting), $"Required '{resourceName}' module.");
                        return script.DoStream(resourceStream);
                    }
                    catch (SyntaxErrorException ex)
                    {
                        script = null;
                        Logger.Warn(typeof(Scripting), $"{ex.DecoratedMessage.Split(':')[1]} {ex.Message.Substring(0, 1).ToUpper()}{ex.Message.Substring(1)}.");
                        Logger.Warn(typeof(Scripting), "Disabled Lua scripting.");
                    }
                }
                Logger.Warn(typeof(Scripting), $"Module '{resourceName}' does not exist.");
                return DynValue.Nil;
            }
            throw new Exception("Loader has not been initialized yet.");
        }

        /// <summary>
        /// Allocates a class type for to use with the interpeter.
        /// </summary>
        /// <param name="typeName">The class path of the type.</param>
        /// <param name="assemblyName">The assembly the class is part of.</param>
        public static string Alloc(string typeName, string assemblyName = null)
        {
            if (assemblyName == null)
                assemblyName = typeName.Replace($".{typeName.Split('.')[typeName.Split('.').Length - 1]}", "");

            string name = $"{typeName}, {assemblyName}";

            if (!UserData.IsTypeRegistered(Type.GetType(name)))
            {
                UserData.RegisterType(Type.GetType(name));
                Logger.Info(typeof(Scripting), $"Allocated '{typeName.Split('.')[typeName.Split('.').Length - 1]}' class.");
            }

            return name;
        }

        /// <summary>
        /// Deallocates a class type.
        /// </summary>
        /// <param name="name">The class types definition name.</param>
        public static void Dealloc(string name)
        {
            if (UserData.IsTypeRegistered(Type.GetType(name)))
            {
                UserData.UnregisterType(Type.GetType(name));
                Logger.Info(typeof(Scripting), $"Deallocated '{name.Split('.')[name.Split('.').Length - 1].Split(',')[0]}' class.");
            }
        }

        /// <summary>
        /// Instanciates a object by its class types definition name.
        /// </summary>
        /// <param name="name">The class types definition name.</param>
        /// <param name="args">Constructor arguments of the specified class</param>
        public static DynValue Init(string name, params object[] args)
        {
            object[] convertedArgs = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].GetType() == typeof(double) && ((double)args[i] - Math.Floor((double)args[i])) == 0)
                    convertedArgs[i] = (int)(double)args[i];
                else if (args[i].GetType() == typeof(double))
                    convertedArgs[i] = (float)(double)args[i];
                else
                    convertedArgs[i] = args[i];
            }

            return UserData.Create(Activator.CreateInstance(Type.GetType(name), convertedArgs));
        }

        /// <summary>
        /// Staticly creates a object by its class types definition name.
        /// </summary>
        /// <param name="name">The class types definition name.</param>
        public static DynValue Enum(string name)
        {
            return UserData.CreateStatic(Type.GetType(name));
        }

        /// <summary>
        /// Safly executes a function withing Lua.
        /// </summary>
        /// <param name="function">The functions definition point.</param>
        /// <param name="args">The specified functions arguments.</param>
        public static DynValue Call(DynValue function, params DynValue[] args)
        {
            if (Global != null && function.IsNotNil())
            {
                try
                {
                    return function.Function.Call(args);
                }
                catch (ScriptRuntimeException ex)
                {
                    script = null;
                    Logger.Warn(typeof(Scripting), $"{ex.DecoratedMessage.Split(':')[1]} {ex.Message.Substring(0, 1).ToUpper()}{ex.Message.Substring(1)}.");
                    Logger.Warn(typeof(Scripting), "Disabled Lua scripting.");
                }
            }
            return DynValue.NewNil();
        }

        /// <summary>
        /// Prints out a specified message to the console.
        /// </summary>
        /// <param name="message">The message to print.</param>
        public static void Print(string message)
        {
            Logger.Info(typeof(Scripting), $"{(message != null ? message : "(nil)")}");
        }

        /// <summary>
        /// Gets or sets the default search directory for scripts.
        /// </summary>
        public static string RootDirectory
        {
            get { return rootDirectory; }
            set { rootDirectory = value; }
        }

        /// <summary>
        /// Gets a reference to the interpreter.
        /// </summary>
        public static Script Script
        {
            get { return script; }
        }

        /// <summary>
        /// Gets a reference to the standard librarys table.
        /// </summary>
        public static Table Global
        {
            get {return script?.Globals.Get("bare").Table; }
        }
    }
}
