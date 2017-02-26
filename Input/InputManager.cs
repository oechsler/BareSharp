using System.Collections.Generic;

using BareKit.Graphics;

namespace BareKit.Input
{
	public class InputManager
	{
		List<Input> inputs;

		ScalingManager scaling;

        /// <summary>
        /// Initializes a new instance of the InputManager class.
        /// </summary>
        /// <param name="scaling">The ScalingManager attached to the Inputs.</param>
		public InputManager(ScalingManager scaling)
		{
			inputs = new List<Input>();

			this.scaling = scaling;
		}

        /// <summary>
        /// Sends an updatecall to the attached Inputs.
        /// </summary>
		public void Update()
		{
			foreach (Input input in inputs)
				input.Update();
		}

        /// <summary>
		/// Adds a child Input to the InputManager.
		/// </summary>
		/// <param name="child">The child Input being added.</param>
		public InputManager AddChild(Input child)
		{
			child.Initialize(scaling);
			inputs.Add(child);

			return this;
		}

        /// <summary>
		/// Removes a child Input to the InputManager.
		/// </summary>
		/// <param name="child">The child Input being removed.</param>
		public InputManager RemoveChild(Input child)
		{
			inputs.Remove(child);

			return this;
		}
	}
}
