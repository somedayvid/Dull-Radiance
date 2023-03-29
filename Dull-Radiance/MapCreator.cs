using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dull_Radiance
{
    /// <summary>
    /// Non Collectible Wall|Floor|Intereactible types
    /// </summary>
    public enum WallType
    {
        TLCorner,       //Top Left Corner
        BLCorner,       //Bottom Left Corner
        TRCorner,       //Top Right Corner
        BRCorner,       //Bottom Right Corner
        Floor,          //Floor
        HoriWall,       //Horiozontal Wall
        VertWall,       //Vertical Wall
        LMWall,         //Left Mid Wall
        TMWall,         //Top Mid Wall
        BMWall,         //Bottom Mid Wall
        RMWall,         //Right Mid Wall
        HIF,            //Hole In Floor
        SawBlade,       //Saw Blade
        MBGoal,         //Moving Block Goal
        MBDoor,         //Moving Block Door
        Lore            //Lore
    }

    /// <summary>
    /// Collectible|Movable Type of Object
    /// </summary>
    public enum CollectibleType
    {
        GKey,           //Green Key
        GDoor,          //Green Door
        YKey,           //Yellow Key
        YDoor,          //Yellow Door
        MB              //Moving Block
    }

    /// <summary>
    /// Class focused around creating the 2D Array map for the player
    /// </summary>
    internal class MapCreator
    {
        //Variables
        private double[,] map;
        StreamReader readMap;
        string lineOfText;

        //Properties


        //Constructor
        public MapCreator()
        {
            //Map Size
            map = new double[52, 30];

            //Load Map
            LoadMap();
        }

        /// <summary>
        /// Using try|catch to safely load in map
        /// </summary>
        public void LoadMap()
        {
            try
            {
                //Initialize Reader
                readMap = new StreamReader("../../StartingMapCoordinates");

                //Loop through lines until reaching data
                lineOfText = readMap.ReadLine();
                while (lineOfText[0] == '-')
                {
                    lineOfText = readMap.ReadLine();
                }

                //Loop data into 2d Array
                int row = 0;
                int col = 0;

                //Run through lines in txt
                while((lineOfText = readMap.ReadLine()) != null)
                {
                    //Turn line into array of strings - Parse to double
                    string[] splitPrint = lineOfText.Split('|');
                    double[] parsedPrint = new double[splitPrint.Length];

                    //iterate through coloumn
                    for (int i = 0; i < splitPrint.Length; i++)
                    {
                        //Parse string to double for 2D Array
                        parsedPrint[i] = double.Parse(splitPrint[i]);

                        //Add double value into map
                        map[row, i] = parsedPrint[i];
                    }

                    //Move down one row
                    row++;
                }
            }
            catch(Exception error)
            {
                Console.WriteLine("The error is: " + error);
            }
            finally
            {
                //Check for readMap existing to close
                if (readMap != null)
                {
                    readMap.Close();
                }
            }
        }
    }
}
