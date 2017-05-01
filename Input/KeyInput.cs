using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BareKit.Input
{
	public class KeyInput : Input
	{
		KeyboardState previousState;
		KeyboardState currentState;
	    readonly Keys key;

        /// <summary>
        /// Initializes a new instance of the KeyInput class.
        /// </summary>
        /// <param name="inputState">The state the input device needs to be in for the event to trigger.</param>
        /// <param name="key">The key the KeyboardInput is listening to.</param>
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
			    case InputState.Moved:
			        break;
			    case InputState.Unknown:
			        break;
			    default:
			        throw new ArgumentOutOfRangeException();
			}

			previousState = currentState;
			currentState = Keyboard.GetState();
		}
	}
}
