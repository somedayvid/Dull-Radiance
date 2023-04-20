﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Schema;

/* Game: Dull Radience
 * Dev Team: Rice Bowls
 * Rice in the Bowl: David Gao, Jason Chen, Thaw Thaw, Will Forbes
 */

//HELLO WILL THIS IS TEMP - PLEASE DELETE THIS COMMENT

namespace Dull_Radiance
{
    #region Enums
    /// <summary>
    /// Player state enumerations used for player action animations
    /// </summary>
    public enum PlayerState
    {
        WalkUp,
        WalkDown,
        WalkLeft,
        WalkRight,
    }
    // Enum which contains the possible state of the game
    public enum GameState
    {
        Title,
        Pause,
        Game,
        Instructions,
        GameOver
    }
    #endregion
    public class Game1 : Game
    {
        #region Variables
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Player/Enemy Related variables
        private Vector2 cord;

        // Keyboard and mouse states
        private KeyboardState kbState;
        private KeyboardState prevkbState;
        private MouseState mState;
        private MouseState prevmState;

        // Call classes
        private PlayerHealth hearts;
        private MapCreator mapMaker;
        private GameState currentState;
        private PlayerState playerState;

        // Map textures
        private List<Texture2D> wallList;
        private Texture2D tlCorner;
        private Texture2D trCorner;
        private Texture2D blCorner;
        private Texture2D brCorner;
        private Texture2D topWall;
        private Texture2D bottomWall;
        private Texture2D leftWall;
        private Texture2D rightWall;
        private Texture2D floor;
        private Texture2D horizontalWall;
        private Texture2D verticalWall;
        private Texture2D boxWall;
        private Texture2D door;
        private List<Collectibles> collectibleList;
        private Texture2D key;
        private Texture2D lights;
        private Collectibles yellowKey;
        private Collectibles greenKey;
        private Texture2D shadow;
        private Texture2D voidTile;
        private Texture2D keyTile;

        // Player texture
        private Player player;
        private Texture2D playerTexture;

        // Menu
        private List<Screens> screensList;
        private Texture2D titleScreen;
        private Texture2D controlsScreen;
        private Texture2D pauseScreen;
        private Texture2D playScreen;
        private Screens title;
        private Screens controls;
        private Screens pause;
        private Screens play;
        private UIManager uiManager;
        private Inventory inventory;

        // Button items
        private List<Button> buttonList;
        private Button startButton;
        private Button quitButton;
        private Button controlsButton;
        private Button resumeButton;
        private Button titleReturn;
        private Button quitButton2;
        private Texture2D buttonTexture;
        private SpriteFont agencyFB;

        // Window size
        private int windowHeight;
        private int windowWidth;

        // UI elements
        private Texture2D aliveHeart;
        private Texture2D deadHeart;
        private int minuteTimer;
        private int secondTimer;
        private int millisecondTimer;
        private string time;
        private bool isSuccessful;
        private int elapsedMinute;
        private int elapsedSecond;
        private int elapsedMillisecond;
        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            currentState = GameState.Title;
        }

