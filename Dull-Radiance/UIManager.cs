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
    /// Consolidates all in game UI elements to the manager
    /// </summary>
    internal class UIManager
    {
        //fields
        private PlayerHealth hearts;
        private Player player;
        private Inventory inventory;

        /// <summary>
        /// Subscribes and initializes methods and variables
        /// </summary>
        /// <param name="hearts">Hearts UI</param>
        /// <param name="player">Player to work with hearts</param>
        /// <param name="inventory">Inventory UI</param>
        public UIManager(PlayerHealth hearts, Player player, Inventory inventory)
        {
            this.hearts = hearts;
            this.player = player;
            this.inventory = inventory;

            player.OnDamageTaken += hearts.TakeDamage;
            player.OnGameReset += hearts.Reset;
            player.OnHeal += hearts.Heal;
        }

        /// <summary>
        /// Checks for inputs for the UI elements
        /// </summary>
        /// <param name="gameTime">Update every frame</param>
        /// <param name="first">First keyboard press</param>
        /// <param name="second">Previous keyboard press</param>
        public void Update(GameTime gameTime, KeyboardState first, KeyboardState second)
        { 
            if (SingleKeyPress(first, second, Keys.Q))
            {
                hearts.TakeDamage();
            }
            if(SingleKeyPress(first, second, Keys.Z))
            {
                hearts.Heal();
            }
            //if(SingleKeyPress(currentKBState, previousKBState, Keys.Space))
            //{
            //    inventory.AddToInventory()
            //}
        }

        /// <summary>
        /// Draws the UI elements to the game screen
        /// </summary>
        /// <param name="sb">Uses monogame library spritebatch</param>
        public void Draw(SpriteBatch sb)
        {
            hearts.Draw(sb);
            inventory.DrawInventory(sb);
        }

        /// <summary>
        /// Single KeyPress Checker
        /// </summary>
        /// <param name="firstPress">KeyboardState firstPress</param>
        /// <param name="secondPress">KeyBoardState secondPress</param>
        /// <returns>bool if key is only active for 1 frame</returns>
        public bool SingleKeyPress(KeyboardState firstPress, KeyboardState secondPress, Keys key)
        {
            if (firstPress.IsKeyDown(key) && secondPress.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
