using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BareKit.Graphics
{
    public class Sprite : Drawable
    {
        readonly ContentManager content;

        string assetName;
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
            assetScale = new Vector2(1);
            texture = content.Load<Texture2D>(assetName + "_1x");
            TextureRect = texture.Bounds;
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
                            sourceRectangle: new Rectangle((int)(TextureRect.X * assetScale.X), (int)(TextureRect.Y * assetScale.X), (int)(TextureRect.Width * assetScale.X), (int)(TextureRect.Height * assetScale.X)),
							origin: new Vector2(TextureRect.Width, TextureRect.Height) / 2 + Origin,
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
            this.assetName = assetName;
            assetScale = new Vector2(1);
            texture = content.Load<Texture2D>(assetName + "_1x");
            TextureRect = texture.Bounds;

            OnResize(Scaling, EventArgs.Empty);

            return this;
        }

        public Rectangle TextureRect { get; set; }

        public int TextureRectX
        {
            get => TextureRect.X;
            set => TextureRect = new Rectangle(value, TextureRect.Y, TextureRect.Width, TextureRect.Height);
        }

        public int TextureRectY
        {
            get => TextureRect.Y;
            set => TextureRect = new Rectangle(TextureRect.X, value, TextureRect.Width, TextureRect.Height);
        }

        public int TextureRectWidth
        {
            get => TextureRect.Width;
            set => TextureRect = new Rectangle(TextureRect.X, TextureRect.Y, value, TextureRect.Height);
        }

        public int TextureRectHeight
        {
            get => TextureRect.X;
            set => TextureRect = new Rectangle(TextureRect.X, TextureRect.Y, TextureRect.Width, value);
        }

        /// <summary>
        /// Gets the size vector.
        /// </summary>
        public Vector2 Size => new Vector2(TextureRect.Width, TextureRect.Height) * screenScale * Scale;

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
