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

			RotatedRectangle screenBounds = Scaling.Bounds;
			screenBounds.ChangePosition((int)(-Scaling.Size.X / 2), (int)(-Scaling.Size.Y / 2));
			if (screenBounds.Intersects(Bounds) && Alpha > 0)
			{
				isRendered = true;

				if (primitiveBuffer == null)
					primitiveBuffer = new SpriteBatch(buffer.GraphicsDevice);

				Matrix transfrom = Matrix.CreateTranslation(-Size.X / 2 + Origin.X, -Size.Y / 2 + Origin.Y, 1) *
								   Matrix.CreateRotationZ(Rotation) *
								   Matrix.CreateTranslation(Scaling.Size.X / 2 + Position.X, Scaling.Size.Y / 2 + Position.Y, 0);

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

		public bool Filled
		{
			get { return filled; }
			set { filled = value; }
		}

		public float Tickness
		{
			get { return thickness; }
			set { thickness = value; }
		}
		
		public Vector2 Size
		{
			get { return Scale * Scaling.Scale; }
		}

		public RotatedRectangle Bounds
		{
			get { return new RotatedRectangle(new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y), Rotation); }
		}

		public bool IsRendered
		{
			get { return isRendered; }
		}
	}
}
