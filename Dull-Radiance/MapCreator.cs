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
    #region ENUMS
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
    #endregion

    /// <summary>
    /// Class focused around creating the 2D Array map for the player
    /// </summary>
    internal class MapCreator
    {
        // Variables
        private double[,] doubleMap;
        private WallType[,] map;
        private StreamReader readMap;
        private string lineOfText;
        private Rectangle playerBounds;
        private KeyboardState KbState;
        private KeyboardState PrevState;


        //Variables for Arrays of draw
        private int[,] playerRowLoad;
        private int[,] playerColLoad;
        private int startingX;
        private int startingY;

        /// <summary>
        /// Read only property for the map array
        /// </summary>
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
        public MapCreator(int windowWidth, int windowHeight, Player player)
        {
            //Map Sizing
            doubleMap = new double[30, 52];
            map = new WallType[30, 52];

            Rectangle playerBounds = player.Bounds;

            //Load Map
            LoadMap();

            //ConvertMap to enum map
            ConvertMap(doubleMap);

            //Tilesize to be multiply to change 
            int tileSize = 500;

            //Create rectangles for collision detection
            Rectangles = CreateMapRectangles(windowWidth, windowHeight, tileSize, Map);
        }

        #region MAP READING
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
        #endregion

        #region MAP DRAWING
        /// <summary>
        /// Determines which texture to draw base on the enum value
        /// </summary>
        /// <param name="_sb"></param>
        /// <param name="texture"></param>
        public void DrawMap(SpriteBatch _sb, List<Texture2D> texture)
        {
            int imageWidth = 500;
            int imageHeight = 500;
            int multiplerX = imageWidth;
            int multiplerY = imageHeight;

            for (int row = 0; row < 30; row++)
            {
                for (int col = 0; col < 52; col++)
                {
                    switch (map[row, col])
                    {
                        case WallType.TLCorner:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.BLCorner:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.TRCorner:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.BRCorner:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.Floor:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.HoriWall:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.VertWall:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.LMWall:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.TMWall:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.BMWall:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.RMWall:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.HIF:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.SawBlade:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.MBGoal:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.MBDoor:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.Lore:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                        case WallType.LightSwitch:
                            _sb.Draw(texture[0], new Rectangle(col * multiplerX, row * multiplerY, imageWidth, imageHeight), Color.White);
                            break;
                    }
                }
            }
        }



        /// <summary>
        /// Loading in a specific area of the scren for the player to see
        /// </summary>
        public void StartingPosition()
        {
            //Player starting coord (by Tile)
            playerBounds.X = 2;
            playerBounds.Y = 28;

            //Put info in 2D load arrays
            int[,] playerRowLoad = { { 1, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } };
            int[,] playerColLoad = { { 27, 27, 27 }, { 28, 28, 28 }, { 29, 29, 29 } };

            // Start the map at the bottom left (player spawn)
            // If player presses W move map down (basically reverse)
            // Do NOT move map if player is at edge 

        }

        //----------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Move screen depending on if the player wants ot move
        /// </summary>
        public void MoveScreen()
        {
            //Variables for Arrays of draw
            //private WallType[,] playerRowLoad = new WallType[3, 3];
            //private WallType[,] playerColLoad = new WallType[3, 3];


            //Iniutial Key Press
            KbState = Keyboard.GetState();

            //Initial Corrds
            //Need to press space to start the positioning. Should ultimately tied with start but
            //heres code that can be moved into a new method once we do that
           

            //regular Allowed Movement
            if (playerBounds.X > 1 && playerBounds.X < 51 && playerBounds.Y > 1 && playerBounds.Y > 29 )//&& playercollision is false)
            {
                if (KbState.IsKeyDown(Keys.A))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (CheckPlayerCollisions() == true)
                            {
                                //nothing
                            }
                            else
                            {
                                playerRowLoad[i, j]--;
                            }
                        }
                    }
                }
                if (KbState.IsKeyDown(Keys.D))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            playerRowLoad[i, j]++;
                        }
                    }
                }
                if (KbState.IsKeyDown(Keys.W))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            playerColLoad[i, j]--;
                        }
                    }
                }
                if (KbState.IsKeyDown(Keys.S))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            playerColLoad[i, j]++;
                        }
                    }
                }

            }


            /*Draw Images
             * sort through enum array until recieving correct coordinates
             * 
             */

            //Recieve one frame key press
            PrevState = Keyboard.GetState();
        }

        //----------------------------------------------------------------------------------------------------------------
        #endregion

        #region COLLECTABLE DRAWING
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_sb"></param>
        /// <param name="texture"></param>
        public void DrawCollectable(SpriteBatch _sb, List<Texture2D> texture)
        {

        }
        #endregion

        #region COLLISION
        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        /// <param name="tileSize"></param>
        /// <param name="map"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks player collison
        /// </summary>
        /// <returns>Returns true if they collide</returns>
        public bool CheckPlayerCollisions()
        {
            for (int row = 0; row < Rectangles.GetLength(0); row++)
            {
                for (int col = 0; col < Rectangles.GetLength(1); col++)
                {
                    Rectangle tileBounds = Rectangles[row, col];

                    if (tileBounds != null && tileBounds.Intersects(playerBounds))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion
    }
}