using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.WebRequestMethods;

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
        //private SpriteBatch _spriteBatch;

        //Properties
        public WallType[,] Map
        {
            get { return map; }
        }

        public Rectangle[,] Rectangles
        {
            get;
            private set;
        }

        //Constructor
        public MapCreator()
        {
            //Map Sizing
            doubleMap = new double[30, 52];
            map = new WallType[30, 52];

            //Window Dimensions


            //Load Map
            LoadMap();

            //ConvertMap to enum map
            ConvertMap(doubleMap);


            //Draw Objects to screen
            //Draw(_spriteBatch);
        }

        /// <summary>
        /// Using try|catch to safely load in map
        /// </summary>
        public void LoadMap()
        {
            try
            {
                //Initialize Reader
                readMap = new StreamReader("../../../StartingMapCoordinates.txt");

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
                while (lineOfText != null)
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

                    lineOfText = readMap.ReadLine();
                }
            }
            catch (Exception error)
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

        /// <summary>
        /// Converts doublemap to enum map
        /// </summary>
        /// <param name="doubleMap">2D array map of doubles ot convert</param>
        public void ConvertMap(double[,] doubleMap)
        {
            //Variables
            int row;
            int col;
            double tile;

            //Convert doubleMap into Enum Map
            for (row = 0; row < 30; row++)
            {
                for (col = 0; col < 52; col++)
                {
                    tile = doubleMap[row, col];
                    switch (tile)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_sb"></param>
        /// <param name="texture"></param>
        /// <param name="map"></param>
        public void Draw(SpriteBatch _sb, Texture2D[] texture, WallType[,] map)
        {
            for (int row = 0; row < 30; row++)
            {
                for (int col = 0; col < 52; col++)
                {
                    switch (map[row, col])
                    {
                        case WallType.TLCorner:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.BLCorner:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.TRCorner:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.BRCorner:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.Floor:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.HoriWall:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.VertWall:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.LMWall:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.TMWall:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.BMWall:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.RMWall:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.HIF:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.SawBlade:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.MBGoal:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.MBDoor:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.Lore:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                        case WallType.LightSwitch:
                            _sb.Draw(texture[0], new Vector2(col * 32, row * 32), Color.White);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerX"></param>
        /// <param name="playerY"></param>
        public void DrawPlayerScreen(int playerX, int playerY)
        {

        }


        public Rectangle[,] CreateMapRectangles(int windowWidth, int windowHeight, int tileSize, WallType[,] map)
        {
            int mapWidth = map.GetLength(1);
            int mapHeight = map.GetLength(0);

            Rectangle[,] rectangles = new Rectangle[mapHeight, mapWidth];

            for (int row = 0; row < mapHeight; row++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    int x = col * tileSize;
                    int y = row * tileSize;
                    int width = tileSize;
                    int height = tileSize;

                    // If the current point on the map is a wall or interactable object,
                    // create a rectangle that represents its position and size
                    if (map[row, col] != WallType.Floor)
                    {
                        rectangles[row, col] = new Rectangle(x, y, width, height);
                    }
                }
            }

            return rectangles;
        }
    }
}