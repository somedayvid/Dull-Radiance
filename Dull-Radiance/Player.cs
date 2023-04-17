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
    internal class Player //: ICollideAndDraw
    {
        public event GameReset OnGameReset;
        //Fields
        private int windowWidth;
        private int windowHeight;
        private int width;
        private int height;

        //Player Creation Properties
        private Texture2D playerTexture;
        private Rectangle playerRect;

        private Rectangle theBox;

        //gameplay fields
        private int playerSpeed;
        private KeyboardState KBState;

        //private PlayerHealth hearts;

        
        //Properties
        /// <summary>
        /// Returns player's X position
        /// </summary>
        public int X
        {
            get { return playerRect.X; }
            set { playerRect.X = value; }
        }

        /// <summary>
        /// Returns player's Y position
        /// </summary>
        public int Y
        {
            get { return playerRect.Y; }
            set { playerRect.Y = value; }
           
        }

        public Rectangle PlayerRect
        {
            get { return playerRect; }
        }

        //Constructors
        /// <summary>
        /// Initializes the player's initial position and starting stats
        /// </summary>
        /// <param name="playerTexture">The texture of the player character</param>
        public Player(Texture2D playerTexture, PlayerHealth hearts, int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            //height = windowHeight/60;
            //width = windowWidth/34;

            //Incredibly important rect deciding how far the player can move before getting teleported back
            theBox = new Rectangle(740, 300, 300, 300);

            playerRect = new Rectangle(windowWidth/2 - width/2, windowHeight/2 - height/2, width, height);

            playerSpeed = 5;
            height = 280;
            width = 280;
            this.playerTexture = playerTexture;

            //playerRect = new Rectangle(960, 540, width, height);

            // Set initial bounds for player object
            //bounds = new Rectangle(0, 0, 320, 320);
        }

        //Methods
        /// <summary>
        /// Updates the player's movement every frame in update
        /// </summary>
        /// <param name="gameTime">Gametime</param> //TODO i dont actually know what to put in here
        public void Update(GameTime gameTime)   //neccesary? -asking
        {
            Movement();
        }

        //public void PlayerHeal()
        //{
        //    OnHeal();
        //}

        /// <summary>
        /// Resets the player's stats to base game start stats and alerts heart UI to reset
        /// </summary>
        public void Reset()
        {
            OnGameReset();
            playerRect = new Rectangle(
                windowWidth / 2 - (width / 2), 
                windowHeight / 2 - (height / 2), 
                width, height);
        }

        /// <summary>
        /// Draw player onto screen
        /// </summary>
        /// <param name="sb">SpriteBatch sb</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                playerTexture,
                playerRect,
                Color.White);
        }

        /// <summary>
        /// Players Movement using WASD and Arrow Keys
        /// </summary>
        public void Movement()
        {
            //KBState
            KBState = Keyboard.GetState();

            //Movement
            if (KBState.IsKeyDown(Keys.Left) || KBState.IsKeyDown(Keys.Left))
            {
                playerRect.X -= playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.Right) || KBState.IsKeyDown(Keys.Right))
            {
                playerRect.X += playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.Up) || KBState.IsKeyDown(Keys.Up))
            {
                playerRect.Y -= playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.Down) || KBState.IsKeyDown(Keys.Down))
            {
                playerRect.Y += playerSpeed;
            }
        }

        /// <summary>
        /// Full implimentation of THE BOX
        /// </summary>
        /// <returns>the direction that the player went in the tile</returns>
        public string CheckPosition()
        {
            //Left
            if (playerRect.X < theBox.X)
            {
                playerRect.X = theBox.X + theBox.Width - 20;
                return "left";
            }
            //Up
            else if (playerRect.Y < theBox.Y)
            {
                playerRect.Y = theBox.Y + theBox.Height - 20;
                return "up";
            }
            //Right
            else if (playerRect.X > theBox.X + theBox.Width)
            {
                playerRect.X = theBox.X + 20;
                return "right";
            }
            //Down
            else if (playerRect.Y > theBox.Y + theBox.Height)
            {
                playerRect.Y = theBox.Y + 20;
                return "down";
            }

            return "";
        }

        // Method to update player object's bounds based on current position
        public void UpdateBounds(int x, int y)
        {
            playerRect.X = x;
            playerRect.Y = y;
        }
    }
}
