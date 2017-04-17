using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MoonSharp.Interpreter;

using BareKit.Audio;
using BareKit.Tweening;

namespace BareKit.Graphics
{
    [MoonSharpUserData]
    public sealed class Stage : Container
    {
        /// <summary>
        /// Initializes a new instance of the Stage class.
        /// </summary>
        /// <param name="scaling">The ScalingManager attached to the Scenes.</param>
        /// <param name="content">The content pipline attached to the Scenes.</param>
        /// <param name="tweening">The Glide tweening instacne attached to the Scenes.</param>
        /// <param name="sound">The SoundManager attached to the Scenes.</param>
        /// <param name="global">The global Database attached to the Scenes.</param>
        public Stage(ScalingManager scaling, ContentManager content, Tweener tweening, SoundManager sound, Database global)
        {
			Initialize(scaling);
            Content = content;
			Tweening = tweening;
			Sound = sound;
            Global = global;

            // Default background color (hex #282828)
            Color = new Color(40, 40, 40);
        }

        /// <summary>
        /// Sends an updatecall to the current Scene.
        /// </summary>
        /// <param name="delta">The time it took since the last updatecall in seconds.</param>
		public void Update(float delta)
        {
            if (Children.Count > 0)
				((Scene)Children[Children.Count - 1]).Update(delta);
        }

        public override void Draw(SpriteBatch buffer, Matrix transform)
        {
            if (Children.Count > 0)
                Children[Children.Count - 1].Draw(buffer, transform);
        }

        /// <summary>
        /// Navigates to a specific Scene by its type.
        /// </summary>
        /// <param name="sceneType">The type of the Scene.</param>
        public Stage NavigateTo(Type sceneType)
        {
			return NavigateTo((Scene)Activator.CreateInstance(sceneType));
        }

        /// <summary>
        /// Navigates to a specific Scene by its instance.
        /// </summary>
        /// <param name="sceneInstance">The instance of the Scene.</param>
		public Stage NavigateTo(Scene sceneInstance)
		{
			Scene current = null;

			if (Children.Count > 0)
				current = (Scene)Children[Children.Count - 1];

			current?.Leave(false);

			AddChild(sceneInstance);
			sceneInstance.Enter(current);

		    Logger.Info(GetType(),
		        current == null
		            ? $"Initial navigation to '{sceneInstance.GetType().Name}'."
		            : $"Navigated from '{current.GetType().Name}' to '{sceneInstance.GetType().Name}'.");

		    return this;
		}

        /// <summary>
        /// Navigates back to the previous Scene.
        /// </summary>
        public Stage NavigateBack()
        {
            if (!CanNavigateBack) return this;
            var current = (Scene)Children[Children.Count - 1];
            var target = (Scene)Children[Children.Count - 2];

            current.Leave(true);
            target.Enter(current);

            RemoveChild(current);

            Logger.Info(GetType(), $"Navigated back from '{current.GetType().Name}' to '{target.GetType().Name}'.");

            return this;
        }

        /// <summary>
        /// Gets the value indicating whether a backwards navigation is posssible.
        /// </summary>
        public bool CanNavigateBack => Children.Count > 1;

        /// <summary>
        /// Gets the attached content pipline.
        /// </summary>
		public ContentManager Content { get; }

        /// <summary>
        /// Gets the attached Glide tweening instance.
        /// </summary>
		public Tweener Tweening { get; }

        /// <summary>
        /// Gets the attached SoundManager.
        /// </summary>
		public SoundManager Sound { get; }

        /// <summary>
        /// Gets the attached global Database.
        /// </summary>
		public Database Global { get; }
    }
}
