using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BareKit.Graphics
{
    class Sprite : Drawable
    {
        ContentManager content;

        string assetName;
        Vector2 assetScale;
        Vector2 screenScale;
        Texture2D texture;
		bool isRendered;

        /// <summary>
        /// Initializes a new instance of the Sprite class.
        /// </summary>
        /// <param name="content">The content pipeline which the asset will be loaded from.</param>
		/// <param name="assetName">The within the content pipeline assigned name.</param>
        public Sprite(ContentManager content, string assetName)
        {
            this.content = content;
            this.assetName = assetName;
            texture = content.Load<Texture2D>(assetName + "_1x");
			isRendered = true;
        }

		public override void Initialize(ScalingManager scaling)
		{
			base.Initialize(scaling);

			Scaling.Resized += onResize;
			onResize(Scaling, EventArgs.Empty);
		}

        public override void Draw(SpriteBatch buffer)
        {
            base.Draw(buffer);

            // Determine whether the Label should be rendered 
            RotatedRectangle screenBounds = Scaling.Bounds;
			screenBounds.ChangePosition((int)(-Scaling.Size.X / 2), (int)(-Scaling.Size.Y / 2));
			if (screenBounds.Intersects(Bounds) && Alpha > 0)
			{
				isRendered = true;
				buffer.Draw(texture: texture,
							position: Scaling.Size / 2 + Position,
                            sourceRectangle: texture.Bounds,
							origin: new Vector2(texture.Width, texture.Height) / 2 + Origin,
							rotation: Rotation,
							scale: screenScale * Scale,
							color: Color * Alpha,
                        
                            effects: SpriteEffects.None,
                            layerDepth: 0
						   );
			}
			else
				isRendered = false;
        }

        void onResize(object sender, EventArgs e)
        {
            // Scaling factor the sprite asset is optimaly displayed with
            Vector2 targetAssetScale = new Vector2((float)Math.Max(1, Math.Round(Scaling.Scale.X)));
            if (assetScale != targetAssetScale)
            {
                assetScale = targetAssetScale;
                for (int i = (int) assetScale.X; i > 0; i--)
                {
                    try
                    {
                        // Try loading the optimal sprite asset from the content pipeline
                        texture = content.Load<Texture2D>(assetName + "_" + i + "x");
                        break;
                    }
                    catch (Exception)
                    {
                        // Fallback to less optimal sprite asset and retry
                        assetScale = new Vector2(i - 1);
                    }
                }
            }

            // Actual scaling factor the sprite asset is displayed onscreen
            screenScale = new Vector2(1) / assetScale * Scaling.Scale;
        }

        /// <summary>
		/// Gets the size vector.
		/// </summary>
        public Vector2 Size
        {
            get { return new Vector2(texture.Width, texture.Height) * screenScale; }
        }

        /// <summary>
		/// Gets the bounds rectangle.
		/// </summary>
        public RotatedRectangle Bounds
		{
			get { return new RotatedRectangle(new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y), Rotation, Origin); }
		}

        /// <summary>
		/// Gets the value indicating whether the Label is rendered.
		/// </summary>
		public bool IsRendered
		{
			get { return isRendered; }
		}
    }
}
