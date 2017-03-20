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
        GraphicsDeviceManager graphics;
        ScalingManager scaling;

		SoundManager sound;

        SpriteBatch buffer;
		Tweener tweening;
        Database global;
        Stage stage;

		float oneSecond;
		static int frames;
		static int fps;

        /// <summary>
        /// Initializes a new instance of the Entrypoint class.
        /// </summary>
        public Entrypoint()
        {
            graphics = new GraphicsDeviceManager(this);
			scaling = new ScalingManager(graphics, Window, new Vector3(720, 16, 9), 1.25f);

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
            Scripting.Call(Scripting.Global?.Get("init"));

#if MONOMAC
			scaling.Center();
#endif

            sound = new SoundManager();

            buffer = new SpriteBatch(GraphicsDevice);
			tweening = new Tweener();
            global = new Database("global");
            stage = new Stage(scaling, Content, tweening, sound, global);

            Scripting.Global?.Set("stage", UserData.Create(stage));
            Scripting.Global?.Set("delta", DynValue.NewNumber(0));
            Scripting.Global?.Set("fps", DynValue.NewNumber(0));

            Logger.Info(GetType(), $"Initial window size is '{scaling.Size}'.");
            Logger.Info(GetType(), $"Initial content scale is '{scaling.Scale.X}'.");
            Logger.Info(GetType(), "Ready. Handoff to userdefined code.");

            Scripting.Call(Scripting.Global?.Get("start"));

            GC.Collect();
        } 

        protected override void Update(GameTime gameTime)
        {
			sound.Update();

			tweening.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            stage.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            Scripting.Global?.Set("delta", DynValue.NewNumber(gameTime.ElapsedGameTime.TotalSeconds));
            Scripting.Call(Scripting.Global?.Get("update"));

            if (oneSecond >= 1)
			{
				fps = frames;
				frames = 0;
				oneSecond--;

                Scripting.Global?.Set("fps", DynValue.NewNumber(fps));
            }
			oneSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(stage.Color);

            buffer.Begin();
            stage.Draw(buffer, Matrix.Identity);
            buffer.End();

            base.Draw(gameTime);

			frames++;
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);

            global.Save();
        }

        /// <summary>
        /// Gets the attached GraphicsDeviceManager.
        /// </summary>
        [MoonSharpVisible(true)]
        protected GraphicsDeviceManager Graphics
		{
			get { return graphics; }
		}

        /// <summary>
        /// Gets or sets the attached ScalingManager.
        /// </summary>
        [MoonSharpVisible(true)]
		protected ScalingManager Scaling
		{
			get { return scaling; }
			set { scaling = value; } 
		}

        /// <summary>
        /// Gets the attached SpriteBatch buffer.
        /// </summary>
		protected SpriteBatch Buffer
		{
			get { return buffer; }
		}

        /// <summary>
        /// Gets the attached Stage.
        /// </summary>
		protected Stage Stage
		{
			get { return stage; }
		}

        /// <summary>
        /// Gets the current Frames/s count.
        /// </summary>
        [MoonSharpVisible(false)]
		public static int FramesPerSecond
		{
			get { return fps; }
		}
    }
}
