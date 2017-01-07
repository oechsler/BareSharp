using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glide;

namespace BareKit.Graphics
{
    public class Stage : Container
    {
		ContentManager content;
		Tweener tweening;

		public Stage(ScalingManager scaling, ContentManager content, Tweener tweening)
        {
			Initialize(scaling);
			this.content = content;
			this.tweening = tweening;

			Color = new Color(40, 40, 40);
        }

        public void Update(GameTime delta)
        {
            if (Children.Count > 0)
                ((Page)Children[Children.Count - 1]).Update(delta);
        }

        public override void Draw(SpriteBatch buffer)
        {
            if (Children.Count > 0)
                Children[Children.Count - 1].Draw(buffer);
        }

        public Stage NavigateTo(Type pageType)
        {
			return NavigateTo((Page)Activator.CreateInstance(pageType));
        }

		public Stage NavigateTo(Page pageInstance)
		{
			Page current = null;

			if (Children.Count > 0)
				current = (Page)Children[Children.Count - 1];

			current?.Leave(false);

			AddChild(pageInstance);
			pageInstance.Enter(current);

			return this;
		}

        public Stage NavigateBack()
        {
            if (Children.Count > 1)
            {
                Page current = (Page)Children[Children.Count - 1];
                Page target = (Page)Children[Children.Count - 2];

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
    }
}
