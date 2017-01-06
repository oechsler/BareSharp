using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
    public class Container : Drawable
    {
        List<Drawable> drawables;
        SpriteBatch containerBuffer;

        public Container(ScalingManager scaling) : base(scaling)
        {
            drawables = new List<Drawable>();
        }

        public override void Draw(SpriteBatch buffer)
        {
            base.Draw(buffer);

            if (containerBuffer == null)
                containerBuffer = new SpriteBatch(buffer.GraphicsDevice);

            containerBuffer.Begin();
            foreach (Drawable drawable in drawables)
                drawable.Draw(containerBuffer);
            containerBuffer.End();
        }

        public Container AddChild(Drawable child)
        {
            drawables.Add(child);
            child.Parent = this;

            return this;
        }

        public Container RemoveChild(Drawable child)
        {
            child.Parent = null;
            drawables.Remove(child);

            return this;
        }

        public List<Drawable> Children
        {
            get { return new List<Drawable>(drawables); }
        }
    }
}
