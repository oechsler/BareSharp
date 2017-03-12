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
                    alloc('Microsoft.Xna.Framework.Content.ContentManager', 'MonoGame.Framework')
                    bare.rotatedRectangle = alloc('Microsoft.Xna.Framework.RotatedRectangle', _DEFAULT)

                    bare.sound = alloc('BareKit.Audio.Sound', _DEFAULT)
                    alloc('BareKit.Audio.SoundManager', _DEFAULT)

                    bare.container = alloc('BareKit.Graphics.Container', _DEFAULT)
                    alloc('BareKit.Graphics.Drawable', _DEFAULT)
                    bare.label = alloc('BareKit.Graphics.Label', _DEFAULT)
                    bare.rect = alloc('BareKit.Graphics.Rect', _DEFAULT)
                    alloc('BareKit.Graphics.ScalingManager', _DEFAULT)
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

        public static string Alloc(string typeName, string assemblyName = null)
        {
            if (assemblyName == null)
                assemblyName = typeName.Replace($".{typeName.Split('.')[typeName.Split('.').Length - 1]}", "");

            string name = $"{typeName}, {assemblyName}";

            if (!UserData.IsTypeRegistered(Type.GetType(name)))
            {
                UserData.RegisterType(Type.GetType(name));
                Print($"Allocated \"{typeName.Split('.')[typeName.Split('.').Length - 1]}\" class.");
            }

            return name;
        }

        public static void Dealloc(string name)
        {
            if (UserData.IsTypeRegistered(Type.GetType(name)))
            {
                UserData.UnregisterType(Type.GetType(name));
                Print($"Deallocated \"{name.Split('.')[name.Split('.').Length - 1].Split(',')[0]}\" class.");
            }
        }

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

        public static DynValue Enum(string name)
        {
            return UserData.CreateStatic(Type.GetType(name));
        }

        public static void Print(string message)
        {
            Logger.Info(typeof(Scripting), $"{(message != null ? message : "nil")}");
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
