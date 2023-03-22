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
    internal class Walls// : //ICollideAndDraw
    {
        //Fields
        List<Rectangle> allWalls = new List<Rectangle>();

        Rectangle wallRect = new Rectangle();

        //Properties
        /// <summary>
        /// Returns the x position
        /// </summary>
        public int X
        {
            get { return wallRect.X; }
        }

        /// <summary>
        /// Returns the y position
        /// </summary>
        public int Y
        {
            get { return wallRect.Y; }
        }

        //Constructors


        //Methods]
        /*
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                //wallTexture,
                wallRect,
                Color.WhiteSmoke);
        }

        public void Interesects(Player player)
        {
            if interesects
            {
                //stop movement
            }
        }
        */

        /* PsuedoCode W 
         * public void PlayerMoves(x,y)
         * {
         *      foreach list item
         *      {
         *          move sameAmount;
         *      }
         * }
         */

    }
}
