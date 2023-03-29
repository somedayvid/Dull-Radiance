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
        private Player player;
        private Rectangle keyRect;
        private Color color;  

        //Properties
        /// <summary>
        /// Receive X coordinate
        /// </summary>
        int X { get; }
        /// <summary>
        /// Receive y coordinate
        /// </summary>
        int Y { get; }


        public Collectibles(Texture2D keyTexture, Player player, Color color)
        {
            this.keyTexture = keyTexture;
            this.color = color;
            keyRect = new Rectangle(1,1, 100,100);  //TODO do not hardcode
        }

        //Methods
        /// <summary>
        /// Checks for collision with player
        /// </summary>
        /// <param name="playerRect">Player playerRect Position</param>
        public void Intersects(Player player)
        {
            //if(keyRect.Intersects(player.PlayerRect))
            //{

            //}
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