        protected override void Initialize()
        {
            //Commments
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //_graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.PreferredBackBufferWidth = 2000;
            _graphics.ApplyChanges();
            windowHeight = _graphics.PreferredBackBufferHeight;
            windowWidth = _graphics.PreferredBackBufferWidth;

            minuteTimer = 5;
            secondTimer = 0;
            millisecondTimer = 0;
            time = "";
            isSuccessful = false;
            elapsedMinute = 0;
            elapsedSecond = 0;
            elapsedMillisecond = 0;
            base.Initialize();

            // buttonList with all buttons
            buttonList = new List<Button>
            {
                startButton,
                controlsButton,
                quitButton,
                quitButton2,
                resumeButton,
                titleReturn
            };

            // Screen list with all screens
            screensList = new List<Screens>
            {
                title,
                controls,
                play,
                pause
            };

            // Intialize 2D map
            mapMaker = new MapCreator(player);
            cord = new Vector2(800, 800);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Ui elements
            shadow = Content.Load<Texture2D>("shadow");
            aliveHeart = Content.Load<Texture2D>("LiveHeart");
            deadHeart = Content.Load<Texture2D>("DeadHeart");
            key = Content.Load<Texture2D>("KEY");
            hearts = new PlayerHealth(aliveHeart, deadHeart);
            yellowKey = new Collectibles(key, Color.White);
            // Add collectables to list of texture2D
            collectibleList = new List<Collectibles>();
            collectibleList.Add(yellowKey);
            collectibleList.Add(yellowKey);
            collectibleList.Add(yellowKey);

            // Player
            playerTexture = Content.Load<Texture2D>("Player");
            player = new Player(playerTexture, hearts, windowWidth, windowHeight);

            // Screens
            titleScreen = Content.Load<Texture2D>("StartMenu");
            controlsScreen = Content.Load<Texture2D>("ControlsScreen");
            pauseScreen = Content.Load<Texture2D>("PauseScreen");
            playScreen = Content.Load<Texture2D>("TempPlayScreen");

            title = new Screens(titleScreen, _graphics);
            controls = new Screens(controlsScreen, _graphics);
            pause = new Screens(pauseScreen, _graphics);
            play = new Screens(playScreen, _graphics);

            // Buttons
            buttonTexture = Content.Load<Texture2D>("BUTTON_UNHOVER");

            // Fonts
            agencyFB = Content.Load<SpriteFont>("Agency FB");

            #region Map Textures
            // Load all the wall types
            tlCorner = Content.Load<Texture2D>("T_TL");
            trCorner = Content.Load<Texture2D>("T_TR");
            blCorner = Content.Load<Texture2D>("T_BL");
            brCorner = Content.Load<Texture2D>("T_BR");
            topWall = Content.Load<Texture2D>("T_T");
            bottomWall = Content.Load<Texture2D>("T_B");
            leftWall = Content.Load<Texture2D>("T_L");
            rightWall = Content.Load<Texture2D>("T_R");
            floor = Content.Load<Texture2D>("T_F");
            horizontalWall = Content.Load<Texture2D>("T_H");
            verticalWall = Content.Load<Texture2D>("T_V");
            boxWall = Content.Load<Texture2D>("T_BOX");
            door = Content.Load<Texture2D>("Door");
            voidTile = Content.Load<Texture2D>("void");
            keyTile = Content.Load<Texture2D>("keyTile");

            // Add walls to list of texture2D
            wallList = new List<Texture2D>
            {
                tlCorner,
                trCorner,
                blCorner,
                brCorner,
                topWall,
                bottomWall,
                leftWall,
                rightWall,
                floor,
                horizontalWall,
                verticalWall,
                boxWall,
                door,
                voidTile,
                keyTile
            };
            #endregion

            inventory = new Inventory();

            uiManager = new UIManager(hearts, player, inventory, collectibleList, _graphics, agencyFB);

            #region Buttons
            // Button initializations
            startButton = new Button(
                windowWidth / 10,                           // Width
                windowHeight / 3 + windowHeight / 36,       // Height
                buttonTexture,                              // Skin
                _graphics,                                  // Gets window height and width 
                agencyFB);                                  // Font

            controlsButton = new Button(
                windowWidth / 10,
                windowHeight / 2 + windowHeight / 36,
                buttonTexture,
                _graphics,
                agencyFB);

            quitButton = new Button(
                windowWidth / 10,
                windowHeight / 2 + windowHeight / 6 + windowHeight / 36,
                buttonTexture,
                _graphics,
                agencyFB);

            resumeButton = new Button(
                windowHeight / 3 + windowHeight / 9,
                buttonTexture,
                _graphics,
                agencyFB);

            titleReturn = new Button(
                windowHeight / 2 + windowHeight / 9,
                buttonTexture,
                _graphics,
                agencyFB);

            quitButton2 = new Button(
                windowHeight / 2 + windowHeight / 6 + windowHeight / 9,
                buttonTexture,
                _graphics,
                agencyFB);
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get keyboard and mouse state
            kbState = Keyboard.GetState();
            mState = Mouse.GetState();

            // Update each button
            foreach (Button button in buttonList)
            {
                button.ButtonsUpdate(gameTime);
            }

            switch (currentState)
            {
                // Title
                case GameState.Title:
                    if (startButton.Click())
                    {
                        player.Reset();
                        ResetSuccess();
                        ResetTimer();
                        mapMaker.ResetMap();
                        currentState = GameState.Game;
                    }
                    if (controlsButton.Click())
                    {
                        currentState = GameState.Instructions;
                    }
                    if (quitButton.Click())
                    {
                        Exit();
                    }
                    break;

                // Instructions
                case GameState.Instructions:
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        currentState = GameState.Title;
                    }
                    break;

                case GameState.Game:
                    // Update countdown timer
                    millisecondTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    if (millisecondTimer < 0)
                    {
                        millisecondTimer = 1000;
                        secondTimer--;
                    }
                    if (secondTimer < 0)
                    {
                        secondTimer = 60;
                        minuteTimer--;
                    }

                    // Update elapsed timer
                    elapsedMillisecond += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedMillisecond >= 1000)
                    {
                        elapsedMillisecond = 0;
                        elapsedSecond++;
                    }
                    if (elapsedSecond > 60)
                    {
                        elapsedSecond = 0;
                        elapsedMinute++;
                    }

                    //player.Movement();
                    uiManager.Update(gameTime, kbState, prevkbState);

                    //Move Player
                    player.Movement();
                    //Player Movement + Tile Movement
                    string result = player.CheckPosition();
                    mapMaker.DetectMovement(result);

