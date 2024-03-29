﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using System.Drawing;

namespace Dull_Radiance
{
    #region Enumerations
    /// <summary>
    /// Enum for all the various wall types
    /// </summary>
    internal enum WallType
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
        Door, //Should be door
        Void,
        End
    }

    /// <summary>
    /// Carindal direction
    /// </summary>
    internal enum Direction { Up, Down, Left, Right }

    public enum Difficulty { Broken, Normal, Hard, Insane }
    #endregion

    /// <summary>
    /// Creating, loading, and drawing the map and detecting 
    /// collisions for map related activities
    /// </summary>
    internal class MapCreator
    {
        #region Variables
        // Variable field
        private StreamReader reader;

        // Map variables
        private WallType[,] map;
        private List<Vector2> textureLocation;
        private List<Vector2> collisionTile;
        private string mapSetting;
        private bool isGodMode;

        // Tile size and offset value
        private int tileSize;
        private int xOffset;
        private int yOffset;

        // Key variables
        private int keys;
        private bool checkForKey;
        #endregion

        /// <summary>
        /// Default constructor for MapCreator
        /// </summary>
        public MapCreator()
        {
            mapSetting = "";

            // Tile size and offset value initalization 
            tileSize = 220;
            yOffset = 26;
            xOffset = 1;

            //Initialize Inventory

            keys = 0;
            checkForKey = false;
        }

        #region Map
        /// <summary>
        /// Determine the mode the map will load with
        /// </summary>
        /// <param name="difficulty">The difficulty</param>
        /// <param name="isGodMode">If god mode is on or not</param>
        public void DifficultySelection(Difficulty difficulty, bool isGodMode)
        {
            switch (difficulty)
            {
                // Broken map
                case Difficulty.Broken:
                    LoadMap(Difficulty.Broken);
                    if (isGodMode == true)
                    {
                        this.isGodMode = true;
                    }
                    else
                    {
                        this.isGodMode = false;
                    }
                    break;

                // Normal map
                case Difficulty.Normal:
                    LoadMap(Difficulty.Normal);
                    if (isGodMode == true)
                    {
                        this.isGodMode = true;
                    }
                    else
                    {
                        this.isGodMode = false;
                    }
                    break;

                // Hard map
                case Difficulty.Hard:
                    LoadMap(Difficulty.Hard);
                    if (isGodMode == true)
                    {
                        this.isGodMode = true;
                    }
                    else
                    {
                        this.isGodMode = false;
                    }
                    break;

                // Insane map
                case Difficulty.Insane:
                    LoadMap(Difficulty.Insane);
                    if (isGodMode == true)
                    {
                        this.isGodMode = true;
                    }
                    else
                    {
                        this.isGodMode = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Loads the map based on the difficulty
        /// </summary>
        /// <param name="difficulty">How hard the map is</param>
        private void LoadMap(Difficulty difficulty)
        {
            // Determine which map to load based on difficulty
            switch (difficulty)
            {
                case Difficulty.Broken:
                    mapSetting = "WallType.txt";
                    break;
                case Difficulty.Normal:
                    mapSetting = "MazeMap.txt";
                    break;
                case Difficulty.Hard:
                    mapSetting = "MazeMapHard.txt";
                    break;
                case Difficulty.Insane:
                    mapSetting = "MazeMapInsane.txt";
                    break;
            }

            try
            {
                // Initialize the reader and textLine
                reader = new StreamReader($"../../../{mapSetting}");
                string textLine = "";
                textLine = reader.ReadLine();

                // Initialize array to always be 32x55
                map = new WallType[32, 55];

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
                            case 46: //Door
                                map[c, i] = WallType.Door; //Temporary floor
                                break;
                            case 32:
                                map[c, i] = WallType.Void;
                                break;
                            case 47:
                                map[c, i] = WallType.End;
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
                reader?.Close();
            }

            // Initialize textureLocation and collisionTile and load them
            textureLocation = new List<Vector2>();
            collisionTile = new List<Vector2>();
            StartScreen();
            CollideLoad();
        }

        /// <summary>
        /// Pulls the start screen data from a text file and saves it into a list
        /// </summary>
        private void StartScreen()
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
                    textureLocation.Add(new Vector2(
                        int.Parse(splitData[0]),
                        int.Parse(splitData[1])));
                }
            }
            catch (Exception error)
            {
                // Print error
                System.Diagnostics.Debug.WriteLine("Error: " + error.Message);
            }
            finally
            {
                // Check if reader contains data, if so close it
                reader?.Close();
            }
        }

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
            }
        }

        /// <summary>
        /// Draws the tile at given row and column
        /// </summary>
        /// <param name="_sb">Sprite batch</param>
        /// <param name="texture">Texture to draw</param>
        /// <param name="cord">Cordinate of tile</param>
        private void DrawTile(SpriteBatch _sb, List<Texture2D> texture, Vector2 cord)
        {
            // Loop through 2D array
            for (int col = 0; col <= map.GetLength(0); col++)
            {
                for (int row = 0; row <= map.GetLength(1); row++)
                {
                    // Check if given row and column match, if they do, draw corresponding tile
                    if (col == cord.X && row == cord.Y)
                    {
                        // Create rectangle to draw map, convert map to screen cords
                        Rectangle rectToDraw = new Rectangle(
                            (tileSize * row) - (xOffset * tileSize),
                            (tileSize * col) - (yOffset * tileSize),
                            tileSize, tileSize);

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
                            case WallType.Void:
                                _sb.Draw(texture[13], rectToDraw, Color.White);
                                break;
                            case WallType.End:
                                _sb.Draw(texture[14], rectToDraw, Color.White);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resets the map for next play through
        /// </summary>
        public void ResetMap()
        {
            StartScreen();
            CollideLoad();
            yOffset = 26;
            xOffset = 1;
        }
        #endregion

        #region Movement
        /// <summary>
        /// Updates screen and offset values based on direction
        /// </summary>
        public void DetectMovement(string direction)
        {
            // Check direction and update values if collision is false
            switch (direction)
            {
                case "up":
                    if (CheckCollision(Direction.Up) == false)
                    {
                        UpdateScreenTile(Direction.Up);
                        UpdateCollisionTile(Direction.Up);
                        yOffset--;
                    }
                    break;
                case "left":
                    if (CheckCollision(Direction.Left) == false)
                    {
                        UpdateScreenTile(Direction.Left);
                        UpdateCollisionTile(Direction.Left);
                        xOffset--;
                    }
                    break;
                case "down":
                    if (CheckCollision(Direction.Down) == false)
                    {
                        UpdateScreenTile(Direction.Down);
                        UpdateCollisionTile(Direction.Down);
                        yOffset++;
                    }
                    break;
                case "right":
                    if (CheckCollision(Direction.Right) == false)
                    {
                        UpdateScreenTile(Direction.Right);
                        UpdateCollisionTile(Direction.Right);
                        xOffset++;
                    }
                    break;
            }
        }

        /// <summary>
        /// Updates the Vector2s of the screen based on the direction
        /// </summary>
        /// <param name="direction">Direction user pressed</param>
        /// <returns>An updated list with screen vector2</returns>
        private List<Vector2> UpdateScreenTile(Direction direction)
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
        /// Updates the collisionTile list based on direction moved
        /// </summary>
        /// <param name="direction"></param>
        private void UpdateCollisionTile(Direction direction)
        {
            // Switch based on direction
            // Updates the collisionTile  
            switch (direction)
            {
                case Direction.Up:
                    for (int i = 0; i < collisionTile.Count; i++)
                    {
                        Vector2 temp = collisionTile[i];
                        temp.X -= 1;
                        collisionTile[i] = temp;
                    }
                    break;
                case Direction.Down:
                    for (int i = 0; i < collisionTile.Count; i++)
                    {
                        Vector2 temp = collisionTile[i];
                        temp.X += 1;
                        collisionTile[i] = temp;
                    }
                    break;
                case Direction.Left:
                    for (int i = 0; i < collisionTile.Count; i++)
                    {
                        Vector2 temp = collisionTile[i];
                        temp.Y -= 1;
                        collisionTile[i] = temp;
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < collisionTile.Count; i++)
                    {
                        Vector2 temp = collisionTile[i];
                        temp.Y += 1;
                        collisionTile[i] = temp;
                    }
                    break;
            }
        }
        #endregion

        #region Collision
        /// <summary>
        /// Load the collision tiles from a text file
        /// </summary>
        private void CollideLoad()
        {
            // Initialize texture location
            collisionTile = new List<Vector2>();

            try
            {
                // Initialize the reader and textLine
                reader = new StreamReader("../../../CollisionTiles.txt");
                string lineOfText = "";

                // Loop through saving each line into the list
                while ((lineOfText = reader.ReadLine()!) != null)
                {
                    // Splits data and parse each value into the list
                    string[] splitData = lineOfText.Split(',');
                    collisionTile.Add(new Vector2(int.Parse(splitData[0]), int.Parse(splitData[1])));
                }
            }
            catch (Exception error)
            {
                // Print error
                System.Diagnostics.Debug.WriteLine("Error: " + error.Message);
            }
            finally
            {
                // Check if reader contains data, if so close it
                reader?.Close();
            }
        }

        /// <summary>
        /// Checks the direction the player is moving and updates the collision tile
        /// </summary>
        /// <param name="direction">The direction the key is pressed</param>
        /// <returns>True if the tile is not floor</returns>
        private bool CheckCollision(Direction direction)
        {
            // Get the collision tile
            Vector2 Top = collisionTile[0];
            Vector2 Left = collisionTile[1];
            Vector2 Right = collisionTile[2];
            Vector2 Down = collisionTile[3];

            switch (mapSetting)
            {
                #region Hopeful Map
                case "WallType.txt":
                    switch (direction)
                    {
                        case Direction.Up:
                            //Check for player having key
                            if (keys > 0)
                            {
                                checkForKey = true;
                            }

                            if (map[(int)Top.X, (int)Top.Y] == WallType.Floor ||
                            (map[(int)Top.X, (int)Top.Y] == WallType.Door && checkForKey))
                            {
                                return false;
                            }
                            break;
                        case Direction.Down:
                            if (map[(int)Down.X, (int)Down.Y] == WallType.Floor ||
                            (map[(int)Down.X, (int)Down.Y] == WallType.Door && checkForKey))
                            {
                                return false;
                            }
                            break;
                        case Direction.Left:
                            if (map[(int)Left.X, (int)Left.Y] == WallType.Floor ||
                            (map[(int)Left.X, (int)Left.Y] == WallType.Door && checkForKey))
                            {
                                return false;
                            }
                            break;
                        case Direction.Right:
                            if (map[(int)Right.X, (int)Right.Y] == WallType.Floor ||
                            (map[(int)Right.X, (int)Right.Y] == WallType.Door && checkForKey))
                            {
                                return false;
                            }
                            break;
                    }
                    break;
                #endregion
                #region Maze Map
                case "MazeMap.txt":
                case "MazeMapHard.txt":
                case "MazeMapInsane.txt":
                    switch (direction)
                    {
                        // Check the tile above the player
                        case Direction.Up:
                            if (isGodMode == true)
                            {
                                if (map[(int)Top.X, (int)Top.Y] == WallType.Floor ||
                                map[(int)Top.X, (int)Top.Y] == WallType.End ||
                                map[(int)Top.X, (int)Top.Y] == WallType.HorizontalWall)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (map[(int)Top.X, (int)Top.Y] == WallType.Floor ||
                                map[(int)Top.X, (int)Top.Y] == WallType.End)
                                {
                                    return false;
                                }
                            }
                            break;

                        // Check the tile below the player
                        case Direction.Down:
                            if (isGodMode == true)
                            {
                                if (map[(int)Down.X, (int)Down.Y] == WallType.Floor ||
                                map[(int)Down.X, (int)Down.Y] == WallType.End ||
                                map[(int)Down.X, (int)Down.Y] == WallType.HorizontalWall)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (map[(int)Down.X, (int)Down.Y] == WallType.Floor ||
                                map[(int)Down.X, (int)Down.Y] == WallType.End)
                                {
                                    return false;
                                }
                            }
                            break;

                        // Check the tile to the left of the player
                        case Direction.Left:
                            if (isGodMode == true)
                            {
                                if (map[(int)Left.X, (int)Left.Y] == WallType.Floor ||
                                map[(int)Left.X, (int)Left.Y] == WallType.End ||
                                map[(int)Left.X, (int)Left.Y] == WallType.HorizontalWall)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (map[(int)Left.X, (int)Left.Y] == WallType.Floor ||
                                map[(int)Left.X, (int)Left.Y] == WallType.End)
                                {
                                    return false;
                                }
                            }
                            break;

                        // Check the tile to the right of the player
                        case Direction.Right:
                            if (isGodMode == true)
                            {
                                if (map[(int)Right.X, (int)Right.Y] == WallType.Floor ||
                                map[(int)Right.X, (int)Right.Y] == WallType.End ||
                                map[(int)Right.X, (int)Right.Y] == WallType.HorizontalWall)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (map[(int)Right.X, (int)Right.Y] == WallType.Floor ||
                                map[(int)Right.X, (int)Right.Y] == WallType.End)
                                {
                                    return false;
                                }
                            }
                            break;
                    }
                    break;
                    #endregion
            }

            return true;
        }
        #endregion

        #region collectable
        /// <summary>
        /// Checks if the key is collected
        /// </summary>
        /// <returns>True if player is on key</returns>
        public bool IsKeyCollected()
        {
            // Set the end point depending on map
            Vector2 end = mapSetting switch
            {
                "MazeMapInsane.txt" => new Vector2(29, 50),
                _ => new Vector2(6, 3),
            };

            // Check if the player is on the key
            if (textureLocation[22] == end)
                return true;
            return false;
        }

        /// <summary>
        /// Add A key
        /// </summary>
        public void AddKey()
        {
            if (keys < 5)
            {
                keys++;
            }
        }

        /// <summary>
        /// Remove A Key
        /// </summary>
        public void RemoveKey()
        {
            if (keys > 0)
            {
                keys--;
            }
        }
        #endregion
    }
}