using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dull_Radiance
{
    /// <summary>
    /// Draws the various game state screens to the screen
    /// </summary>
    internal class Screens
    {
        //fields
        private Texture2D screenTexture;
        private int windowHeight;
        private int windowWidth;

        /// <summary>
        /// Initializaes the texture of the screen and gets graphics to size texture correctly
        /// </summary>
        /// <param name="screenTexture">What the screen looks like</param>
        /// <param name="graphics">Used to get width and height of screen</param>
        public Screens(Texture2D screenTexture, GraphicsDeviceManager graphics) 
        {
            this.screenTexture = screenTexture;
            windowHeight = graphics.PreferredBackBufferHeight;
            windowWidth = graphics.PreferredBackBufferWidth;
        }    

        /// <summary>
        /// Draws the texture to fit the entire screen
        /// </summary>
        /// <param name="sb">Spritebatch to draw</param>
        public void ScreenDraw(SpriteBatch sb) 
        {
            sb.Draw(
                screenTexture,
                new Rectangle(0, 0, windowWidth, windowHeight),
                Color.White);
        }
    }
}
