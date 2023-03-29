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
        }

        public void Update(GameTime gameTime)
        {

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
    }
}
