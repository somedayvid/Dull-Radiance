using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Dull_Radiance
{
    /// <summary>
    /// A custom list with limited functionality to add and remove items 
    /// </summary>
    internal class Inventory
    {
        //fields
        private List<Collectibles> inventory;
        private int maxCount;
        private int count;
        
        /// <summary>
        /// Initializes a list representing the player's inventory 
        /// </summary>
        public Inventory(List<Collectibles> collectiblesList)
        {
            inventory = new List<Collectibles>();
            maxCount = 5;
        }

        /// <summary> 
        /// Adds at first avaliable open index
        /// </summary>
        /// <param name="item">Item to add to the inventory</param>
        public void Add(Collectibles item)
        {
            //    for (int i = 0; i < maxCount; i++)
            //    {
            //        if (inventory[i] == null)
            //        {
            //            inventory[i] = item;
            //            break;
            //        }
            //    }
            count++;
            if (count <= maxCount)
            {
                inventory.Add(item);
            }
        }

        /// <summary>
        /// Method which allows other classes to check if the inventory contains an item
        /// </summary>
        /// <param name="item">A specific item of the collectibles class which pairs with a door
        /// in a dictionary</param>
        /// <returns>A boolean on whether the item is within the list or not</returns>
        public bool Contains(Collectibles item)
        {
            if(inventory.Contains(item)) 
                return true;
            else
                return false;
        }

        /// <summary>
        /// Removes item at certain index
        /// </summary>
        /// <param name="index">Index of item to remove from inventory</param>
        public void Remove(int index)
        {
            inventory[index] = null;
        }

        /// <summary>
        /// Draws the items in the inventory to the screen
        /// </summary>
        /// <param name="sb">Uses monogame library's spritebatch</param>
        /// <param name="windowWidth">The width of the window</param>
        /// <param name="windowHeight">The height of the window</param>
        public void Draw(SpriteBatch sb, int windowWidth, int windowHeight)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i] != null)
                {
                    sb.Draw(
                      inventory[i].KeyTexture,
                      new Rectangle(i * windowWidth/32, windowHeight/18, windowWidth/32, 
                        windowHeight/18),
                      inventory[i].Color);
                }
            }
        }
    }
}
