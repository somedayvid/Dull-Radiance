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
    /// The main player class
    /// </summary>
    internal class Player
    {
        //Variables
        public KeyboardState KBState;


        /// <summary>
        /// Players Movement using WASD and Arrow Keys
        /// </summary>
        public void Movement()
        {
            //KBState
            KBState = Keyboard.GetState();

            //Movement
            if (KBState.IsKeyDown(Keys.A) || KBState.IsKeyDown(Keys.Left))
            {
                //playerRect.X -= 5;
            }
            if (KBState.IsKeyDown(Keys.D) || KBState.IsKeyDown(Keys.Right))
            {
                //playerRect.X += 5;
            }
            if (KBState.IsKeyDown(Keys.W) || KBState.IsKeyDown(Keys.Up))
            {
                //playerRect.Y -= 5;
            }
            if (KBState.IsKeyDown(Keys.S) || KBState.IsKeyDown(Keys.Down))
            {
                //playerRect.Y -= 5;
            }
        }
    }
}
