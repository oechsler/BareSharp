using System;

using BareKit.Graphics;

namespace BareKit.Input
{
	public enum InputState
	{
		Down = 0,
		Pressed = 1,
		Released = 2
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
