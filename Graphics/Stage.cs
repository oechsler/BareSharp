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
    public class Stage : Container
    {
		ContentManager content;
		Tweener tweening;
		SoundManager sound;

        /// <summary>
        /// Initializes a new instance of the Stage class.
        /// </summary>
        /// <param name="scaling">The ScalingManager attached to the Scenes.</param>
        /// <param name="content">The content pipline attached to the Scenes.</param>
        /// <param name="tweening">The Glide tweening instacne attached to the Scenes.</param>
        /// <param name="sound">The SoundManager attached to the Scenes.</param>
		public Stage(ScalingManager scaling, ContentManager content, Tweener tweening, SoundManager sound)
        {
			Initialize(scaling);
			this.content = content;
			this.tweening = tweening;
			this.sound = sound;

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

        public override void Draw(SpriteBatch buffer)
        {
            if (Children.Count > 0)
                Children[Children.Count - 1].Draw(buffer);
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

            if (current == null)
                Logger.Info(GetType(), $"Initial navigation to '{sceneInstance.GetType().Name}'.");
            else
                Logger.Info(GetType(), $"Navigated from '{current.GetType().Name}' to '{sceneInstance.GetType().Name}'.");


            return this;
		}

        /// <summary>
        /// Navigates back to the previous Scene.
        /// </summary>
        public Stage NavigateBack()
        {
            if (CanNavigateBack)
            {
                Scene current = (Scene)Children[Children.Count - 1];
                Scene target = (Scene)Children[Children.Count - 2];

                current.Leave(true);
                target.Enter(current);

                RemoveChild(current);

                Logger.Info(GetType(), $"Navigated back from '{current.GetType().Name}' to '{target.GetType().Name}'.");
            }

            return this;
        }

        /// <summary>
        /// Gets the value indicating whether a backwards navigation is posssible.
        /// </summary>
        public bool CanNavigateBack
        {
            get { return Children.Count > 1 ? true : false; }
        }

        /// <summary>
        /// Gets the attached content pipline.
        /// </summary>
		public ContentManager Content
		{
			get { return content; }
		}

        /// <summary>
        /// Gets the attached Glide tweening instance.
        /// </summary>
		public Tweener Tweening
		{
			get { return tweening; }
		}

        /// <summary>
        /// Gets the attached SoundManager.
        /// </summary>
		public SoundManager Sound
		{
			get { return sound; }
		}
    }
}
