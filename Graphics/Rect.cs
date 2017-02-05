using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
	public class Rect : Drawable
	{
		SpriteBatch primitiveBuffer;
		bool filled;
		float thickness;
		bool isRendered;

		public Rect(bool filled = true)
		{
			this.filled = filled;
			thickness = 1;
			isRendered = true;
		}

		public override void Draw(SpriteBatch buffer)
		{
			base.Draw(buffer);

			// Determine wether the Rect should be rendered 
			RotatedRectangle screenBounds = Scaling.Bounds;
			screenBounds.ChangePosition((int)(-Scaling.Size.X / 2), (int)(-Scaling.Size.Y / 2));
			if (screenBounds.Intersects(Bounds) && Alpha > 0)
			{
				isRendered = true;

				// Allocate a offscreen SpriteBatch buffer for the Rect
				// if none existed to this point
				if (primitiveBuffer == null)
					primitiveBuffer = new SpriteBatch(buffer.GraphicsDevice);

				// Calculate the transform of the Rect
				Matrix transfrom = Matrix.CreateTranslation(-Size.X / 2 + Origin.X, -Size.Y / 2 + Origin.Y, 1) *
								   Matrix.CreateRotationZ(Rotation) *
								   Matrix.CreateTranslation(Scaling.Size.X / 2 + Position.X, Scaling.Size.Y / 2 + Position.Y, 0);

				// Apply the transform to the SpriteBatch buffer and
				// render the primitive rectangle shape to it
				primitiveBuffer.Begin(transformMatrix: transfrom);

				if (filled)
					Primitives2D.FillRectangle(primitiveBuffer, new Vector2(0), Size, Color * Alpha);
				else
					Primitives2D.DrawRectangle(primitiveBuffer, new Vector2(0), Size, Color * Alpha, thickness * Scaling.Scale.X);

				primitiveBuffer.End();
			}
			else
				isRendered = false;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the Rect is filled.
		/// </summary>
		/// <value>The fill indicating value.</value>
		public bool Filled
		{
			get { return filled; }
			set { filled = value; }
		}

		/// <summary>
		/// Gets or sets the tickness displayed onscreen.
		/// </summary>
		/// <value>The tickness displayed onscreen.</value>
		public float Tickness
		{
			get { return thickness; }
			set { thickness = value; }
		}

		/// <summary>
		/// Gets the size vector.
		/// </summary>
		/// <returns>The size vector.</return
		public Vector2 Size
		{
			get { return Scale * Scaling.Scale; }
		}

		/// <summary>
		/// Gets the bounds rectangle.
		/// </summary>
		/// <value>The bounds rectangle.</value>
		public RotatedRectangle Bounds
		{
			get { return new RotatedRectangle(new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y), Rotation); }
		}

		/// <summary>
		/// Gets the value indicating whether the Rect is rendered.
		/// </summary>
		/// <value>The render indicating value.</value>
		public bool IsRendered
		{
			get { return isRendered; }
		}
	}
}
