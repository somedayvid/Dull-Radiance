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
    /// <summary>
    /// A class which displays the hearts of the player character
    /// </summary>
    internal class PlayerHealth
    {
        //fields
        //private Player player;
        private int currentHealth;
        private bool[] healthbar;
        private Texture2D liveHeart;
        private Texture2D deadHeart;
        private int maxHealth;

        /// <summary>
        /// Gets the current health of the player
        /// </summary>
        public int CurrentHealth
        {
            get { return currentHealth; }
            set 
            {
                if(currentHealth < 0)
                {
                    currentHealth = 0;
                }
                else if(currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
            }
        }

        /// <summary>
        /// Initializes the heart textures, amt of starting life, and an array to represent total hp  
        /// </summary>
        /// <param name="liveHeart">Living heart texture</param>
        /// <param name="deadHeart">Dead heart textures</param>
        public PlayerHealth(Texture2D liveHeart, Texture2D deadHeart)
        {
            this.liveHeart = liveHeart;
            this.deadHeart = deadHeart;
            maxHealth = 5;
            currentHealth = maxHealth;

            healthbar = new bool[maxHealth];
        }

        //TODO correctly couple take damage and heal with the player's hp
        /// <summary>
        /// Lowers currentHealth on taking damage and sets the heart in the array to false  to
        /// indicate damage taken
        /// </summary>
        public void TakeDamage()
        {
            try
            {
                currentHealth--;
                healthbar[currentHealth] = false;
            }
            catch(IndexOutOfRangeException)
            {

            }
        }

        /// <summary>
        /// Resets the hearts display to its original stat 
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < maxHealth; i++)
            {
                healthbar[i] = true;
            }
            currentHealth = maxHealth;
        }

        /// <summary>
        /// Draws each heart in the array depending on whether the entry is true for alive heart
        /// or false for dead heart
        /// </summary>
        /// <param name="sb">Spritebatch to draw</param>
        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < healthbar.Length; i++)
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
