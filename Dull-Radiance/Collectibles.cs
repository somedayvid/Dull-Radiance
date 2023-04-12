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
    internal class Collectibles
    {
        private Texture2D keyTexture;
        private Rectangle keyRect;
        private Color color;

        //Properties
        public Color Color
        {
            get { return color; }
        }

        public Texture2D KeyTexture
        {
            get { return keyTexture; }
        }


        public Collectibles(Texture2D keyTexture, Color color)
        {
            this.keyTexture = keyTexture;
            this.color = color;
        }

        /*
        public bool Intersects(Player player)
        {
            if (keyRect.Intersects(player.Bounds))
            {
                return true;
            }
            else
            { 
                return false; }
        }
        */

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
