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
        public Inventory()
        {
            inventory = new List<Collectibles>();
            maxCount = 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, List<Collectibles> collectibleList, KeyboardState first, KeyboardState second)
        {
            MaxCapacity();
        }

        /// <summary> 
        /// Adds at first avaliable open index
        /// </summary>
        /// <param name="item">Item to add to the inventory</param>
        public void Add(Collectibles item)
        {
            count++;
            if(count <= maxCount)
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
        /// Checks if the inventory ever goes over max capacity
        /// </summary>
        /// <returns>Boolean that represents if the inventory has space for more items</returns>
        public bool MaxCapacity()
        {
            if (count >= maxCount)
            {
                count = 5;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Removes item at certain index
        /// </summary>
        /// <param name="index">Index of item to remove from inventory</param>
        public void Remove(Collectibles item)
        {
            inventory.RemoveAt(inventory.IndexOf(item));
            count--;
        }

        /// <summary>
        /// Resets the inventory by dropping the old one and setting the inventory's list as 
        /// an empty one
        /// </summary>
        public void Reset()
        {
            this.inventory = new List<Collectibles>();
            count = 0;
        }

        /// <summary>
        /// Draws a warning to the screen if the inventory is full
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        /// <param name="font"></param>
        public void DrawWarning(SpriteBatch sb, int windowWidth, int windowHeight, SpriteFont font, Player player, List<Collectibles> collectiblesList)
        {
            foreach (Collectibles item in collectiblesList)
            {
                if (this.MaxCapacity() /*&& item.KeyRect.Intersects(player.PlayerRect)*/) //uncomment once can test in world
                {
                    sb.DrawString(
                    font,
                    "I can't carry anymore stuff...",
                    new Vector2(windowWidth / 2 - font.MeasureString("I can't carry anymore stuff...").X / 2, windowHeight / 4),
                    Color.White);
                }
            }
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
                      new Rectangle(i * windowWidth/32, windowHeight/18, 
                        windowWidth/32, windowHeight/18),
                      inventory[i].Color);
                }
            }
        }
    }
}
