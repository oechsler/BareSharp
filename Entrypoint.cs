using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Glide;

using BareKit.Audio;
using BareKit.Graphics;

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

			scaling.Center();

			sound = new SoundManager();

            buffer = new SpriteBatch(GraphicsDevice);
			tweening = new Tweener();
			stage = new Stage(scaling, Content, tweening, sound);
        }

        protected override void Update(GameTime gameTime)
        {
			sound.Update();

			tweening.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            stage.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(stage.Color);

            buffer.Begin();
            stage.Draw(buffer);
            buffer.End();

            base.Draw(gameTime);
        }

		protected GraphicsDeviceManager Graphics
		{
			get { return graphics; }
		}

		protected ScalingManager Scaling
		{
			get { return scaling; }
			set { scaling = value; } 
		}

		protected SpriteBatch Buffer
		{
			get { return buffer; }
		}

		protected Stage Stage
		{
			get { return stage; }
		}
    }
}
