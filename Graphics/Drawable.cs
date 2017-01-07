using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
    public class Drawable
    {
        ScalingManager scaling;

        Vector2 position;
        float rotation;
        Vector2 scale;
        Vector2 origin;

        Color color;
        float alpha;

        Container parent;

        public Drawable()
        {
            position = new Vector2(0);
            rotation = 0;
            scale = new Vector2(1);
            origin = new Vector2(0);

            color = Color.White;
            alpha = 1;
        }

		public virtual void Initialize(ScalingManager scaling)
		{
			this.scaling = scaling;
		}		

        public virtual void Draw(SpriteBatch buffer)
        {
            // TODO: Drawing code goes here ...
        }

        protected ScalingManager Scaling
        {
            get { return scaling; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public float Rotation 
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float ScaleX
        {
            get { return scale.X; }
            set { scale.X = value; }
        }

        public float ScaleY
        {
            get { return scale.Y; }
            set { scale.Y = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public float OriginX
        {
            get { return origin.X; }
            set { origin.X = value; }
        }

        public float OriginY
        {
            get { return origin.Y; }
            set { origin.Y = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public int ColorR
        {
            get { return color.R; }
            set { color.R = (byte)value; }
        }

        public int ColorG
        {
            get { return color.G; }
            set { color.G = (byte)value; }
        }

        public int ColorB
        {
            get { return color.B; }
            set { color.B = (byte)value; }
        }

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public Container Parent
        {
            get { return parent; }
            set
            {
                if (value == null || value.Children.Contains(this))
                    parent = value;
            }
        }
    }
}
