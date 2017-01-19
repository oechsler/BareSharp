using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glide;

using BareKit.Audio;

namespace BareKit.Graphics
{
    public class Stage : Container
    {
		ContentManager content;
		Tweener tweening;
		SoundManager sound;

		public Stage(ScalingManager scaling, ContentManager content, Tweener tweening, SoundManager sound)
        {
			Initialize(scaling);
			this.content = content;
			this.tweening = tweening;
			this.sound = sound;

			Color = new Color(40, 40, 40);
        }

        public void Update(GameTime delta)
        {
            if (Children.Count > 0)
				((Scene)Children[Children.Count - 1]).Update(delta);
        }

        public override void Draw(SpriteBatch buffer)
        {
            if (Children.Count > 0)
                Children[Children.Count - 1].Draw(buffer);
        }

        public Stage NavigateTo(Type pageType)
        {
			return NavigateTo((Scene)Activator.CreateInstance(pageType));
        }

		public Stage NavigateTo(Scene pageInstance)
		{
			Scene current = null;

			if (Children.Count > 0)
				current = (Scene)Children[Children.Count - 1];

			current?.Leave(false);

			AddChild(pageInstance);
			pageInstance.Enter(current);

			return this;
		}

        public Stage NavigateBack()
        {
            if (Children.Count > 1)
            {
                Scene current = (Scene)Children[Children.Count - 1];
                Scene target = (Scene)Children[Children.Count - 2];

                current.Leave(true);
                target.Enter(current);

                RemoveChild(current);
            }

            return this;
        }

		public ContentManager Content
		{
			get { return content; }
		}

		public Tweener Tweening
		{
			get { return tweening; }
		}

		public SoundManager Sound
		{
			get { return sound; }
		}
    }
}
