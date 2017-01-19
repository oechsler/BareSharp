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

		public event EventHandler<EventArgs> Triggered;

		public Input(InputState triggerState)
		{
			this.triggerState = triggerState;
		}

		public void Initialize(ScalingManager scaling)
		{
			this.scaling = scaling;
		}

		public virtual void Update()
		{
			// TODO: Checking code goes here ...
		}

		public void Trigger()
		{
			Triggered?.Invoke(this, EventArgs.Empty);
		}

		protected InputState TriggerState
		{
			get { return triggerState; }
		}

		protected ScalingManager Scaling
		{
			get { return scaling; }
		}
	}
}
