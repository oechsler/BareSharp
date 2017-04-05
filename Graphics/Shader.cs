using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BareKit.Graphics
{
    public class Shader
    {
        readonly Effect effect;

        /// <summary>
        /// Initializes a ne instance of the Shader class.
        /// </summary>
        /// <param name="content">The content pipeline which the asset will be loaded from.</param>
        /// <param name="assetName">The within the content pipeline assigned name.</param>
        public Shader(ContentManager content, string assetName)
        {
            effect = content.Load<Effect>(assetName);
        }

        /// <summary>
        /// Gets the Shaders parameters table.
        /// </summary>
        public EffectParameterCollection Parameters => effect.Parameters;

        /// <summary>
        /// Gets the Shaders effect.
        /// </summary>
        public Effect Effect => effect;
    }
}
