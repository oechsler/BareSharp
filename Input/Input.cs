using System;

using BareKit.Graphics;

namespace BareKit.Input
{
	public class Input
	{
		ScalingManager scaling;

		public event EventHandler<EventArgs> Triggered;

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

		protected ScalingManager Scaling
		{
			get { return scaling; }
		}
	}
}
