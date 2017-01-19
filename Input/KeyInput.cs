using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BareKit.Input
{
	public class KeyInput : Input
	{
		KeyboardState previousState;
		KeyboardState currentState;
		Keys key;

		public KeyInput(InputState inputState, Keys key) : base(inputState)
		{
			previousState = currentState = Keyboard.GetState();
			this.key = key;
		}

		public override void Update()
		{
			base.Update();

			switch (TriggerState)
			{
				case InputState.Pressed:
					if (currentState.IsKeyDown(key) && previousState.IsKeyUp(key))
						Trigger();
					break;
				case InputState.Released:
					if (currentState.IsKeyUp(key) && previousState.IsKeyDown(key))
						Trigger();
					break;
				case InputState.Down:
					if (currentState.IsKeyDown(key))
						Trigger();
					break;
			}

			previousState = currentState;
			currentState = Keyboard.GetState();
		}
	}
}
