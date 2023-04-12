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
using System.Reflection;
using Microsoft.Xna.Framework.Content;

namespace Dull_Radiance
{
    #region ENUMS
    /// <summary>
    /// Enum for all the various wall types
    /// </summary>
    public enum WallType
    {
        TLCorner,
        BLCorner,
        TRCorner,
        BRCorner,
        TopWall,
        BottomWall,
        LeftWall,
        RightWall,
        Floor,
        HorizontalWall,
        VerticalWall,
        BoxWall,
        Door
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

    public enum Direction { Up, Down, Left, Right }
    #endregion
    /// <summary>
    /// Class focused around creating the 2D Array map for the player
    /// </summary>
    internal class MapCreator
    {
        #region Variables
        // Variables
        private StreamReader reader;
        private WallType[,] map;
        private KeyboardState kbState;
        private KeyboardState prevState;
        
        //private Rectangle box;
        
        
        
        List<Vector2> textureLocation;
        private Vector2 playerLocation;
        private int tileSize;

        private int xOffset;
        private int yOffset;
        private Player player;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Rectangle[,] Rectangles
        {
            get;
            private set;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        /// <param name="player"></param>
        public MapCreator(int windowWidth, int windowHeight, Player player)
        { 
            // Load Map
            LoadMap();

            // Tile size and offset value initalization 
            tileSize = 400;
            yOffset = 26;
            xOffset = 0;

            //Create player for collision
            this.player = player;
            playerLocation = new Vector2(2, 27);
            //box = new Rectangle(400, 400, tileSize,tileSize);

            // Initialize textureLocation and start the screen view
            textureLocation = new List<Vector2>();
            StartScreen();
        }
        
        /// <summary>
        /// Load the map
        /// </summary>
        public void LoadMap()
        {
            try
            {
                // Initialize the reader and textLine
                reader = new StreamReader("../../../WallType.txt");
                string textLine = "";
                textLine = reader.ReadLine();

                // Initialize array to always be 30x53
                map = new WallType[30, 53];

                // Skip every data containing '-' before it
                while (textLine[0] == '-')
                {
                    textLine = reader.ReadLine();
                }

                // Keep looping unless textLine is empty
                int c = 0;
                while (textLine != null)
                {
                    // Split the csv data into an array
                    string[] splitData = textLine.Split(',');

                    // Loop through split data
                    for (int i = 0; i < splitData.Length; i++)
                    {
                        // Parse string into int and save to map as WallType enum
                        switch (int.Parse(splitData[i]))
                        {
                            case 1:
                                map[c, i] = WallType.TLCorner;
                                break;
                            case 3:
                                map[c, i] = WallType.TRCorner;
                                break;
                            case 17:
                                map[c, i] = WallType.BLCorner;
                                break;
                            case 19:
                                map[c, i] = WallType.BRCorner;
                                break;
                            case 2:
                                map[c, i] = WallType.TopWall;
                                break;
                            case 18:
                                map[c, i] = WallType.BottomWall;
                                break;
                            case 9:
                                map[c, i] = WallType.LeftWall;
                                break;
                            case 11:
                                map[c, i] = WallType.RightWall;
                                break;
                            case 30:
                                map[c, i] = WallType.Floor;
                                break;
                            case 10:
                                map[c, i] = WallType.HorizontalWall;
                                break;
                            case 34:
                                map[c, i] = WallType.VerticalWall;
                                break;
                            case 15:
                                map[c, i] = WallType.BoxWall;
                                break;
                            case 46:
                                map[c, i] = WallType.Door;
                                break;
                        }
                    }

                    // Increase column and read in new line
                    c++;
                    textLine = reader.ReadLine();
                }
            }
            catch (Exception error)
            {
                // Print error
                Console.WriteLine("Error: " + error.Message);
            }
            finally
            {
                // Check if reader contains data, if so close it
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #region MAP DRAWING
        /// <summary>
        /// Determines which texture to draw base on the enum value
        /// </summary>
        /// <param name="_sb">Spritebatch</param>
        /// <param name="texture">List of texture to use</param>
        public void DrawMap(SpriteBatch _sb, List<Texture2D> texture)
        {
            // Iterate through list of locations and draw
            for (int i = 0; i < textureLocation.Count; i++)
            {
                DrawTile(
                    _sb,
                    texture,
                    new Vector2(
                        textureLocation[i].X,
                        textureLocation[i].Y));
                System.Diagnostics.Debug.WriteLine("Tile " + i + ": " + textureLocation[i]);
            }
        }

        /// <summary>
        /// Draws the tile at given row and column
        /// </summary>
        /// <param name="_sb">Sprite batch</param>
        /// <param name="texture">Texture to draw</param>
        /// <param name="cord">Cordinate of tile</param>
        public void DrawTile(SpriteBatch _sb, List<Texture2D> texture, Vector2 cord)
        {
            // Loop through 2D array
            for (int col = 0; col <= map.GetLength(0); col++)
            {
                for (int row = 0; row <= map.GetLength(1); row++)
                {
                    // Check if given row and column match, if they do, draw corresponding tile
                    if (col == cord.X && row == cord.Y)
                    {
                        // Create rectangle to draw converted to screen display
                        Rectangle rectToDraw = new Rectangle(
                                    (tileSize * row) - (xOffset * tileSize),
                                    (tileSize * col) - (yOffset * tileSize),
                                    tileSize, tileSize);

                        System.Diagnostics.Debug.WriteLine("------------------------------------");
                        System.Diagnostics.Debug.WriteLine(rectToDraw);

                        // Determine the wall type and draw it
                        switch (map[col, row])
                        {
                            case WallType.TLCorner:
                                _sb.Draw(texture[0], rectToDraw, Color.White);
                                break;
                            case WallType.TRCorner:
                                _sb.Draw(texture[1], rectToDraw, Color.White);
                                break;
                            case WallType.BLCorner:
                                _sb.Draw(texture[2], rectToDraw, Color.White);
                                break;
                            case WallType.BRCorner:
                                _sb.Draw(texture[3], rectToDraw, Color.White);
                                break;
                            case WallType.TopWall:
                                _sb.Draw(texture[4], rectToDraw, Color.White);
                                break;
                            case WallType.BottomWall:
                                _sb.Draw(texture[5], rectToDraw, Color.White);
                                break;
                            case WallType.LeftWall:
                                _sb.Draw(texture[6], rectToDraw, Color.White);
                                break;
                            case WallType.RightWall:
                                _sb.Draw(texture[7], rectToDraw, Color.White);
                                break;
                            case WallType.Floor:
                                _sb.Draw(texture[8], rectToDraw, Color.White);
                                break;
                            case WallType.HorizontalWall:
                                _sb.Draw(texture[9], rectToDraw, Color.White);
                                break;
                            case WallType.VerticalWall:
                                _sb.Draw(texture[10], rectToDraw, Color.White);
                                break;
                            case WallType.BoxWall:
                                _sb.Draw(texture[11], rectToDraw, Color.White);
                                break;
                            case WallType.Door:
                                _sb.Draw(texture[12], rectToDraw, Color.White);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the Vector2s of the screen based on the direction
        /// </summary>
        /// <param name="direction">Direction user pressed</param>
        /// <returns>An updated list with screen vector2</returns>
        public List<Vector2> DetermineScreen(Direction direction)
        {
            // Switch based on the 4 cardinal directions
            // Updates the entire list according to direction pressed
            switch (direction)
            {
                case Direction.Up:
                    for (int i = 0; i < textureLocation.Count; i++)
                    {
                        Vector2 temp = textureLocation[i];
                        temp.X -= 1;
                        textureLocation[i] = temp;
                    }
                    // do offset math
                    break;
                case Direction.Down:
                    for (int i = 0; i < textureLocation.Count; i++)
                    {
                        Vector2 temp = textureLocation[i];
                        temp.X += 1;
                        textureLocation[i] = temp;
                    }
                    break;
                case Direction.Left:
                    for (int i = 0; i < textureLocation.Count; i++)
                    {
                        Vector2 temp = textureLocation[i];
                        temp.Y -= 1;
                        textureLocation[i] = temp;
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < textureLocation.Count; i++)
                    {
                        Vector2 temp = textureLocation[i];
                        temp.Y += 1;
                        textureLocation[i] = temp;
                    }
                    break;
            }

            // Return the list
            return textureLocation;
        }

        /// <summary>
        /// Detect if any movement was present, if so, update the screen via DetermineScreen()
        /// </summary>
        public void DetectMovement(Player player)
        {
            // Get the keyboard state
            kbState = Keyboard.GetState();

            if (player.CheckPosition() == 2)
            {
                DetermineScreen(Direction.Up);
                yOffset--;
            }
            if (player.CheckPosition() == 1)
            {
                DetermineScreen(Direction.Left);
                xOffset--;
            }
            if (player.CheckPosition() == 4)
            {
                DetermineScreen(Direction.Down);
                yOffset++;
            }
            if (player.CheckPosition() == 3)
            {
                DetermineScreen(Direction.Right);
                xOffset++;
            }

            
            // Check for single key presses and
            // Call DetermineScreen() with corresponding direction
            if (kbState.IsKeyDown(Keys.W) && prevState.IsKeyUp(Keys.W))
            {
                DetermineScreen(Direction.Up);
                yOffset--;
            }
            else if (kbState.IsKeyDown(Keys.A) && prevState.IsKeyUp(Keys.A))
            {
                DetermineScreen(Direction.Left);
                xOffset--;
            }
            else if (kbState.IsKeyDown(Keys.S) && prevState.IsKeyUp(Keys.S))
            {
                DetermineScreen(Direction.Down);
                yOffset++;
            }
            else if (kbState.IsKeyDown(Keys.D) && prevState.IsKeyUp(Keys.D))
            {
                DetermineScreen(Direction.Right);
                xOffset++;
            }

            // Set previous state to current
            prevState = kbState;
        }

        /// <summary>
        /// Pulls the start screen data from a text file and saves it into a list
        /// </summary>
        public void StartScreen()
        {
            // Initialize texture location
            textureLocation = new List<Vector2>();

            try
            {
                // Initialize the reader and textLine
                reader = new StreamReader("../../../StartCords.txt");
                string lineOfText = "";

                // Loop through saving each line into the list
                while ((lineOfText = reader.ReadLine()!) != null)
                {
                    // Splits data and parse each value into the list
                    string[] splitData = lineOfText.Split(',');
                    textureLocation.Add(new Vector2(int.Parse(splitData[0]), int.Parse(splitData[1])));
                }
            }
            catch (Exception error)
            {
                // Print error
                Console.WriteLine("Error: " + error.Message);
            }
            finally
            {
                // Check if reader contains data, if so close it
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        #endregion

        #region COLLECTABLE DRAWING
        /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_sb"></param>
    /// <param name="texture"></param>
    public void DrawCollectable(SpriteBatch _sb, List<Texture2D> texture)
    {

    }
        */
        #endregion

        public void PlayerMoved()
        {
            if (player.X > 800)
            {
                playerLocation.X++;
                player.X = 200;
            }
            //else if (player.Y > PlayerMoved)
        }

        #region Revitalized Collision

        /*
        public bool CheckCollision(Player player)
        {
            //if (textureLocation[2,0] or [2,2] or [1,1] or [1,3] in map is wall)
            // dont allow map to move
        }
        */

        #endregion











        //Code Graveyard
        #region Inaccurate / Ineffective COLLISION
        /*
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
            for (int col = 0; col < mapHeight; col++)
            {
                for (int row = 0; row < mapWidth; row++)
                {
                    int x = row * tileSize;
                    int y = col * tileSize;
                    int width = tileSize;
                    int height = tileSize;
                    // If the current point on the map is a wall or interactable object,
                    // create a rectangle that represents its position and size
                    if (map[col, row] != WallType.Floor)
                    {
                        rectangles[col, row] = new Rectangle(x, y, width, height);
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
        */
        #endregion
    }
}