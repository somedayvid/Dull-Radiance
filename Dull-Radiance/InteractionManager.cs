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
    /// Handles the interactions between several other classes in world
    /// </summary>
    internal class InteractionManager
    {
        //fields
        private Dictionary<string, Collectibles> keyToDoorMatch; //TODO currently no door class so string placeholder
        private Inventory inventory;

        /// <summary>
        /// Adds the possible doors to the dictionary and their respective keys
        /// </summary>
        /// <param name="inventory">Acesses the keys in the inventory</param>
        /// <param name="inWorldKeys">Matches the keys to be found in game to their doors</param>
        public InteractionManager(Inventory inventory, List<Collectibles> inWorldKeys /*doorlist*/) 
        { 
            keyToDoorMatch= new Dictionary<string, Collectibles>();
            this.inventory = inventory;

            keyToDoorMatch.Add("BeginningDoor", inWorldKeys[0]);
            keyToDoorMatch.Add("RedDoor", inWorldKeys[1]);
            keyToDoorMatch.Add("BlueDoor", inWorldKeys[2]);
            keyToDoorMatch.Add("GreenDoor", inWorldKeys[3]);
        }

        /// <summary>
        /// A check for whenever a player approaches a door to see if they have the correct key
        /// for the door to open it
        /// </summary>
        /// <param name="door">The door that is being approached</param>
        public void CheckForKey(string door) //should check for door tile
        {
            if (inventory.Contains(keyToDoorMatch[door]))
            {
                //door tile set to open
            }
        }
    }
}
