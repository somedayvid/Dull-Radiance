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

        public event DamageTakenDelegate OnDamageTaken;
        public event GameReset OnGameReset;
        //Fields
        private int windowWidth;
        private int windowHeight;
        private int width;
        private int height;
        private Texture2D playerTexture;
        private Rectangle bounds;


        //gameplay fields
        private int playerSpeed;
        //private bool playerAlive;

        //private Texture2D playerTexture;
        private Rectangle playerRect;

        private KeyboardState KBState;

        //private PlayerHealth hearts;

        
        //Properties
        /// <summary>
        /// Returns player's X position
        /// </summary>
        public int X
        {
            get { return playerRect.X; }
            //set { playerRect.X = value; }
        }

        /// <summary>
        /// Returns player's Y position
        /// </summary>
        public int Y
        {
            get { return playerRect.Y; }
            //set { playerRect.Y = value; }
        }
        

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
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

            //this.playerTexture = playerTexture;

            //playerRect = new Rectangle(windowWidth/2 - width/2, windowHeight/2 - height/2, width, height);

            playerSpeed = 5;
            height = 320;
            width = 320;
            this.playerTexture = playerTexture;

            //playerRect = new Rectangle(960, 540, width, height);

            // Set initial bounds for player object
            bounds = new Rectangle(0, 0, 320, 320);
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

        /*
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
        */

        /// <summary>
        /// When the player collides with something hazardous they will take dmg and alert event
        /// </summary>
        //TODO when player collides with enemy or environmental hazard
        public void PlayerCollision()
        {
            //TODO alerts systems maybe get a screen  flash in here or smth 
            OnDamageTaken();
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
            bounds = new Rectangle(
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
                bounds,
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
                bounds.X -= playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.D) || KBState.IsKeyDown(Keys.Right))
            {
                bounds.X += playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.W) || KBState.IsKeyDown(Keys.Up))
            {
                bounds.Y -= playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.S) || KBState.IsKeyDown(Keys.Down))
            {
                bounds.Y += playerSpeed;
            }
        }

        // Method to check collision with another object's bounds
        public bool Intersects(Rectangle otherBounds)
        {
            return bounds.Intersects(otherBounds);
        }

        // Method to update player object's bounds based on current position
        public void UpdateBounds(int x, int y)
        {
            bounds.X = x;
            bounds.Y = y;
        }
    }
}
