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

		}

		/// <summary>
		/// Enters the Scene.
		/// </summary>
		/// <param name="from">The Scene which the was triggered from.</param>
        public virtual void Enter(Scene from)
        {
			if (!contentLoaded)
			{
				LoadContent();
				contentLoaded = true;
			}
        }

		/// <summary>
		/// Unloads the drawables and other components used in the Scene.
		/// </summary>
		protected virtual void UnloadContent()
		{

		}

		/// <summary>
		/// Leave the Scene.
		/// </summary>
		/// <param name="terminate">The value indicating whether the Scene is removed from memory.</param>
        public virtual void Leave(bool terminate)
        {
			if (terminate)
				UnloadContent();

			Tweening.CancelAndComplete();
        }

		/// <summary>
		/// Updates the logic components of the Scene.
		/// </summary>
		public virtual void Update(float delta)
        {
			input.Update();
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
