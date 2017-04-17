using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

using BareKit.Audio;
using BareKit.Graphics;
using BareKit.Tweening;

namespace BareKit
{
    [MoonSharpUserData]
    public class Entrypoint : Game
    {
        Tweener tweening;
        SoundManager sound;
        Database global;

        float oneSecond;
		static int frames;

        /// <summary>
        /// Initializes a new instance of the Entrypoint class.
        /// </summary>
        public Entrypoint()
        {
            Graphics = new GraphicsDeviceManager(this);
			Scaling = new ScalingManager(Graphics, Window, new Vector3(720, 16, 9), 1.25f);

            Content.RootDirectory = "Content";
            Lua.RootDirectory = "Scripts";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Storage.Initalize();

            Logger.Info(GetType(), $"Content will be loaded from '{Content.RootDirectory}'.");
            Logger.Info(GetType(), $"Scripts will be loaded from '{Lua.RootDirectory}'.");

#if !NOSCRIPT
            Lua.Initialize();
            Lua.Global?.Get("bare").Table?.Set("entrypoint", UserData.Create(this));
            Lua.Call(Lua.Global?.Get("bare").Table?.Get("config"), UserData.Create(this));
#endif

#if MONOMAC
			scaling.Center();
#endif

            Buffer = new SpriteBatch(GraphicsDevice);
			tweening = new Tweener();
            sound = new SoundManager();
            global = new Database("global");
            Stage = new Stage(Scaling, Content, tweening, sound, global);

#if !NOSCRIPT
            Lua.Global?.Get("bare").Table?.Set("stage", UserData.Create(Stage));
            Lua.Global?.Get("bare").Table?.Set("delta", DynValue.NewNumber(0));
            Lua.Global?.Get("bare").Table?.Set("fps", DynValue.NewNumber(0));
#endif

            Logger.Info(GetType(), $"Vertical synchronisation activated '{Graphics.SynchronizeWithVerticalRetrace}'.");
            Logger.Info(GetType(), $"Framstep is set to '{TargetElapsedTime.TotalMilliseconds} ms'.");
            Logger.Info(GetType(), $"Initial window size is '{Scaling.Size.ToString().Replace("{X:", "").Replace(" Y:", "x").Replace("}", "")}'.");
            Logger.Info(GetType(), $"Initial content scale is 'x{Scaling.Scale.X}'.");
            Logger.Info(GetType(), "Ready. Handoff to userdefined code.");

#if !NOSCRIPT
            Lua.Call(Lua.Global?.Get("bare").Table?.Get("start"));
            GC.Collect();
#endif

        } 

        protected override void Update(GameTime gameTime)
        {
			tweening.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            sound.Update();
            Stage.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

#if !NOSCRIPT
            Lua.Global?.Get("bare").Table?.Set("delta", DynValue.NewNumber(gameTime.ElapsedGameTime.TotalSeconds));
            Lua.Call(Lua.Global?.Get("bare").Table?.Get("update"));
#endif

            if (oneSecond >= 1)
			{
				FramesPerSecond = frames;
				frames = 0;
				oneSecond--;

#if !NOSCRIPT
			    Lua.Global?.Get("bare").Table?.Set("fps", DynValue.NewNumber(FramesPerSecond));
#endif
            }
			oneSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Stage.Color);

            Buffer.Begin();
            Stage.Draw(Buffer, Matrix.Identity);
            Buffer.End();

            base.Draw(gameTime);

			frames++;
        }


        protected override void OnExiting(object sender, EventArgs args)
        {
            global.Save();

            base.OnExiting(sender, args);
        }

#if !WINDOWS && !MONOMAC && !LINUX 
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            OnExiting(sender, args);

            base.OnDeactivated(sender, args);
        }
#endif

        /// <summary>
        /// Gets the attached GraphicsDeviceManager.
        /// </summary>
        [MoonSharpVisible(true)]
        protected GraphicsDeviceManager Graphics { get; }

        /// <summary>
        /// Gets or sets the attached ScalingManager.
        /// </summary>
        [MoonSharpVisible(true)]
		protected ScalingManager Scaling { get; set; }

        /// <summary>
        /// Gets the attached SpriteBatch buffer.
        /// </summary>
		protected SpriteBatch Buffer { get; set; }

        /// <summary>
        /// Gets the attached Stage.
        /// </summary>
		protected Stage Stage { get; set; }

        /// <summary>
        /// Gets the current Frames/s count.
        /// </summary>
        [MoonSharpVisible(false)]
		public static int FramesPerSecond { get; set; }
    }
}
