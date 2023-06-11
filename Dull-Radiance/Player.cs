using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//for animating movement
public enum AnimationState
{
    FaceLeft,
    FaceRight,
    WalkLeft,
    WalkRight
}

//used in determining up and down directino
public enum CurrentDirection
{
    Left,
    Right 
}
//for detecting up and down movement
public enum UpNDown
{
    WalkUp,
    WalkDown,
    Still
}

namespace Dull_Radiance
{
    /// <summary>
    /// The main player class
    /// </summary>
    internal class Player
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

        // Animation data
        private AnimationState currentState;
        private UpNDown upDownState;
        private CurrentDirection direct;
        private int playerCurrentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;
        private int widthOfSingleSprite;
        private int numSpritesInsheet;

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

        public int PlayerSpeed
        {
            set
            {
                playerSpeed = value;
            }
        }

        //Constructors
        /// <summary>
        /// Initializes the player's initial position and starting stats
        /// </summary>
        /// <param name="playerTexture">The texture of the player character</param>
        public Player(Texture2D playerTexture, PlayerHealth hearts, int windowWidth, int windowHeight, int playerSpeed)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            //Incredibly important rect deciding how far the player can move before getting teleported back
            theBox = new Rectangle(820, 370, 155, 100);

            playerRect = new Rectangle(windowWidth/2 - width/2, windowHeight/2 - height/2, width, height);

            playerSpeed = this.playerSpeed; //6 is original speed
            height = 190;
            width = 190;
            this.playerTexture = playerTexture;

            //animation stuff
            fps = 10.0;
            secondsPerFrame = 1.0 /fps;
            timeCounter = 0;
            playerCurrentFrame = 1;

            numSpritesInsheet = 5;

            widthOfSingleSprite = playerTexture.Width/ numSpritesInsheet;