                    //Add Key / Remove key
                    if (kbState.IsKeyDown(Keys.D1) && prevkbState.IsKeyUp(Keys.D1))
                    {
                        mapMaker.AddKey();
                    }
                    if (kbState.IsKeyDown(Keys.D5) && prevkbState.IsKeyUp(Keys.D5))
                    {
                        mapMaker.RemoveKey();
                    }

                    //Changed state based on events
                    if (kbState.IsKeyDown(Keys.P))
                    {
                        currentState = GameState.Pause;
                    }
                    if (hearts.CurrentHealth == 0)
                    {
                        currentState = GameState.GameOver;
                        isSuccessful = false;
                    }
                    if (mapMaker.IsKeyCollected() == true)
                    {
                        currentState = GameState.GameOver;
                        isSuccessful = true;
                    }
                    if (minuteTimer < 0)
                    {
                        currentState = GameState.GameOver;
                        isSuccessful = false;
                    }
                    break;

                // Pause
                case GameState.Pause:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();
                    if (quitButton2.Click())
                    {
                        Exit();
                    }
                    if (resumeButton.Click())
                    {
                        currentState = GameState.Game;
                    }
                    if (titleReturn.Click())
                    {
                        currentState = GameState.Title;
                    }
                    break;

                // Game over
                case GameState.GameOver:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();
                    if (SingleKeyPress(kbState, prevkbState, Keys.Enter))
                    {
                        currentState = GameState.Title;
                    }
                    break;
            }

            // Set previous keyboard and mouse state to current
            prevkbState = kbState;
            prevmState = mState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            switch (currentState)
            {
                // Title
                case GameState.Title:
                    title.ScreenDraw(_spriteBatch);
                    startButton.DrawButton(_spriteBatch, "Start");
                    controlsButton.DrawButton(_spriteBatch, "Controls");
                    quitButton.DrawButton(_spriteBatch, "Quit");
                    break;

                // Instruction
                case GameState.Instructions:
                    controls.ScreenDraw(_spriteBatch);
                    break;

                // Game
                case GameState.Game:
                    // 
                    play.ScreenDraw(_spriteBatch);

                    // Draw map using mapMaker class
                    mapMaker.DrawMap(_spriteBatch, wallList);

                    player.Draw(_spriteBatch);

                    // Shadow outline VERY temporary
                    _spriteBatch.Draw(shadow, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

                    // Draw UI
                    uiManager.Draw(_spriteBatch);

                    _spriteBatch.DrawString(agencyFB,
                        "PRESS Q TO TEST DMG TAKING\n" +
                        "Try adding with 1 to max then press 5\n",
                        new Vector2(windowWidth / 2, windowHeight / 2),
                        Color.White);

                    // Draw timer
                    time = $"Timer: {minuteTimer:0}:{secondTimer:00}:{millisecondTimer:000}";
                    _spriteBatch.DrawString(
                        agencyFB,
                        time,
                        new Vector2(
                            windowWidth / 2 - 132,
                            agencyFB.MeasureString(time).Y / 4),
                        Color.White);
                    break;

                // Pause
                case GameState.Pause:
                    pause.ScreenDraw(_spriteBatch);
                    resumeButton.DrawButton(_spriteBatch, "Resume");
                    titleReturn.DrawButton(_spriteBatch, "Return to Title Screen");
                    quitButton2.DrawButton(_spriteBatch, "Quit");
                    break;

                // Game over
                // TODO: replace temp with actual game over screen
                case GameState.GameOver:

                    play.ScreenDraw(_spriteBatch);

                    if (isSuccessful == true)
                    {
                        _spriteBatch.DrawString(
                            agencyFB, 
                            $"{elapsedMinute:0}:{elapsedSecond:00}:{elapsedMillisecond:00}", 
                            new Vector2(0, 0), 
                            Color.White);

                        _spriteBatch.DrawString(
                            agencyFB,
                            "You won! PRESS ENTER TO GO TO TITLE", 
                            new Vector2(windowWidth / 2, windowHeight / 2), 
                            Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(
                            agencyFB,
                            "Game over! PRESS ENTER TO GO TO TITLE",
                            new Vector2(windowWidth / 2, windowHeight / 2),
                            Color.White);
                    }
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Resets the timer back to default values
        /// </summary>
        public void ResetTimer()
        {
            minuteTimer = 5;
            secondTimer = 0;
            millisecondTimer = 0;

            elapsedMinute = 0;
            elapsedSecond = 0;
            elapsedMillisecond = 0;
        }

        /// <summary>
        /// Resets the success bool to false
        /// </summary>
        public void ResetSuccess()
        {
            isSuccessful = false;
        }
        #region Single Presses
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
        /// Single MousePress Checker
        /// </summary>
        /// <param name="firstPress">MouseState firstPress</param>
        /// <param name="secondPress">MouseState secondPress</param>
        /// <returns>bool if mouse click is only active for 1 frame</returns>
        public bool SingleMouseClick(MouseState firstPress, MouseState secondPress)
        {
            if (firstPress.LeftButton == ButtonState.Pressed && secondPress.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}