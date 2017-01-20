using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BareKit.Input
{
	[Flags]
	public enum Finger
	{
		One,
		Two,
		Three,
		Four,
		Five
	}

	public class TouchInput : Input
	{
		TouchCollection touches;
		MouseState previousState;
		MouseState currentState;
		Finger finger;

		RotatedRectangle area;
		Vector2 position;

		public TouchInput(InputState inputState, Finger finger) : base(inputState)
		{
			touches = TouchPanel.GetState();
			previousState = currentState = Mouse.GetState();
			this.finger = finger;

			area = new RotatedRectangle(new Rectangle(int.MinValue / 2, int.MinValue / 2, int.MaxValue, int.MaxValue), 0);
			position = new Vector2(0, 0);
		}

		public override void Update()
		{
			base.Update();

			if(TouchPanel.GetCapabilities().IsConnected)
			{
				Vector2 panelSize = new Vector2(TouchPanel.DisplayWidth, TouchPanel.DisplayHeight);

				TouchLocationState state;
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
					default:
						state = TouchLocationState.Invalid;
						break;
				}

				Finger currentFinger = 0;
				foreach (TouchLocation touch in touches)
				{
					if (currentFinger == finger && touch.State == state)
					{
						position = -Scaling.Size / 2 + touch.Position / panelSize * Scaling.Size;

						if (area.Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))
							Trigger();
					}
					currentFinger++;
				}

				touches = TouchPanel.GetState();
			}
			else
			{
				bool matches = false;

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
				}

				if (matches || TriggerState == InputState.Moved)
				{
					position = currentState.Position.ToVector2() - Scaling.Size / 2;

					if (area.Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))
						Trigger();
				}

				previousState = currentState;
				currentState = Mouse.GetState();
			}
		}

		public RotatedRectangle Area
		{
			get { return area; }
			set { area = value; }
		}

		public Vector2 Position
		{
			get { return position; }
		}
	}
}
