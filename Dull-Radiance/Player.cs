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

    //TODO move these into a game manager class instead of being in player
    public delegate void DamageTakenDelegate();
    public delegate void GameReset();
    public delegate void Healing();

    /// <summary>
    /// 
    /// </summary>
    internal class Player : ICollideAndDraw
    {
        //event
        //TODO this as well
        public event DamageTakenDelegate OnDamageTaken;
        public event GameReset OnGameReset;
        public event Healing OnHeal; 

        //Fields
        private int windowWidth;
        private int windowHeight;
        private int width;
        private int height;
        private Texture2D playerTexture;


        //gameplay fields
        private int playerSpeed;
        private bool playerAlive;

        //private Texture2D playerTexture;
        private Rectangle playerRect;

        private KeyboardState KBState;

        private PlayerHealth hearts;

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

        //Constructors
        /// <summary>
        /// Initializes the player's initial position and starting stats
        /// </summary>
        /// <param name="playerTexture">The texture of the player character</param>
        public Player(Texture2D playerTexture, PlayerHealth hearts)      //TODO find way to not hard code numbers for initial positioning and size
        {
            //windowWidth = graphics.PreferredBackBufferWidth;
            //windowHeight = graphics.PreferredBackBufferHeight;
            //height = windowHeight/60;
            //width = windowWidth/34;

            //this.playerTexture = playerTexture;

            //playerRect = new Rectangle(windowWidth/2 - width/2, windowHeight/2 - height/2, width, height);

            playerSpeed = 5;
            height = 320;
            width = 320;
            this.playerTexture = playerTexture;

            playerRect = new Rectangle(960, 540, width, height);
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
        /// When the player collides with something hazardous they will take dmg and alert event
        /// </summary>
        //TODO when player collides with enemy or environmental hazard
        public void PlayerCollision()
        {
            //TODO alerts systems maybe get a screen  flash in here or smth 
            OnDamageTaken();
        }

        public void PlayerHeal()
        {
            OnHeal();
        }

        /// <summary>
        /// Resets the player's stats to base game start stats and alerts heart UI to reset
        /// </summary>
        public void Reset()
        {
            OnGameReset();
            playerRect = new Rectangle(960, 540, width, height);
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
