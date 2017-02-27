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

			// Allocate a offscreen SpriteBatch buffer for the Container
			// if none exists to this point
            if (containerBuffer == null)
                containerBuffer = new SpriteBatch(buffer.GraphicsDevice);

			// Calculate the transform of the Container
			Matrix transfrom = Matrix.CreateTranslation(-Scaling.Size.X / 2 + Origin.X, -Scaling.Size.Y / 2 + Origin.Y, 1) *
						       Matrix.CreateRotationZ(Rotation) *
						       Matrix.CreateTranslation(Scaling.Size.X / 2 + Position.X, Scaling.Size.Y / 2 + Position.Y, 0) *
						       Matrix.CreateScale(Scale.X, Scale.Y, 1);

			// Apply the transform to the SpriteBatch buffer and
			// render the contained Drawables to it
			containerBuffer.Begin(transformMatrix: transfrom);
            foreach (Drawable drawable in drawables)
                drawable.Draw(containerBuffer);
            containerBuffer.End();
        }

        /// <summary>
		/// Gets the size vector.
		/// </summary>
        public Vector2 Size
        {
            get { return Scaling.Size * Scale; }
        }

        /// <summary>
		/// Gets the bounds rectangle.
		/// </summary>
        public RotatedRectangle Bounds
        {
            get { return new RotatedRectangle(new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y), Rotation, Origin); }
        }

        /// <summary>
        /// Adds a child Drawable to the Container.
        /// </summary>
        /// <param name="child">The child Drawable being added.</param>
        public Container AddChild(Drawable child)
        {
			child.Initialize(Scaling);
            drawables.Add(child);
            child.Parent = this;

            return this;
        }

		/// <summary>
		/// Removes a child Drawable from the Container.
		/// </summary>
		/// <param name="child">The child Drawable being removed.</param>
        public Container RemoveChild(Drawable child)
        {
            child.Parent = null;
            drawables.Remove(child);

            return this;
        }

		/// <summary>
		/// Returs a copy of the contianed child Drawables.
		/// </summary>
        public List<Drawable> Children
        {
            get { return new List<Drawable>(drawables); }
        }
    }
}
