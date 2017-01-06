using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
    public class Stage : Container
    {
		ContentManager content;

		public Stage(ScalingManager scaling, ContentManager content) : base(scaling)
        {
			this.content = content;

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
			Page target = (Page)Activator.CreateInstance(pageType, Scaling, content, this);
            Page current = null;

            if (Children.Count > 0)
                current = (Page)Children[Children.Count - 1];
            
            current?.Leave(false);

            AddChild(target);
			target.Enter(current);

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
    }
}
