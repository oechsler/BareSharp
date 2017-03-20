 using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
    public class Container : Drawable
    {
        List<Drawable> drawables;
        SpriteBatch containerBuffer;

        BlendState blendMode;
        Shader shader;

		public Container()
        {
            drawables = new List<Drawable>();

            blendMode = BlendState.AlphaBlend;
        }

        public override void Draw(SpriteBatch buffer, Matrix transform)
        {
            base.Draw(buffer, transform);

			// Allocate a offscreen SpriteBatch buffer for the Container
			// if none exists to this point
            if (containerBuffer == null)
                containerBuffer = new SpriteBatch(buffer.GraphicsDevice);

			// Calculate the transform of the Container
			transform = Matrix.CreateTranslation(-Scaling.Size.X / 2 + Origin.X, -Scaling.Size.Y / 2 + Origin.Y, 0) *
						 Matrix.CreateRotationZ(Rotation) *
						 Matrix.CreateTranslation(Scaling.Size.X / 2 + Position.X - Origin.X, Scaling.Size.Y / 2 + Position.Y - Origin.Y, 0) *
						 Matrix.CreateScale(Scale.X, Scale.Y, 1) * 
                         transform;

			// Apply the transform to the SpriteBatch buffer and
			// render the contained Drawables to it
			containerBuffer.Begin(transformMatrix: transform, blendState: blendMode, effect: shader?.Effect);
            foreach (Drawable drawable in drawables)
                drawable.Draw(containerBuffer, transform);
            containerBuffer.End();
        }

        /// <summary>
        /// Gets or sets the used blending mode.
        /// </summary>
        public BlendState BlendMode
        {
            get { return blendMode; }
            set { blendMode = value; }
        }

        /// <summary>
        /// Gets or sets the used shader.
        /// </summary>
        public Shader Shader
        {
            get { return shader; }
            set { shader = value; }
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
            get { return new RotatedRectangle(new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2), (int)Size.X / 2, (int)Size.Y / 2), Rotation, Origin); }
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
