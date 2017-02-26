using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BareKit.Audio;
using BareKit.Input;
using BareKit.Tweening;

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

		/// <summary>
		/// Loads the drawables and other components used in the Scene.
		/// </summary>
		protected virtual void LoadContent()
		{
            // Load your Drawables here
            // ex.: sprite = new Sprite(Content, "example");
		}

		/// <summary>
		/// Enters the Scene.
		/// </summary>
		/// <param name="from">The Scene which the navigation was triggered from.</param>
        public virtual void Enter(Scene from)
        {
			if (!contentLoaded)
			{
				LoadContent();
				contentLoaded = true;
			}

            // Add your drawables here
            // ex.: AddChild(sprite);
        }

		/// <summary>
		/// Leave the Scene.
		/// </summary>
		/// <param name="terminate">The value indicating whether the Scene will be removed from memory.</param>
        public virtual void Leave(bool terminate)
        {
            // Remove your drawables here
            // ex.: RemoveChild(sprite);

            if (terminate)
				UnloadContent();

			Tweening.CancelAndComplete();
        }

        /// <summary>
		/// Unloads the drawables and other components used in the Scene.
		/// </summary>
		protected virtual void UnloadContent()
        {
            // Unload your Drawables here (mark for gc)
            // ex.: sprite = null;
        }

        /// <summary>
        /// Updates the logic components of the Scene.
        /// </summary>
        public virtual void Update(float delta)
        {
			input.Update();

            // Update needed components and others here
            // ex.: sprite.Position.X += 10 * delta;
        }

		public override sealed void Draw(SpriteBatch buffer)
		{
			base.Draw(buffer);
		}

		/// <summary>
		/// Resizes the Scenes other components
		/// </summary>
		protected virtual void Resized()
        {
            // Resize and/or reposition needed components here
            // ex.: sprite.Scale = Scaling.UnFit(Scaling.Size / new Vector(2));
        }

        /// <summary>
        /// Gets the Stage the Scene is a child of.
        /// </summary>
		protected Stage Stage
		{
			get { return (Stage)Parent; }
		}

        /// <summary>
        /// Gets the attached content pipeline.
        /// </summary>
		protected ContentManager Content
		{
			get { return ((Stage)Parent).Content; }
		}

        /// <summary>
        /// Gets the attached Glide tweening instance.
        /// </summary>
		protected Tweener Tweening
		{
			get { return ((Stage)Parent).Tweening; }
		}

        /// <summary>
        /// Gets the attached SoundManager.
        /// </summary>
		protected SoundManager Sound 
		{
			get { return ((Stage)Parent).Sound; }
		}

        /// <summary>
        /// Gets the Scenes InputManager.
        /// </summary>
		protected InputManager Input
		{
			get { return input; }
		}
    }
}
