using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BareKit.Graphics
{
    class Sprite : Drawable
    {
        readonly ContentManager content;

        readonly string assetName;
        Vector2 assetScale;
        Vector2 screenScale;
        Texture2D texture;

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
			IsRendered = true;
        }

		public override void Initialize(ScalingManager scaling)
		{
			base.Initialize(scaling);

			Scaling.Resized += OnResize;
			OnResize(Scaling, EventArgs.Empty);
		}

        public override void Draw(SpriteBatch buffer, Matrix transform)
        {
            base.Draw(buffer, transform);

            // Determine whether the Label should be rendered 
            var screenBounds = Scaling.Bounds;
			screenBounds.ChangePosition((int)(-Scaling.Size.X / 2), (int)(-Scaling.Size.Y / 2));
			if (screenBounds.Intersects(Bounds) && Alpha > 0)
			{
				IsRendered = true;
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
				IsRendered = false;
        }

        void OnResize(object sender, EventArgs e)
        {
            // Scaling factor the sprite asset is optimaly displayed with
            var targetAssetScale = new Vector2((float)Math.Max(1, Math.Round(Scaling.Scale.X)));
            if (assetScale != targetAssetScale)
            {
                assetScale = targetAssetScale;
                for (var i = (int) assetScale.X; i > 0; i--)
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
        /// Canges the attached texture asset.
        /// </summary>
        /// <param name="assetName">The within the content pipeline assigned name.</param>
		public Sprite Change(string assetName)
        {
            texture = content.Load<Texture2D>(assetName);

            return this;
        }

        /// <summary>
		/// Gets the size vector.
		/// </summary>
        public Vector2 Size => new Vector2(texture.Width, texture.Height) * screenScale * Scale;

        /// <summary>
		/// Gets the bounds rectangle.
		/// </summary>
        public RotatedRectangle Bounds => new RotatedRectangle(new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y), Rotation, Origin);

        /// <summary>
		/// Gets the value indicating whether the Label is rendered.
		/// </summary>
		public bool IsRendered { get; set; }
    }
}
