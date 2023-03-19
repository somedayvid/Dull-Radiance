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
    internal class Player : ICollideAndDraw
    {
        //Fields
        private int windowWidth;
        private int windowHeight;
        private int width;
        private int height;
        private Texture2D playerTexture;

        //gameplay fields
        private int playerSpeed;
        private int playerHealth;

        //private Texture2D playerTexture;
        private Rectangle playerRect;

        private KeyboardState KBState;

        //Properties
        /// <summary>
        /// Returns player's X position
        /// </summary>
        public int X
        {
            get { return playerRect.X; }
        }

        /// <summary>
        /// Returns player's Y position
        /// </summary>
        public int Y
        {
            get { return playerRect.Y; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        //Constructors
        public Player(Texture2D playerTexture, GraphicsDeviceManager graphics)
        {
            windowWidth = graphics.PreferredBackBufferWidth;
            windowHeight = graphics.PreferredBackBufferHeight;
            height = windowHeight/60;
            width = windowWidth/34;

            this.playerTexture = playerTexture;
            playerSpeed = 5;
            playerHealth = 5;

            playerRect = new Rectangle(windowWidth/2 - width/2, windowHeight/2 - height/2, width, height);
        }

        //Methods
        public void Update(GameTime gameTime)
        {
            Movement();
        }

        public void TakeDamage()
        {
            //TODO if ex: trap intersect health -= 1, heart die turn die;
        }

        /// <summary>
        /// Check for player intersecting with an object
        /// </summary>
        /// <param name="player">Player player</param>
        public void Intersects(Player player)
        {
            //Intersection code
            //if (this.Intersects(playerRect))
            //{
            //     happening
            //}
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
            if (KBState.IsKeyDown(Keys.A) || KBState.IsKeyDown(Keys.Left))
            {
                playerRect.X -= playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.D) || KBState.IsKeyDown(Keys.Right))
            {
                playerRect.X += playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.W) || KBState.IsKeyDown(Keys.Up))
            {
                playerRect.Y -= playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.S) || KBState.IsKeyDown(Keys.Down))
            {
                playerRect.Y += playerSpeed;
            }
        }
    }
}
