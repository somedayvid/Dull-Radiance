﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dull_Radiance //TODO THE COMMENTS AND SUMMARIES
{
    /// <summary>
    /// The player's inventory to hold usable items
    /// </summary>
    internal class Inventory
    {
        //fields 
        private List<Collectibles> inventory;
        private KeyboardState currentKBState;
        private KeyboardState prevKBState;
        private Keys currentKey;
        private Texture2D inventorySlots;
        private int counter;
        private int width;
        private int height;
        private int windowWidth;
        private int windowHeight;
        private Rectangle slotRect;
        private int currentKeyNumber;
        private int count;

        public Collectibles this[int index]
        {
            get 
            {
                return inventory[index];
            }
            set
            {
                inventory[index] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inventorySlots"></param>
        /// <param name="graphics"></param>
        public Inventory(Texture2D inventorySlots, GraphicsDeviceManager graphics)
        {
            windowWidth = graphics.PreferredBackBufferWidth;
            windowHeight = graphics.PreferredBackBufferHeight;

            this.inventorySlots = inventorySlots;
            counter = 0;
            count = 0;
            inventory = new List<Collectibles>(5);

            width = windowWidth / 192;
            height = windowHeight / 108;

            slotRect = new Rectangle(width * counter, windowHeight - height, width, height);  //TODO fix x initial positioning so that the middle slot is in the middle of the screen
        }

        public void Update()
        {
            currentKBState = Keyboard.GetState();

            currentKeyNumber = InventorySelect();

            prevKBState = Keyboard.GetState();
        }

        /// <summary>
        /// Checks the number buttons to see which inventory slot is selected
        /// </summary>
        /// <returns>A number that represents the selected slot</returns>
        public int InventorySelect()
        {
            switch (currentKey)
            {
                case Keys.D1:
                    return 1;
                case Keys.D2:
                    return 2;
                case Keys.D3:
                    return 3;
                case Keys.D4:
                    return 4;
                case Keys.D5:
                    return 5;
                default:
                    return currentKeyNumber;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddToInventory(Collectibles item)
        {
            if (count == 5)
            {
                foreach (Collectibles thing in inventory)
                {
                    if (thing == null)
                    {
                        inventory[inventory.IndexOf(thing)] = thing;
                        break;
                    }
                }
            }
            else
            {
                count++;
                inventory.Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveFromInventory(int index)
        {

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

        /// <summary>
        /// Draws the inventory slots, the selected one will be draw as gray instead of white
        /// </summary>
        /// <param name="sb">Spritebatch to draw</param>
        public void DrawInventory(SpriteBatch sb)
        {
            counter = 0;
            for (int i = 0; i < 5; i++)
            {
                counter++;
                if (currentKeyNumber == i)
                {
                    sb.Draw(
                    inventorySlots,
                    slotRect,
                    Color.DimGray);
                }
                else
                {
                    sb.Draw(
                    inventorySlots,
                    slotRect,
                    Color.White);
                }
            }

        }
    }
}