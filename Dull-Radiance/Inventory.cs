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
    internal class Inventory
    {
        /// <summary>
        /// A custom list with limited functionality to add and remove items 
        /// </summary>
        private List<Collectibles> inventory;
        private int maxCount;
        private Texture2D inventorySlot;
        private int count;

        public int MaxCount
        {
            get { return maxCount; }
        }

        public Collectibles this[int index]
        {
            get { return inventory[index]; }
        }

        public int Count
        {
            get { return count; }
        }
        
        /// <summary>
        /// Initializes the array 
        /// </summary>
        public Inventory(List<Collectibles> collectiblesList)
        {
            inventory = new List<Collectibles>();
            inventory.Add(collectiblesList[0]);
        } 

        /// <summary> 
        /// Adds at first avaliable open index
        /// </summary>
        /// <param name="item">Item to add to the inventory</param>
        public void Add(Collectibles item)
        {
            for (int i = 0; i < maxCount; i++)
            {
                if (inventory[i] == null)
                {
                    inventory[i] = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Removes item at certain index
        /// </summary>
        /// <param name="index">Index of item to remove from inventory</param>
        public void Remove(int index)
        {
            inventory[index] = null;
        } 

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                sb.Draw(
                    inventory[i].KeyTexture,
                     new Rectangle(i * 110, 0, 100, 200),
                     Color.White);
            }
        }
    }
}
