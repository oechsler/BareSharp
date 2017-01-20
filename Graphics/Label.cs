using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BareKit.Graphics
{
    public class Label : Drawable
    {
        ContentManager content;

        string assetName;
        Vector2 assetScale;
        Vector2 screenScale;
        SpriteFont font;

        string text;

        public Label(ContentManager content, string assetName, string text = "")
        {
			this.content = content;
            this.assetName = assetName;

			font = content.Load<SpriteFont>(assetName + "_1x");
            this.text = text;
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

			Rectangle screenBounds = Scaling.Bounds;
			screenBounds.Offset(-Scaling.Size.X / 2, -Scaling.Size.Y / 2);
			if (screenBounds.Intersects(Bounds.CollisionRectangle) && Alpha > 0)
			{
				buffer.DrawString(spriteFont: font,
								  text: text,
								  position: Scaling.Size / 2 + Position,
								  origin: font.MeasureString(text) / 2 + Origin,
								  rotation: Rotation,
								  scale: screenScale * Scale,
								  color: Color * Alpha,
								  effects: SpriteEffects.None,
								  layerDepth: 0
								 );
			}
        }

        void onResize(object sender, EventArgs e)
        {
            Vector2 targetAssetScale = new Vector2((float)Math.Max(1, Math.Round(Scaling.Scale.X*2)));
            if (assetScale != targetAssetScale)
            {
                assetScale = targetAssetScale;
                for (int i = (int) assetScale.X; i > 0; i--)
                {
                    try
                    {
						font = content.Load<SpriteFont>(assetName + "_" + i + "x");
                        break;
                    }
                    catch (Exception) { assetScale = new Vector2(i - 1); }
                }
            }

            screenScale = new Vector2(.5f) / assetScale * Scaling.Scale;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Size
		{
			get { return font.MeasureString(text) * screenScale; }
		}

		public RotatedRectangle Bounds
        {
			get { return new RotatedRectangle(new Rectangle((int)(Position.X - Size.X / 2), (int)(Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y), Rotation); }
		}
    }
}
