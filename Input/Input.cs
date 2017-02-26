using System;

using BareKit.Graphics;

namespace BareKit.Input
{
	public enum InputState
	{
		Down,
		Pressed,
		Released,
		Moved,
		Unknown
	}

	public class Input
	{
		InputState triggerState;
		ScalingManager scaling;

        /// <summary>
        /// Occurs once the given InputState condition is valid.
        /// </summary>
		public event EventHandler<EventArgs> Triggered;

        /// <summary>
        /// Initializes a new instance of the Input class.
        /// </summary>
        /// <param name="triggerState">The state the input device needs to be in for the event to trigger.</param>
		public Input(InputState triggerState)
		{
			this.triggerState = triggerState;
		}

        /// <summary>
		/// Initializes the Input instance.
		/// </summary>
		/// <param name="scaling">ScalingManager the instance scale is being controled by.</param>
		public void Initialize(ScalingManager scaling)
		{
			this.scaling = scaling;
		}

        /// <summary>
        /// Updates the checking code for the Input device.
        /// </summary>
		public virtual void Update()
		{
			// Add your checking code here
            // ex.: if (condition) { Trigger(); }
		}

        /// <summary>
        /// Releases the instances Triggered event.
        /// </summary>
		public void Trigger()
		{
			Triggered?.Invoke(this, EventArgs.Empty);
		}

        /// <summary>
        /// Gets the needed InputState.
        /// </summary>
		protected InputState TriggerState
		{
			get { return triggerState; }
		}

        /// <summary>
        /// Gets the attached ScalingManager.
        /// </summary>
		protected ScalingManager Scaling
		{
			get { return scaling; }
		}
	}
}
