using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dull_Radiance
{
    /// <summary>
    /// 
    /// </summary>
    internal class Collectibles
    {
        // Variable field
        private Texture2D keyTexture;
        private Rectangle keyRect;
        private Color color;

        /// <summary>
        /// Get property for Color
        /// </summary>
        public Color Color { get { return color; }
        }

        /// <summary>
        /// Get property for KeyTexture
        /// </summary>
        public Texture2D KeyTexture { get { return keyTexture; }
        }

        /// <summary>
        /// Get property for KeyRect
        /// </summary>
        public Rectangle KeyRect { get { return keyRect; }
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="keyTexture">Texture of collectible</param>
        /// <param name="color">Color of collectible</param>
        public Collectibles(Texture2D keyTexture, Color color)
        {
            this.keyTexture = keyTexture;
            this.color = color;
        }

        /// <summary>
        /// Draw Object to Screen
        /// </summary>
        /// <param name="sb">SpriteBatch sb</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                keyTexture,
                keyRect,
                color);
        }
    }
}
