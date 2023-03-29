using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dull_Radiance
{
    //TODO HOW TO COUPLE PLAYER CLASS HP TO THIS CLASS 
    /// <summary>
    /// A class which displays the hearts of the player character
    /// </summary>
    internal class PlayerHearts
    {
        //fields
        //private Player player;
        private int healthCounter;
        private bool[] healthbar;
        private Texture2D liveHeart;
        private Texture2D deadHeart;

        /// <summary>
        /// Initializes the heart textures, amt of starting life, and an array to represent total hp  
        /// </summary>
        /// <param name="liveHeart"></param>
        /// <param name="deadHeart"></param>
        public PlayerHearts(Texture2D liveHeart, Texture2D deadHeart)
        {
            this.liveHeart = liveHeart;
            this.deadHeart = deadHeart;

            healthbar = new bool[5] { true, true, true, true, true };
            healthCounter = healthbar.Length;
            //player = new Player(playerTexture);
        }

        //TODO correctly couple take damage and heal with the player's hp
        /// <summary>
        /// Lowers healthcounter on taking damage and sets the heart in the array to false  to
        /// indicate damage taken
        /// </summary>
        public void TakeDamage()
        {
            healthCounter--;
            healthbar[healthCounter] = false;
        }

        /// <summary>
        /// Raises healthcounter on healing damage and sets the heart in the array to true  to
        /// indicate damage healed
        /// </summary>
        public void Heal()
        {
            healthCounter++;
            healthbar[healthCounter] = true;
        }

        /// <summary>
        /// Resets the hearts display to its original stat 
        /// </summary>
        public void Reset()
        {
            healthCounter = 5;
            for (int i = 0; i < 5; i++)
            {
                healthbar[i] = true;
            }
        }

        /// <summary>
        /// Draws each heart in the array depending on whether the entry is true for alive heart
        /// or false for dead heart
        /// </summary>
        /// <param name="sb">Spritebatch to draw</param>
        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < 5; i++)
            {
                if (healthbar[i])
                {
                    sb.Draw(                                   //TODO DO NOT HARDCODE
                        liveHeart,
                        new Rectangle(i * 110, 0, 100, 100),
                        Color.White);
                }
                else
                {
                    sb.Draw(
                        deadHeart,
                        new Rectangle(i * 110, 0, 100, 100),
                        Color.White);
                }
            }
        }
    }
}
