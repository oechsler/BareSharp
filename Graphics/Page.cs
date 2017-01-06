using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BareKit.Graphics
{
    public class Page : Container
    {
		ContentManager content;
        Stage stage;

		public Page(ScalingManager scaling, ContentManager content, Stage stage) : base(scaling)
        {
			this.content = content;
            this.stage = stage;

            Scaling.Resized += (object sender, EventArgs e) =>
            {
                Resized();
            };
        }

        public virtual void Enter(Page from)
        {

        }

        public virtual void Leave(bool terminate)
        {

        }

        public virtual void Update(GameTime delta)
        {
            
        }

        public virtual void Resized()
        {

        }

		protected ContentManager Content
		{
			get { return content; }
		}

        protected Stage Stage
        {
            get { return stage; }
        }
    }
}
