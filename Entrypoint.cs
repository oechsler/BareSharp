using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using BareKit.Audio;
using BareKit.Graphics;
using BareKit.Tweening;
#if !WINDOWS_UAP
using BareKit.Lua;
#else
using MoonSharp.Interpreter;
#endif

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

        Script script;

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

            script = new Script();
            script.Globals["bare"] = new Table(script);
        }

        protected override void Update(GameTime gameTime)
        {
			sound.Update();

			tweening.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            stage.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

			if (oneSecond >= 1)
			{
				fps = frames;
				frames = 0;
				oneSecond--;
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
