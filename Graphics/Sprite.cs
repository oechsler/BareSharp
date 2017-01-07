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

        public Sprite(ContentManager content, string assetName)
        {
            this.content = content;
            this.assetName = assetName;

            texture = content.Load<Texture2D>(assetName + "_1x");
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

            buffer.Draw(texture: texture,
			            position: Scaling.Size / 2 + Position,
                        origin: new Vector2(texture.Width, texture.Height) / 2 + Origin,
                        rotation: Rotation,
                        scale: screenScale * Scale,
                        color: Color * Alpha
                       );
        }

        void onResize(object sender, EventArgs e)
        {
            Vector2 targetAssetScale = new Vector2((float)Math.Max(1, Math.Round(Scaling.Scale.X)));
            if (assetScale != targetAssetScale)
            {
                assetScale = targetAssetScale;
                for (int i = (int) assetScale.X; i > 0; i--)
                {
                    try
                    {
                        texture = content.Load<Texture2D>(assetName + "_" + i + "x");
                        break;
                    }
                    catch (Exception) { assetScale = new Vector2(i - 1); }
                }
            }

            screenScale = new Vector2(1) / assetScale * Scaling.Scale;
        }

        public Vector2 Size
        {
            get { return new Vector2(texture.Width, texture.Height) * screenScale; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y); }
        }
    }
}
