using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

using BareKit.Audio;
using BareKit.Graphics;
using BareKit.Tweening;
using BareKit.Lua;

namespace BareKit
{
    [MoonSharpUserData]
    public class Entrypoint : Game
    {
        SoundManager sound;

        Tweener tweening;
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
            Scripting.RootDirectory = "Scripts";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Logger.Info(GetType(), $"Content will be loaded from '{Content.RootDirectory}'.");
            Logger.Info(GetType(), $"Scripts will be loaded from '{Scripting.RootDirectory}'.");

            Scripting.Initialize(this, "main");
            Scripting.Global?.Set("entrypoint", UserData.Create(this));
            Scripting.Call(Scripting.Global?.Get("config"), UserData.Create(this));

#if MONOMAC
			scaling.Center();
#endif

            sound = new SoundManager();

            Buffer = new SpriteBatch(GraphicsDevice);
			tweening = new Tweener();
            global = new Database("global");
            Stage = new Stage(Scaling, Content, tweening, sound, global);

            Scripting.Global?.Set("stage", UserData.Create(Stage));
            Scripting.Global?.Set("delta", DynValue.NewNumber(0));
            Scripting.Global?.Set("fps", DynValue.NewNumber(0));

            Logger.Info(GetType(), $"Vertical synchronisation activated '{Graphics.SynchronizeWithVerticalRetrace}'.");
            Logger.Info(GetType(), $"Framstep is set to '{TargetElapsedTime.TotalMilliseconds} ms'.");
            Logger.Info(GetType(), $"Initial window size is '{Scaling.Size.ToString().Replace("{X:", "").Replace(" Y:", "x").Replace("}", "")}'.");
            Logger.Info(GetType(), $"Initial content scale is 'x{Scaling.Scale.X}'.");
            Logger.Info(GetType(), "Ready. Handoff to userdefined code.");

            Scripting.Call(Scripting.Global?.Get("start"));

            GC.Collect();
        } 

        protected override void Update(GameTime gameTime)
        {
			sound.Update();

			tweening.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            Stage.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            Scripting.Global?.Set("delta", DynValue.NewNumber(gameTime.ElapsedGameTime.TotalSeconds));
            Scripting.Call(Scripting.Global?.Get("update"));

            if (oneSecond >= 1)
			{
				FramesPerSecond = frames;
				frames = 0;
				oneSecond--;

                Scripting.Global?.Set("fps", DynValue.NewNumber(FramesPerSecond));
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
