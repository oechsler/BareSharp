﻿using System;
#if WINDOWS_UAP
using System.Diagnostics;
#endif
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

        public static void Initialize(object target, string path)
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

                script.Globals.Set("_default", DynValue.NewString(target.GetType().GetTypeInfo().Assembly.GetName().Name));
                script.Globals.Set("bare", DynValue.NewTable(script));
                script.DoString($@"
                    bare.vector = alloc('Microsoft.Xna.Framework.Vector2', 'MonoGame.Framework')
                    bare.color = alloc('Microsoft.Xna.Framework.Color', 'MonoGame.Framework')
                    alloc('Microsoft.Xna.Framework.Content.ContentManager', 'MonoGame.Framework')
                    bare.rrotatedRectangle = alloc('Microsoft.Xna.Framework.RotatedRectangle', _default)

                    bare.sound = alloc('BareKit.Audio.Sound', _default)
                    alloc('BareKit.Audio.SoundManager', _default)

                    bare.container = alloc('BareKit.Graphics.Container', _default)
                    alloc('BareKit.Graphics.Drawable', _default)
                    bare.label = alloc('BareKit.Graphics.Label', _default)
                    bare.rect = alloc('BareKit.Graphics.Rect', _default)
                    alloc('BareKit.Graphics.ScalingManager', _default)
                    bare.scene = alloc('BareKit.Graphics.Scene', _default)
                    bare.sprite = alloc('BareKit.Graphics.Sprite', _default)

                    bare.gamepadInput = alloc('BareKit.Input.GamepadInput', _default)
                    bare.buttons = alloc('Microsoft.Xna.Framework.Input.Buttons', 'MonoGame.Framework')
                    alloc('BareKit.Input.Input', _default)
                    bare.inputState = alloc('BareKit.Input.InputState', _default)
                    alloc('BareKit.Input.InputManager', _default)
                    bare.keyInput = alloc('BareKit.Input.KeyInput', _default)
                    bare.keys = alloc('Microsoft.Xna.Framework.Input.Keys', 'MonoGame.Framework')
                    bare.touchInput = alloc('BareKit.Input.TouchInput', _default)
                    bare.finger = enum(alloc('BareKit.Input.Finger', _default))
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

        public static void Dealloc(string typeName, string assemblyName = null)
        {
            if (assemblyName == null)
                assemblyName = typeName.Replace($".{typeName.Split('.')[typeName.Split('.').Length - 1]}", "");

            string name = $"{typeName}, {assemblyName}";

            if (UserData.IsTypeRegistered(Type.GetType(name)))
            {
                UserData.UnregisterType(Type.GetType(name));
                Print($"Deallocated \"{typeName.Split('.')[typeName.Split('.').Length - 1]}\" class.");
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