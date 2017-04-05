using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
	public class Rect : Drawable
	{
		SpriteBatch primitiveBuffer;

	    /// <summary>
        /// Initializes a new instance of the Rect class.
        /// </summary>
        /// <param name="filled">Whether the Rect is filled.</param>
		public Rect(bool filled = true)
		{
			this.Filled = filled;
			Thickness = 1;
			IsRendered = true;
		}

		public override void Draw(SpriteBatch buffer, Matrix transform)
		{
			base.Draw(buffer, transform);

			// Determine whether the Rect should be rendered 
			var screenBounds = Scaling.Bounds;
			screenBounds.ChangePosition((int)(-Scaling.Size.X / 2), (int)(-Scaling.Size.Y / 2));
			if (screenBounds.Intersects(Bounds) && Alpha > 0)
			{
				IsRendered = true;

				// Allocate a offscreen SpriteBatch buffer for the Rect
				// if none exists to this point
				if (primitiveBuffer == null)
					primitiveBuffer = new SpriteBatch(buffer.GraphicsDevice);

				// Calculate the transform of the Rect
				transform =  Matrix.CreateTranslation(-Size.X / 2 + Origin.X, -Size.Y / 2 + Origin.Y, 1) *
						     Matrix.CreateRotationZ(Rotation) *
							 Matrix.CreateTranslation(Scaling.Size.X / 2 - Origin.X + Position.X, Scaling.Size.Y / 2 - Origin.Y + Position.Y, 0) * 
                             transform;

				// Apply the transform to the SpriteBatch buffer and
				// render the primitive rectangle shape to it
				primitiveBuffer.Begin(transformMatrix: transform);

				if (Filled)
					primitiveBuffer.FillRectangle(new Vector2(0), Size, Color * Alpha);
				else
					primitiveBuffer.DrawRectangle(new Vector2(0), Size, Color * Alpha, Thickness * Scaling.Scale.X);

				primitiveBuffer.End();
			}
			else
				IsRendered = false;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the Rect is filled.
		/// </summary>
		public bool Filled { get; set; }

	    /// <summary>
		/// Gets or sets the thickness displayed onscreen.
		/// </summary>
		public float Thickness { get; set; }

	    /// <summary>
		/// Gets the size vector.
		/// </summary>
		public Vector2 Size => Scale * Scaling.Scale;

	    /// <summary>
		/// Gets the bounds rectangle.
		/// </summary>
		public RotatedRectangle Bounds => new RotatedRectangle(new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y), Rotation, Origin);

	    /// <summary>
		/// Gets the value indicating whether the Rect is rendered.
		/// </summary>
		public bool IsRendered { get; set; }
	}
}
