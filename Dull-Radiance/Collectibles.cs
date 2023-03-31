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
        public event AddToInventoryDelegate OnAddToInventory;

        //Properties
        /// <summary>
        /// Receive X coordinate
        /// </summary>
        public int X { get; }
        /// <summary>
        /// Receive y coordinate
        /// </summary>
        public int Y { get; }


        public Collectibles(Texture2D keyTexture, Color color)
        {
            this.keyTexture = keyTexture;
        }

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
