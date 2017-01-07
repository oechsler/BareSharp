﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
    public class Page : Container
    {
		bool contentLoaded;

		public sealed override void Initialize(ScalingManager scaling)
		{
			base.Initialize(scaling);

			Scaling.Resized += (object sender, EventArgs e) =>
			{
				Resized();
			};
		}

		protected virtual void LoadContent()
		{

		}

        public virtual void Enter(Page from)
        {
			if (!contentLoaded)
			{
				LoadContent();
				contentLoaded = true;
			}
        }

		protected virtual void UnloadContent()
		{

		}

        public virtual void Leave(bool terminate)
        {
			if (terminate)
				UnloadContent();
        }

        public virtual void Update(GameTime delta)
        {
            
        }

		public sealed override void Draw(SpriteBatch buffer)
		{
			base.Draw(buffer);
		}

		protected virtual void Resized()
        {
			
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
