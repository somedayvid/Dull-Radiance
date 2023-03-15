using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dull_Radiance
{
    /// <summary>
    /// Controls the positioning, clicking, and sizing of buttons
    /// </summary>
    internal class Button //: ICollideAndDraw
    {
        //fields
        private int xPos;
        private int yPos;
        private int width;
        private int height;
        private Texture2D buttonHover;
        private Texture2D buttonUnhover;
        private Texture2D buttonTexture;
        private Rectangle buttonRect;
        private int windowHeight;
        private int windowWidth;
        private MouseState mState;
        private int unhoverY;
        private int hoverY;
        private SpriteFont font;

        /// <summary>
        /// Initializes the position of the button and other variables used in determining 
        /// button position
        /// </summary>
        /// <param name="xPos">X position relative to window width</param>
        /// <param name="yPos">Y position relative to window height</param>
        /// <param name="buttonHover">What the button looks like when clicked</param>
        /// <param name="buttonUnhover">What the button looks like when not clicked</param>
        /// <param name="graphicsDevice">Used in determining width and height of buttons</param>
        /// <param name="font">Font used for the button text</param>
        //TODO CLEAN THIS UP ITS DISGUSTING FROM DAVID TO DAVID
        public Button(int xPos, int yPos, Texture2D buttonHover, Texture2D buttonUnhover, GraphicsDeviceManager graphicsDevice, SpriteFont font)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.buttonHover = buttonHover;
            this.buttonUnhover = buttonUnhover;
            this.buttonTexture = buttonUnhover;
            windowHeight = graphicsDevice.PreferredBackBufferHeight;
            windowWidth = graphicsDevice.PreferredBackBufferWidth;
            width = windowWidth/5;
            height = windowHeight/8;
            this.hoverY = yPos + windowHeight/72;
            this.unhoverY = yPos;
            this.font = font;
            this.buttonRect = new Rectangle(xPos, unhoverY, width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void ButtonsUpdate(GameTime gameTime)
        {
            mState = Mouse.GetState();
        }

        /// <summary>
        /// Checks if the button is intersecting with the mouse and has been left clicked
        /// </summary>
        /// <returns>True or false depending on mouse position and click</returns>
        public bool Click()
        {

            if (ButtonIntersects() && mState.LeftButton == ButtonState.Pressed)
            {
                
                //this.height = windowHeight/9;
                //this.buttonRect.Y = hoverY;
                //this.buttonTexture = this.buttonHover;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the mouse is intersecting with the button
        /// </summary>
        /// <returns>True or false depending on mouse position</returns>
        public bool ButtonIntersects()
        {
            if (buttonRect.X <= mState.X && (buttonRect.X + width) >= mState.X
                && buttonRect.Y <= mState.Y && buttonRect.Y + height >= mState.Y)
            {
                this.height = windowHeight/9;
                this.buttonRect.Y = hoverY;
                this.buttonTexture = this.buttonHover;
                return true;
            }
            else
            {
                this.height = windowHeight/8;
                this.buttonRect.Y = unhoverY;
                this.buttonTexture = this.buttonUnhover;
            }
                return false;
        }

        /// <summary>
        /// Draws the button and the text that accompanies the button
        /// </summary>
        /// <param name="sb">Spritebatch to draw</param>
        /// <param name="buttonText">Text that describes button function</param>
        public void DrawButton(SpriteBatch sb, string buttonText)
        {
            sb.Draw(
                this.buttonTexture,
                buttonRect,
                Color.White);

            sb.DrawString(font,
                buttonText,
                new Vector2(this.xPos + width/4, this.yPos + height/2),
                Color.White);
        }

    }
}
