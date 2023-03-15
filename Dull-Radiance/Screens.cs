using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dull_Radiance
{
    internal class Screens
    {
        private Texture2D screenTexture;
        private int windowHeight;
        private int windowWidth;

        public Screens(Texture2D screenTexture, GraphicsDeviceManager graphics) 
        {
            this.screenTexture = screenTexture;
            windowHeight = graphics.PreferredBackBufferHeight;
            windowWidth = graphics.PreferredBackBufferWidth;
        }    

        public void ScreenDraw(SpriteBatch sb) 
        {
            sb.Draw(
                screenTexture,
                new Rectangle(0, 0, windowWidth, windowHeight),
                Color.White);
        }
    }
}
