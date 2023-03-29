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
    internal class Collisions
    {
        //Properties
        /// <summary>
        /// Receive X coordinate
        /// </summary>
        public int X
        {
            get;
        }
        /// <summary>
        /// Receive y coordinate
        /// </summary>
        public int Y
        {
            get;
        }

        //Methods
        /// <summary>
        /// Checks for collision with player
        /// </summary>
        /// <param name="playerRect">Player playerRect Position</param>
        public void Intersects(Player player, MapCreator map)
        {
            //TODO Add collisions with player against tilemap
            //This can be done maybe by getting player positstion and comparing it to map
            //Need to give rectangle values to map tiles to allow for intersection 

            if (player == null)
            {
                throw new Exception("Player doesn't exist");
            }

            if (map == null)
            {
                throw new Exception("Map doesn't exist");
            }
            else
            {
                map.LoadMap();
            }

        }

        //Constructor
        public Collisions()
        {

        }
    }
}