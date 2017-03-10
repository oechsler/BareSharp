using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MoonSharp.Interpreter;

using BareKit.Audio;
using BareKit.Graphics;
using BareKit.Tweening;
using BareKit.Lua;

namespace BareKit
{
    public class Entrypoint : Game
    {
        GraphicsDeviceManager graphics;
        ScalingManager scaling;

		SoundManager sound;

        SpriteBatch buffer;
		Tweener tweening;
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

#if MONOMAC
			scaling.Center();
#endif

			sound = new SoundManager();

            buffer = new SpriteBatch(GraphicsDevice);
			tweening = new Tweener();
			stage = new Stage(scaling, Content, tweening, sound);
            
            Scripting.Initialize(this, "main");
            Scripting.Global.Set("stage", UserData.Create(stage));

            Scripting.Global.Set("delta", DynValue.NewNumber(0));
            Scripting.Global.Set("fps", DynValue.NewNumber(0));

            if (Scripting.Global != null && Scripting.Global.Get("start").IsNotNil())
                Scripting.Global.Get("start").Function.Call();
        }

        protected override void Update(GameTime gameTime)
        {
			sound.Update();

			tweening.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            stage.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            Scripting.Global.Set("delta", DynValue.NewNumber(gameTime.ElapsedGameTime.TotalSeconds));
            if (Scripting.Global != null && Scripting.Global.Get("update").IsNotNil())
                Scripting.Global.Get("update").Function.Call(gameTime.ElapsedGameTime.TotalSeconds);

            if (oneSecond >= 1)
			{
				fps = frames;
				frames = 0;
				oneSecond--;

                Scripting.Global.Set("fps", DynValue.NewNumber(fps));
            }
			oneSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(stage.Color);

            buffer.Begin();
            stage.Draw(buffer);
            buffer.End();

            base.Draw(gameTime);

			frames++;
        }

        /// <summary>
        /// Gets the attached GraphicsDeviceManager.
        /// </summary>
		protected GraphicsDeviceManager Graphics
		{
			get { return graphics; }
		}

        /// <summary>
        /// Gets or sets the attached ScalingManager.
        /// </summary>
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
		public static int FramesPerSecond
		{
			get { return fps; }
		}
    }
}
