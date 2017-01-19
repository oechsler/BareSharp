using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glide;

using BareKit.Audio;
using BareKit.Input;

namespace BareKit.Graphics
{
    public class Scene : Container
    {
		InputManager input;

		bool contentLoaded;

		public sealed override void Initialize(ScalingManager scaling)
		{
			base.Initialize(scaling);

			input = new InputManager(scaling);

			scaling.Resized += (object sender, EventArgs e) =>
			{
				Resized();
			};
		}

		protected virtual void LoadContent()
		{

		}

        public virtual void Enter(Scene from)
        {
			if (!contentLoaded)
			{
				LoadContent();
				contentLoaded = true;
			}
        }

		protected virtual void UnloadContent()
		{

		}

        public virtual void Leave(bool terminate)
        {
			if (terminate)
				UnloadContent();

			Tweening.CancelAndComplete();
        }

        public virtual void Update(GameTime delta)
        {
			input.Update();
        }

		public sealed override void Draw(SpriteBatch buffer)
		{
			base.Draw(buffer);
		}

		protected virtual void Resized()
        {
			
        }

		protected Stage Stage
		{
			get { return (Stage)Parent; }
		}

		protected ContentManager Content
		{
			get { return ((Stage)Parent).Content; }
		}

		protected Tweener Tweening
		{
			get { return ((Stage)Parent).Tweening; }
		}

		protected SoundManager Sound 
		{
			get { return ((Stage)Parent).Sound; }
		}

		protected InputManager Input
		{
			get { return input; }
		}
    }
}
