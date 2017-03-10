using System;
#if MONOMAC
using System.Drawing;
#endif

using Microsoft.Xna.Framework;

namespace BareKit.Graphics
{
    public class ScalingManager
    {
		GameWindow window;
#if MONOMAC
		GraphicsDeviceManager graphics;
#endif

        Vector2 initialSize;
        Vector2 currentSize;
        float contentScale;
        DisplayOrientation orientation;

		/// <summary>
		/// Occurs when a resizement is made to the controlled window.
		/// </summary>
        public event EventHandler<EventArgs> Resized;

		/// <summary>
		/// Initializes a new instance of the ScalingManager class.
		/// </summary>
		/// <param name="graphics">The GraphicsDeviceManager used by the ScalingManager.</param>
		/// <param name="window">The window the ScalingManger is controlling.</param>
		/// <param name="size">The initial size of the controlled window.</param>
		/// <param name="scale">Scaling factor applied to the size of the controlled window.</param>
		/// <param name="fullscreen">Whether the controlled window is in fullscreen.</param>
		public ScalingManager(GraphicsDeviceManager graphics, GameWindow window, Vector3 size, float scale = 1, bool fullscreen = false)
        {
#if MONOMAC
			this.graphics = graphics;
#endif
			this.window = window;

            window.ClientSizeChanged += OnResized;
            window.OrientationChanged += OnResized;
			window.AllowUserResizing = true;

			initialSize = new Vector2(size.X, size.X / size.Y * size.Z);
			graphics.PreferredBackBufferWidth = (int)(initialSize.X * scale);
			graphics.PreferredBackBufferHeight = (int)(initialSize.Y * scale);

            OnResized(window, EventArgs.Empty);
        }

        void OnResized(object sender, EventArgs e)
        {
            currentSize = new Vector2(window.ClientBounds.Width, window.ClientBounds.Height);
            contentScale = Math.Min(currentSize.X / initialSize.X, currentSize.Y / initialSize.Y);

            orientation = window.CurrentOrientation;

            Resized?.Invoke(this, EventArgs.Empty);
        }

#if MONOMAC
		public void Center()
		{
			// Centers the controlled window
			// Only neccesarry under MonoMac due to a missplaced window if non standard size

			Vector2 desktopSize = new Vector2(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
											  graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height
										     );

			Vector2 newPosition = new Vector2((desktopSize.X - currentSize.X) / 2, (desktopSize.Y - currentSize.Y) / 2);

			float titleBarHeight = window.Window.Frame.Height - currentSize.Y;

			PointF drawPosition = new PointF(newPosition.X, newPosition.Y);
			SizeF drawSize = new SizeF(new PointF(currentSize.X, currentSize.Y + titleBarHeight));
			RectangleF drawRect = new RectangleF(drawPosition, drawSize);

			window.Window.SetFrame(drawRect, true);
    }
#endif

        /// <summary>
        /// Fits the specified number to the current scale.
        /// </summary>
        /// <param name="number">The non scaled number.</param>
        public float Fit(float number)
		{
			return number * contentScale;
		}

		/// <summary>
		/// Fit the specified vector to the current scale.
		/// </summary>
		/// <param name="vector">The non scaled vector.</param>
		public Vector2 Fit(Vector2 vector)
		{
			return vector * contentScale;
		}

		/// <summary>
		/// Unfits the specified number from the current scale.
		/// </summary>
		public float UnFit(float number)
		{
			return number / contentScale;
		}

		/// <summary>
		/// Unfits the specified vector from the current scale.
		/// </summary>
		public Vector2 UnFit(Vector2 vector)
		{
			return vector / contentScale;
		}

		/// <summary>
		/// Gets the controlled windows size vector.
		/// </summary>
        public Vector2 Size
        {
            get { return currentSize; }
        }

		/// <summary>
		/// Gets the controlled windows bounds rectangle.
		/// </summary>
		public RotatedRectangle Bounds
		{
			get { return new RotatedRectangle(new Microsoft.Xna.Framework.Rectangle(0, 0, (int)currentSize.X, (int)currentSize.Y), 0); }
		}

		/// <summary>
		/// Gets the current scale.
		/// </summary>
        public Vector2 Scale
        {
            get { return new Vector2(contentScale); }
        }

		/// <summary>
		/// Gets the current display orientation.
		/// </summary>
        public DisplayOrientation Orientation
        {
            get { return orientation; }
        }
    }
}
