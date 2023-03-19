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
    //TODO make into a struct?
    //TODO or make into abstract class with multiple buttons that inherit from abstract class
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
        private int windowHeight;
        private int windowWidth;
        private SpriteFont font;
        private Texture2D buttonTexture;
        private Rectangle buttonRect;
        private MouseState mState;

        /// <summary>
        /// Initializes the position of the button and other variables used in determining 
        /// button position
        /// </summary>
        /// <param name="xPos">X position relative to window width</param>
        /// <param name="yPos">Y position relative to window height</param>
        /// <param name="buttonTexture">Texture of the button</param>
        /// <param name="graphicsDevice">Used in determining width and height of buttons</param>
        /// <param name="font">Font used for the button text</param>
        public Button(int xPos, int yPos, Texture2D buttonTexture, GraphicsDeviceManager graphicsDevice, SpriteFont font)
        {
            windowHeight = graphicsDevice.PreferredBackBufferHeight;
            windowWidth = graphicsDevice.PreferredBackBufferWidth;

            this.xPos = xPos;
            this.yPos = yPos;
            this.buttonTexture = buttonTexture;
            this.width = windowWidth/5;
            this.height = windowHeight/8;
            this.font = font;

            buttonRect = new Rectangle(this.xPos, this.yPos, this.width, this.height);
        }

        /// <summary>
        /// Initializes position of the button in the center of the screen relative to X position,
        /// and other variables used in determining button position
        /// </summary>
        /// <param name="yPos">Y position relative to window height</param>
        /// <param name="buttonTexture">Texture of the button</param>
        /// <param name="graphicsDevice">Used in determining width and height of buttons</param>
        /// <param name="font">Font used for the button text</param>
        public Button(int yPos, Texture2D buttonTexture, GraphicsDeviceManager graphicsDevice, SpriteFont font)
        {
            windowHeight = graphicsDevice.PreferredBackBufferHeight;
            windowWidth = graphicsDevice.PreferredBackBufferWidth;

            this.yPos = yPos;
            this.buttonTexture = buttonTexture;
            this.width = windowWidth/5;
            this.height = windowHeight/8;
            this.font = font;

            buttonRect = new Rectangle(windowWidth/2 - this.width/2, this.yPos, this.width, this.height);
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
                return true;
            else
                return false;
        }

        /// <summary>
        /// Formula for centering text within a button
        /// </summary>
        /// <param name="buttonText"></param>
        /// <returns></returns>
        public Vector2 CenterText(string buttonText)
        {
            float centerX = this.buttonRect.X + this.buttonRect.Width/2 - 
                this.font.MeasureString(buttonText).X/2;
            float centerY = this.buttonRect.Y + this.buttonRect.Height/2 - 
                (this.font.MeasureString(buttonText).Y/2 + this.font.MeasureString(buttonText).Y/4);
            return new Vector2(centerX, centerY);

        }

        /// <summary>
        /// Draws the button and the text that accompanies the button
        /// </summary>
        /// <param name="sb">Spritebatch to draw</param>
        /// <param name="buttonText">Text that describes button function</param>
        public void DrawButton(SpriteBatch sb, string buttonText)
        {
            Color color = Color.White;

            if(ButtonIntersects())
            {
                color = Color.DimGray;
            }

            sb.Draw(
                this.buttonTexture,
                buttonRect,
                color);
            sb.DrawString(font,
                buttonText,
                CenterText(buttonText),
                Color.White);
        }
    }
}
