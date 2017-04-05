using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BareKit.Input
{
	[Flags]
	public enum Finger
	{
		One = 0,
		Two = 1,
		Three = 2,
		Four = 3,
		Five = 4
	}

	public class TouchInput : Input
	{
		TouchCollection touches;
		MouseState previousState;
		MouseState currentState;
	    readonly Finger finger;

	    /// <summary>
        /// Initializes a new instance of the TouchInput class.
        /// </summary>
        /// <param name="inputState">The state the input device needs to be in for the event to trigger.</param>
        /// <param name="finger">The finger id the TouchInput is listening to.</param>
		public TouchInput(InputState inputState, Finger finger) : base(inputState)
		{
			touches = TouchPanel.GetState();
			previousState = currentState = Mouse.GetState();
			this.finger = finger;

			Bounds = new RotatedRectangle(new Rectangle(int.MinValue / 2, int.MinValue / 2, int.MaxValue, int.MaxValue), 0);
			Position = new Vector2(0, 0);
		}

		public override void Update()
		{
			base.Update();

			if(TouchPanel.GetCapabilities().IsConnected)
			{
				var panelSize = new Vector2(TouchPanel.DisplayWidth, TouchPanel.DisplayHeight);

				var state = TouchLocationState.Invalid;
				switch (TriggerState)
				{
					case InputState.Pressed:
						state = TouchLocationState.Pressed;
						break;
					case InputState.Released:
						state = TouchLocationState.Released;
						break;
					case InputState.Moved:
						state = TouchLocationState.Moved;
						break;
					case InputState.Down:
						state = TouchLocationState.Moved;
						break;
				    case InputState.Unknown:
				        break;
				    default:
						state = TouchLocationState.Invalid;
						break;
				}

				Finger currentFinger = 0;
				foreach (var touch in touches)
				{
					if (currentFinger == finger && touch.State == state)
					{
						Position = -Scaling.Size / 2 + touch.Position / panelSize * Scaling.Size;

						if (Bounds.Intersects(new Rectangle((int)Position.X, (int)Position.Y, 1, 1)))
							Trigger();
					}
					currentFinger++;
				}

				touches = TouchPanel.GetState();
			}
			else
			{
				var matches = false;

				switch (finger)
				{
					case Finger.One:
						if (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released && TriggerState == InputState.Pressed)
							matches = true;
						else if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed && TriggerState == InputState.Released)
							matches = true;
						else if (currentState.LeftButton == ButtonState.Pressed && (TriggerState == InputState.Moved || TriggerState == InputState.Down))
							matches = true;
						break;
					case Finger.Two:
						if (currentState.RightButton == ButtonState.Pressed && previousState.RightButton == ButtonState.Released && TriggerState == InputState.Pressed)
							matches = true;
						else if (currentState.RightButton == ButtonState.Released && previousState.RightButton == ButtonState.Pressed && TriggerState == InputState.Released)
							matches = true;
						else if (currentState.RightButton == ButtonState.Pressed && (TriggerState == InputState.Moved || TriggerState == InputState.Down))
							matches = true;
						break;
				    case Finger.Three:
				        break;
				    case Finger.Four:
				        break;
				    case Finger.Five:
				        break;
				    default:
				        throw new ArgumentOutOfRangeException();
				}

				if (matches || TriggerState == InputState.Moved)
				{
					Position = currentState.Position.ToVector2() - Scaling.Size / 2;

					if (Bounds.Intersects(new Rectangle((int)Position.X, (int)Position.Y, 1, 1)))
						Trigger();
				}

				previousState = currentState;
				currentState = Mouse.GetState();
			}
		}

        /// <summary>
		/// Gets or sets the triggering bounds rectangle.
		/// </summary>
		public RotatedRectangle Bounds { get; set; }

	    /// <summary>
        /// Gets the (cursor-)postition vector.
        /// </summary>
		public Vector2 Position { get; set; }
	}
}
