using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dull_Radiance
{
    internal class Inventory
    {
        /// <summary>
        /// A custom list with limited functionality to add and remove items 
        /// </summary>
        private Collectibles[] inventory;
        private int maxCount;

        public int MaxCount
        {
            get { return maxCount; }
        }

        public Collectibles this[int index]
        {
            get { return inventory[index]; }
        }
        
        /// <summary>
        /// Initializes the array 
        /// </summary>
        public Inventory()
        {
            inventory = new Collectibles[5];
            maxCount = 5;
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
    }
}