            currentState = AnimationState.FaceLeft;
            upDownState = UpNDown.Still;

        }

        //Methods
        /// <summary>
        /// Updates the player's movement every frame in update
        /// </summary>
        /// <param name="gameTime">Gametime</param> //TODO i dont actually know what to put in here
        public void Update(GameTime gameTime)   //neccesary? -asking
        {
            Movement(gameTime);
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
            //draws the player's left and right movement
            switch (currentState)
            {
                case AnimationState.FaceLeft:
                    if (upDownState== UpNDown.Still)
                    {
                        DrawPlayerStanding(SpriteEffects.FlipHorizontally, sb);
                    }
                    break;
                case AnimationState.FaceRight:
                    if (upDownState == UpNDown.Still)
                    {
                        DrawPlayerStanding(SpriteEffects.None, sb);
                    }
                    break;
                case AnimationState.WalkLeft:
                    DrawPlayerMoving(SpriteEffects.FlipHorizontally, sb);
                    break;
                case AnimationState.WalkRight:
                    DrawPlayerMoving(SpriteEffects.None, sb);
                    break;
            }
            //draws the player's up and down movement
            switch(upDownState)
            {
                case UpNDown.WalkUp:
                    if (KBState.IsKeyDown(Keys.W) && direct == CurrentDirection.Left)
                    {
                        DrawPlayerMoving(SpriteEffects.FlipHorizontally, sb);
                    }
                    else if (KBState.IsKeyDown(Keys.W) && direct == CurrentDirection.Right)
                    {
                        DrawPlayerMoving(SpriteEffects.None, sb);
                    }
                    break;
                case UpNDown.WalkDown:
                    if (KBState.IsKeyDown(Keys.S) && direct == CurrentDirection.Left)
                    {
                        DrawPlayerMoving(SpriteEffects.FlipHorizontally, sb);
                    }
                    else if (KBState.IsKeyDown(Keys.S) && direct == CurrentDirection.Right)
                    {
                        DrawPlayerMoving(SpriteEffects.None, sb);
                    }
                    break;
            }
        }

        /// <summary>
        /// Players Movement using WASD and Arrow Keys
        /// </summary>
        public void Movement(GameTime gameTime)
        {
            //KBState
            KBState = Keyboard.GetState();

            //Movement
            if (KBState.IsKeyDown(Keys.A))
            {
                playerRect.X -= playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.D))
            {
                playerRect.X += playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.W))
            {
                playerRect.Y -= playerSpeed;
            }
            if (KBState.IsKeyDown(Keys.S))
            {
                playerRect.Y += playerSpeed;
            }
            switch (currentState)
            {
                //faceleft -> face right or walk left
                case AnimationState.FaceLeft:
                    if (KBState.IsKeyDown(Keys.D))
                    {
                        currentState = AnimationState.FaceRight;
                    }
                    else if (KBState.IsKeyDown(Keys.A))
                    {
                        currentState = AnimationState.WalkLeft;
                    }
                    break;
                //walk left -> face left 
                case AnimationState.WalkLeft:
                    if (KBState.IsKeyUp(Keys.A))
                    {
                        currentState = AnimationState.FaceLeft;
                    }
                    break;
                //face right -> face left or walk right
                case AnimationState.FaceRight:
                    if (KBState.IsKeyDown(Keys.A))
                    {
                        currentState = AnimationState.FaceLeft;
                    }
                    else if (KBState.IsKeyDown(Keys.D))
                    {
                        currentState = AnimationState.WalkRight;
                    }
                    break;
                //walk right -> face left
                case AnimationState.WalkRight:
                    if (KBState.IsKeyUp(Keys.D))
                    {
                        currentState = AnimationState.FaceRight;
                    }
                    break;
            }
            //fsm for up and down movement and standing still
            switch (upDownState)
            {
                case UpNDown.WalkUp:
                    if (KBState.IsKeyDown(Keys.S))
                    {
                        upDownState = UpNDown.WalkDown;
                    }
                    else if (KBState.IsKeyUp(Keys.S) && (KBState.IsKeyUp(Keys.W))) 
                    {
                        upDownState = UpNDown.Still;
                    }
                    break;
                case UpNDown.WalkDown:
                    if (KBState.IsKeyDown(Keys.W))
                    {
                        upDownState = UpNDown.WalkUp;
                    }
                    else if (KBState.IsKeyUp(Keys.W) && KBState.IsKeyUp(Keys.S))
                    {
                        upDownState = UpNDown.Still;
                    }
                    break;
                case UpNDown.Still:
                    if (KBState.IsKeyDown(Keys.W))
                    {
                        upDownState = UpNDown.WalkUp;
                    }
                    else if (KBState.IsKeyDown(Keys.S))
                    {
                        upDownState = UpNDown.WalkDown;
                    }
                    break;

            }
            //used in deciding direction for player up and down movement
            if ((currentState == AnimationState.WalkLeft) || (currentState == AnimationState.FaceLeft))
            {
                direct = CurrentDirection.Left;
            }
            else if ((currentState == AnimationState.WalkRight) || (currentState == AnimationState.FaceRight))
            {
                direct = CurrentDirection.Right;
            }
            // Always update the animation
            UpdateAnimation(gameTime);

        }

        /// <summary>
        /// Full implimentation of THE BOX
        /// </summary>
        /// <returns>the direction that the player went in the tile</returns>
        public string CheckPosition()
        {
            //Left
            if (playerRect.X < theBox.X + 10)
            {
                playerRect.X = theBox.X + theBox.Width - 40;
                return "left";
            }
            //Up
            else if (playerRect.Y < theBox.Y + 5)
            {
                playerRect.Y = theBox.Y + theBox.Height - 40;
                return "up";
            }
            //Right
            else if (playerRect.X > theBox.X + theBox.Width - 10)
            {
                playerRect.X = theBox.X + 40;
                return "right";
            }
            //Down
            else if (playerRect.Y > theBox.Y + theBox.Height + 5)
            {
                playerRect.Y = theBox.Y + 40;
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

        /// <summary>
        /// Updates the current frame of the animation that is active
        /// </summary>
        /// <param name="gameTime">IN game time</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            //checks if ready to go to next animation
            if (timeCounter >= secondsPerFrame)
            {
                // change frames
                playerCurrentFrame++;
                if (playerCurrentFrame >= 4)
                {
                    playerCurrentFrame = 1;
                }

                // reset timer
                timeCounter -= secondsPerFrame;
            }
        }

        /// <summary>
        /// Draws the player running
        /// </summary>
        /// <param name="flip">Effect for flipping the player horizontally</param>
        /// <param name="sb">For drawing</param>
        public void DrawPlayerMoving(SpriteEffects flip, SpriteBatch sb)
        {
            sb.Draw(
                playerTexture,                                   // Whole sprite sheet
                new Vector2(playerRect.X, playerRect.Y),         // Position of the Mario sprite
                new Rectangle(                                  // Which portion of the sheet is drawn:
                    playerCurrentFrame * widthOfSingleSprite,   // - Left edge
                    0,                                          // - Top of sprite sheet
                    widthOfSingleSprite,                        // - Width 
                    playerTexture.Height),                       // - Height
                Color.White,                                    // No change in color
                0.0f,                                           // No rotation
                Vector2.Zero,                                   // Start origin at (0, 0) of sprite sheet 
                1.0f,                                           // Scale
                flip,                                           // Flip it horizontally or vertically?    
                0.0f);                                          // Layer depth
        }

        /// <summary>
        /// Draws the player just standing and not moving
        /// </summary>
        /// <param name="flip">Effect for flipping the player horizontally</param>
        /// <param name="sb">For drawing</param>
        private void DrawPlayerStanding(SpriteEffects flip, SpriteBatch sb)
        { 
            sb.Draw(
                playerTexture,                                   // Whole sprite sheet
                new Vector2(playerRect.X, playerRect.Y),                                  // Position of the Mario sprite
                new Rectangle(                                  // Which portion of the sheet is drawn:
                    0,                                          // - Left edge
                    0,                                          // - Top of sprite sheet
                    widthOfSingleSprite,                        // - Width 
                    playerTexture.Height),                       // - Height
                Color.White,                                    // No change in color
                0.0f,                                           // No rotation
                Vector2.Zero,                                   // Start origin at (0, 0) of sprite sheet 
                1.0f,                                           // Scale
                flip,                                           // Flip it horizontally or vertically?    
                0.0f);                                          // Layer depth
        }
    }
}
