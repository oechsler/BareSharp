using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
    public class Page : Container
    {
		public Page(ScalingManager scaling) : base(scaling)
        {
			Scaling.Resized += (object sender, EventArgs e) =>
            {
                Resized();
            };
        }

        public virtual void Enter(Page from)
        {
			LoadContent();
        }

		protected virtual void LoadContent()
		{
			
		}

        public virtual void Leave(bool terminate)
        {
			if (terminate)
				UnloadContent();
        }

		protected virtual void UnloadContent()
		{
			
		}

        public virtual void Update(GameTime delta)
        {
            
        }

		protected virtual void Resized()
        {
			
        }

		public sealed override void Draw(SpriteBatch buffer)
		{
			base.Draw(buffer);
		}

		protected ContentManager Content
		{
			get { return ((Stage)Parent).Content; }
		}

        protected Stage Stage
        {
			get { return (Stage)Parent; }
        }
    }
}
