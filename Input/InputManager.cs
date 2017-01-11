using System.Collections.Generic;

using BareKit.Graphics;

namespace BareKit.Input
{
	public class InputManager
	{
		List<Input> inputs;

		ScalingManager scaling;

		public InputManager(ScalingManager scaling)
		{
			inputs = new List<Input>();

			this.scaling = scaling;
		}

		public void Update()
		{
			foreach (Input input in inputs)
				input.Update();
		}

		public InputManager AddChild(Input child)
		{
			child.Initialize(scaling);
			inputs.Add(child);

			return this;
		}

		public InputManager RemoveChild(Input child)
		{
			inputs.Remove(child);

			return this;
		}
	}
}
