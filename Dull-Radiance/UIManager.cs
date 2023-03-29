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
        private PlayerHearts hearts;
        private Player player;
        private Inventory inventory;
        private KeyboardState currentKBState;
        private KeyboardState previousKBState;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hearts"></param>
        /// <param name="player"></param>
        /// <param name="inventory"></param>
        public UIManager(PlayerHearts hearts, Player player, Inventory inventory)
        {
            this.hearts = hearts;
            this.player = player;
            this.inventory = inventory;

            player.OnDamageTaken += hearts.TakeDamage;
            player.OnGameReset += hearts.Reset;
            player.OnHeal += hearts.Heal;
        }

        public void Update(GameTime gameTime)
        {
            currentKBState = Keyboard.GetState();

            if (SingleKeyPress(currentKBState, previousKBState, Keys.Z))
            {
                hearts.TakeDamage();
            }
            else if(SingleKeyPress(currentKBState, currentKBState, Keys.X))
            {
                hearts.Heal();
            }
            //if(SingleKeyPress(currentKBState, previousKBState, Keys.Space))
            //{
            //    inventory.AddToInventory()
            //}

            previousKBState = Keyboard.GetState();
        }

        /// <summary>
        /// Draws the UI elements to the game screen
        /// </summary>
        /// <param name="sb"></param>
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
