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
        Lore,           //Lore
        LightSwitch     //Light Switch
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
        private double[,] doubleMap;
        private WallType[,] map;
        StreamReader readMap;
        string lineOfText;

        //Properties


        //Constructor
        public MapCreator()
        {
            //Map Sizing
            doubleMap = new double[30, 52];
            map = new WallType[30,52];

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
                        doubleMap[row, i] = parsedPrint[i];
                    }

                    //Move down one row
                    row++;
                }

                //Reset row|col
                for (row = 0; row < 30; row++)
                {
                    for (col = 0; col < 52; col++)
                    {
                        switch(doubleMap[row,col])
                        {
                            case .1:
                                map[row, col] = WallType.TLCorner;
                                break;
                            case .3:
                                map[row, col] = WallType.BLCorner;
                                break;
                            case 2.1:
                                map[row, col] = WallType.TRCorner;
                                break;
                            case 2.3:
                                map[row, col] = WallType.BRCorner;
                                break;
                            case 1:
                                map[row, col] = WallType.Floor;
                                break;
                            case 3.3:
                                map[row, col] = WallType.HoriWall;
                                break;
                            case 3.2:
                                map[row, col] = WallType.VertWall;
                                break;
                            case .2:
                                map[row, col] = WallType.LMWall;
                                break;
                            case 1.1:
                                map[row, col] = WallType.TMWall;
                                break;
                            case 1.3:
                                map[row, col] = WallType.BMWall;
                                break;
                            case 2.2:
                                map[row, col] = WallType.RMWall;
                                break;
                            case 4.1:
                                map[row, col] = WallType.HIF;
                                break;
                            case 5.1:
                                map[row, col] = WallType.SawBlade;
                                break;
                            case 1.9:
                                map[row, col] = WallType.MBDoor;
                                break;
                            case 3:
                                map[row, col] = WallType.Lore;
                                break;
                            case 3.1:
                                map[row, col] = WallType.LightSwitch;
                                break;
                        }
                    }
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
