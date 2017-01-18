using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BareKit.Input
{
	public class GamepadInput : Input
	{
		GamePadState previousState;
		GamePadState currentState;
		Buttons button;

		public GamepadInput() : base(InputState.Unknown)
		{
			previousState = currentState = GamePad.GetState(PlayerIndex.One);
		}

		public GamepadInput(InputState inputState, Buttons button) : base(inputState)
		{
			previousState = currentState = GamePad.GetState(PlayerIndex.One);
			this.button = button;
		}

		public override void Update()
		{
			base.Update();

			if (currentState.IsConnected)
			{
				switch (TriggerState)
				{
					case InputState.Pressed:
						if (previousState.IsButtonUp(button) && currentState.IsButtonDown(button))
							Trigger();
						break;
					case InputState.Released:
						if (currentState.IsButtonUp(button) && previousState.IsButtonDown(button))
							Trigger();
						break;
					case InputState.Down:
						if (currentState.IsButtonDown(button))
							Trigger();
						break;
				}
			}

			previousState = currentState;
			currentState = GamePad.GetState(PlayerIndex.One);
		}

		public bool IsConnected
		{
			get { return currentState.IsConnected; }
		}

		public Vector2 LeftStick
		{
			get {
				if (currentState.IsConnected)
					return currentState.ThumbSticks.Left;
				return new Vector2(0);
			}
		}

		public Vector2 RightStick
		{
			get
			{
				if (currentState.IsConnected)
					return currentState.ThumbSticks.Right;
				return new Vector2(0);
			}
		}

		public float LeftTrigger
		{
			get
			{
				if (currentState.IsConnected)
					return currentState.Triggers.Left;
				return 0;
			}
		}

		public float RightTrigger
		{
			get
			{
				if (currentState.IsConnected)
					return currentState.Triggers.Right;
				return 0;
			}
		}
	}
}
