using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
    public class Container : Drawable
    {
        List<Drawable> drawables;
        SpriteBatch containerBuffer;

		public Container()
        {
            drawables = new List<Drawable>();
        }

        public override void Draw(SpriteBatch buffer)
        {
            base.Draw(buffer);

            if (containerBuffer == null)
                containerBuffer = new SpriteBatch(buffer.GraphicsDevice);

			Matrix transfrom = Matrix.CreateTranslation(-Scaling.Size.X / 2 + Origin.X, -Scaling.Size.Y / 2 + Origin.Y, 1) *
						       Matrix.CreateRotationZ(Rotation) *
						       Matrix.CreateTranslation(Scaling.Size.X / 2 + Position.X, Scaling.Size.Y / 2 + Position.Y, 0) *
						       Matrix.CreateScale(Scale.X, Scale.Y, 1);

			containerBuffer.Begin(transformMatrix: transfrom);
            foreach (Drawable drawable in drawables)
                drawable.Draw(containerBuffer);
            containerBuffer.End();
        }

        public Container AddChild(Drawable child)
        {
			child.Initialize(Scaling);
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
