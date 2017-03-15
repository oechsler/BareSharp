using System;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MoonSharp.Interpreter.Interop;

using BareKit.Audio;
using BareKit.Input;
using BareKit.Tweening;

namespace BareKit.Graphics
{
    public class Scene : Container
    {
		InputManager input;

		bool contentLoaded;

        /// <summary>
        /// Occures once a navigation to this scene is done.
        /// </summary>
        public event EventHandler<EventArgs> Entered;
        /// <summary>
        /// Occures once a navigation to a other scene is done.
        /// </summary>
        public event EventHandler<EventArgs> Left;
        /// <summary>
        /// Occurs once the scene recieves an update call.
        /// </summary>
        public event EventHandler<EventArgs> Updated;
        /// <summary>
        /// Occurs once the scene recieves a resize call.
        /// </summary>
        public event EventHandler<EventArgs> Resized;

        public sealed override void Initialize(ScalingManager scaling)
		{
			base.Initialize(scaling);

			input = new InputManager(scaling);

			scaling.Resized += (object sender, EventArgs e) =>
			{
				Resize();
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

            Entered?.Invoke(this, EventArgs.Empty);

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

            Left?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
		/// Unloads the drawables and other components used in the Scene.
		/// </summary>
		protected virtual void UnloadContent()
        {
            Content.Unload();
            contentLoaded = false;

            // Unload your Drawables here (mark for gc)
            // ex.: sprite = null;
        }

        /// <summary>
        /// Updates the logic components of the Scene.
        /// </summary>
        public virtual void Update(float delta)
        {
			input.Update();

            Updated?.Invoke(this, EventArgs.Empty);

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
		protected virtual void Resize()
        {
            Resized?.Invoke(this, EventArgs.Empty);

            // Resize and/or reposition needed components here
            // ex.: sprite.Scale = Scaling.UnFit(Scaling.Size / new Vector(2));
        }

        /// <summary>
        /// Gets the Stage the Scene is a child of.
        /// </summary>
        [MoonSharpVisible(true)]
        protected Stage Stage
		{
			get { return (Stage)Parent; }
		}

        /// <summary>
        /// Gets the attached content pipeline.
        /// </summary>
        [MoonSharpVisible(true)]
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
        [MoonSharpVisible(true)]
        protected SoundManager Sound 
		{
			get { return ((Stage)Parent).Sound; }
		}

        /// <summary>
        /// Gets the attached global Database.
        /// </summary>
        [MoonSharpVisible(true)]
        protected Database Global
        {
            get { return ((Stage)Parent).Global; }
        }

        /// <summary>
        /// Gets the Scenes InputManager.
        /// </summary>
        [MoonSharpVisible(true)]
        protected InputManager Input
		{
			get { return input; }
		}
    }
}
