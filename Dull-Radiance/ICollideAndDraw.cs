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
    internal interface ICollideAndDraw
    {
        //Properties
        /// <summary>
        /// Receive X coordinate
        /// </summary>
        int X { get; }
        /// <summary>
        /// Receive y coordinate
        /// </summary>
        int Y { get; }

        //Methods
        /// <summary>
        /// Checks for collision with player
        /// </summary>
        /// <param name="playerRect">Player playerRect Position</param>
        void Intersects(Player playerRect);

        /// <summary>
        /// Draw Object to Screen
        /// </summary>
        /// <param name="sb">SpriteBatch sb</param>
        void Draw(SpriteBatch sb);
    }
}
