using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

/* Game: Dull Radience
 * Dev Team: Rice Bowls
 * Rice in the Bowl: David Gao, Jason Chen, Thaw Thaw, Will Forbes
 */

// MOST RECENT VERSION

namespace Dull_Radiance
{
    #region Enumerations
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

    /// <summary>
    /// State the game can be in
    /// </summary>
    public enum GameState
    {
        Title,
        Difficulty,
        Pause,
        Game,
        Instructions,
        Selector,
        GameOver
    }
    #endregion

    public class Game1 : Game
    {
        #region Variables
        // Graphics
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool isFullScreen;

        // Keyboard and mouse states
        private KeyboardState kbState;
        private KeyboardState prevkbState;

        // Class calls
        private PlayerHealth hearts;
        private MapCreator mapMaker;
        private GameState currentState;
        //private PlayerState playerState;

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
        private Texture2D key;

        private Texture2D shadow;
        private Texture2D voidTile;
        private Texture2D keyTile;
        private Texture2D transparent;

        // Player texture
        private Player player;
        private Texture2D playerTexture;

        // Menu
        private Screens title;
        private Screens selector;
        private Screens controls;
        private Screens pause;
        private Screens play;
        private Screens gameWin;
        private Texture2D titleScreen;
        private Texture2D controlsScreen;
        private Texture2D pauseScreen;
        private Texture2D playScreen;
        private Texture2D winScreen;
        private Texture2D difficultyScreenSelector;
        private UIManager uiManager;
        private Inventory inventory;

        // Button items
        private List<Button> buttonList;
        private Button startButton;
        private Button quitTitleButton;
        private Button normalMode;
        private Button hardMode;
        private Button insaneMode;
        private Button godModeTrue;
        private Button godModeFalse;
        private Button controlsButton;
        private Button resumeButton;
        private Button titleReturn;
        private Button quitPausedButton;
        private Button selectInstruction;
        private Button selectReturn;
        private Button winTime;
        private Texture2D buttonTexture;
        private Texture2D buttonHovered;
        private SpriteFont agencyFB;

        // Window size
        private int windowHeight;
        private int windowWidth;

        // UI elements
        private List<Collectibles> collectibleList;
        private Collectibles yellowKey;
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

        //Difficulty Elements
        private Difficulty difficulty;
        private bool godMode;

        //Trackers for having a mode selected
        private bool isDifficultySelected;
        private bool isModeSelected;

        //Sounds and Music
        private List<SoundEffect> soundEffects;
        private Song bgMusic;
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
            isFullScreen = true;
            #region Window Size
            if (isFullScreen)
            {
                // Puts game into full screen
                _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();
                windowHeight = _graphics.PreferredBackBufferHeight;
                windowWidth = _graphics.PreferredBackBufferWidth;
            }
            else
            {
                // Puts game into windowed mode that simulate lab computers
                _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                _graphics.PreferredBackBufferHeight = 1080;
                _graphics.PreferredBackBufferWidth = 1920;
                _graphics.ApplyChanges();
                windowHeight = _graphics.PreferredBackBufferHeight;
                windowWidth = _graphics.PreferredBackBufferWidth;
            }
            #endregion

            #region Timer
            // Timer
            minuteTimer = 5;
            secondTimer = 0;
            millisecondTimer = 0;

            time = "";
            isSuccessful = false;

            elapsedMinute = 0;
            elapsedSecond = 0;
            elapsedMillisecond = 0;
            #endregion

            base.Initialize();

            #region List of buttons
            // ButtonList with all buttons
            buttonList = new List<Button>
            {
                startButton,
                controlsButton,
                normalMode,
                hardMode,
                insaneMode,
                godModeTrue,
                godModeFalse,
                quitTitleButton,
                quitPausedButton,
                resumeButton,
                titleReturn
            };
            #endregion

            // Intialize 2D map and modes for game
            mapMaker = new MapCreator();
            godMode = false;
            isDifficultySelected = false;
            isModeSelected = false;

            //Sound effects list
            soundEffects = new List<SoundEffect>();
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
            collectibleList = new List<Collectibles>();
            collectibleList.Add(yellowKey);
            collectibleList.Add(yellowKey);
            collectibleList.Add(yellowKey);

            // Player
            playerTexture = Content.Load<Texture2D>("Player");
            player = new Player(playerTexture, hearts, windowWidth, windowHeight, 8);

            // Screens
            titleScreen = Content.Load<Texture2D>("StartMenu");
            controlsScreen = Content.Load<Texture2D>("ControlsScreen");
            //blackScreen = Content.Load<Texture2D>("void");
            pauseScreen = Content.Load<Texture2D>("PauseScreen");
            playScreen = Content.Load<Texture2D>("TempPlayScreen");
            difficultyScreenSelector = Content.Load<Texture2D>("Difficulty");
            transparent = Content.Load<Texture2D>("transparent");
            winScreen = Content.Load<Texture2D>("GameWin(1)");

            title = new Screens(titleScreen, _graphics);
            selector = new Screens(difficultyScreenSelector, _graphics);
            controls = new Screens(controlsScreen, _graphics);
            pause = new Screens(pauseScreen, _graphics);
            play = new Screens(playScreen, _graphics);
            gameWin = new Screens(winScreen, _graphics);

            // Buttons
            buttonTexture = Content.Load<Texture2D>("BUTTON_UNHOVER");
            buttonHovered = Content.Load<Texture2D>("redHovered");

            // Fonts
            agencyFB = Content.Load<SpriteFont>("Agency FB");

            //Background Music
            this.bgMusic = Content.Load<Song>("Hor Hor");
            MediaPlayer.Volume = 2f;
            MediaPlayer.Play(bgMusic);
            MediaPlayer.IsRepeating = true;

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
            voidTile = Content.Load<Texture2D>("png");
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

            normalMode = new Button(
                windowWidth/8,
                windowHeight / 4 + windowHeight/10,
                buttonTexture,
                _graphics,
                agencyFB);

            hardMode = new Button(
                windowHeight / 4 + windowHeight/10,
                buttonTexture,
                _graphics,
                agencyFB);

            insaneMode = new Button(
                (windowWidth - windowWidth/8 - buttonTexture.Width),
                windowHeight / 4 + windowHeight/10,
                buttonTexture,
                _graphics,
                agencyFB);

            godModeTrue = new Button(
                windowWidth/5,
                windowHeight / 2 + windowHeight/10,
                buttonTexture,
                _graphics,
                agencyFB);

            godModeFalse = new Button(
                windowWidth - windowWidth/5 - buttonTexture.Width,
                windowHeight /2 + windowHeight/10,
                buttonTexture,
                _graphics,
                agencyFB);

            quitTitleButton = new Button(
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

            quitPausedButton = new Button(
                windowHeight / 2 + windowHeight / 6 + windowHeight / 9,
                buttonTexture,
                _graphics,
                agencyFB);

            selectInstruction = new Button(
                windowHeight - windowHeight/8,
                transparent,
                _graphics,
                agencyFB);

            selectReturn = new Button(
                windowHeight - windowHeight/6,
                transparent,
                _graphics,
                agencyFB);

            winTime = new Button(
                windowHeight/7,
                transparent,
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

            // Update each button
            foreach (Button button in buttonList)
            {
                button.ButtonsUpdate(gameTime);
            }

            switch (currentState)
            {
                case GameState.Title:
                    if (startButton.Click())
                    {
                        // Selected Buttons Tracker
                        isDifficultySelected = false;
                        isModeSelected = false;

                        // Change Button Texture to reflect buttons being pressed
                        normalMode.ButtonTexture = buttonTexture;
                        hardMode.ButtonTexture = buttonTexture;
                        insaneMode.ButtonTexture = buttonTexture;
                        godModeTrue.ButtonTexture = buttonTexture;
                        godModeFalse.ButtonTexture = buttonTexture;

                        currentState = GameState.Selector;
                    }
                    else if (controlsButton.Click())
                    {
                        currentState = GameState.Instructions;
                    }
                    else if (quitTitleButton.Click())
                    {
                        Exit();
                    }
                    break;

                case GameState.Instructions:
                    if (kbState.IsKeyDown(Keys.Space))
                        currentState = GameState.Title;
                    break;

                case GameState.Selector:
                    #region Difficulty and God Mode Buttons
                    // Determines which difficulty is selected
                    if (normalMode.Click())
                    {
                        difficulty = Difficulty.Normal;

                        normalMode.ButtonTexture = buttonHovered;
                        hardMode.ButtonTexture = buttonTexture;
                        insaneMode.ButtonTexture = buttonTexture;

                        isDifficultySelected = true;
                    }
                    else if (hardMode.Click())
                    {
                        difficulty = Difficulty.Hard;

                        normalMode.ButtonTexture = buttonTexture;
                        hardMode.ButtonTexture = buttonHovered;
                        insaneMode.ButtonTexture = buttonTexture;

                        isDifficultySelected = true;

                    }
                    else if (insaneMode.Click())
                    {
                        difficulty = Difficulty.Insane;

                        normalMode.ButtonTexture = buttonTexture;
                        hardMode.ButtonTexture = buttonTexture;
                        insaneMode.ButtonTexture = buttonHovered;

                        isDifficultySelected = true;
                    }

                    // Determines if god mode is on or off
                    if (godModeTrue.Click())
                    {
                        player.PlayerSpeed = 9;
                        godMode = true;

                        godModeTrue.ButtonTexture = buttonHovered;
                        godModeFalse.ButtonTexture = buttonTexture;

                        isModeSelected = true;
                    }
                    else if (godModeFalse.Click())
                    {
                        player.PlayerSpeed = 6;
                        godMode = false;

                        godModeTrue.ButtonTexture = buttonTexture;
                        godModeFalse.ButtonTexture = buttonHovered;

                        isModeSelected = true;
                    }
                    #endregion

                    // Check if difficulty and god mode are selected
                    if (isDifficultySelected && isModeSelected)
                    {
                        // Check if enter is pressed
                        if (kbState.IsKeyDown(Keys.Enter))
                        {
                            // Button stuff to determine difficulty
                            mapMaker.DifficultySelection(difficulty, godMode);

                            // Start game => reset values to default
                            player.Reset();
                            ResetSuccess();
                            ResetTimer();
                            mapMaker.ResetMap();
                            currentState = GameState.Game;
                        }
                    }

                    // Check if escape is pressed and return to title if so
                    if (kbState.IsKeyDown(Keys.Space))
                        currentState = GameState.Title;
                    break;

                case GameState.Game:
                    #region Game UI
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

                    // Update UI elements
                    uiManager.Update(gameTime, kbState, prevkbState);
                    #endregion

                    // Move Player
                    player.Movement(gameTime);
                    string result = player.CheckPosition();
                    mapMaker.DetectMovement(result);

                    // Add Key / Remove key
                    if (kbState.IsKeyDown(Keys.D1) && prevkbState.IsKeyUp(Keys.D1))
                    {
                        mapMaker.AddKey();
                    }
                    if (kbState.IsKeyDown(Keys.D5) && prevkbState.IsKeyUp(Keys.D5))
                    {
                        mapMaker.RemoveKey();
                    }

                    // Check if any conditions are met and change state accordingly
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

                case GameState.Pause:
                    // Check if buttons are clicked and change state accordingly
                    if (quitPausedButton.Click())
                    {
                        Exit();
                    }
                    else if (resumeButton.Click())
                    {
                        currentState = GameState.Game;
                    }
                    else if (titleReturn.Click())
                    {
                        currentState = GameState.Title;
                    }
                    break;

                case GameState.GameOver:
                    if (SingleKeyPress(kbState, prevkbState, Keys.Enter))
                    {
                        currentState = GameState.Title;
                    }
                    break;
            }

            // Set previous keyboard state to current
            prevkbState = kbState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            switch (currentState)
            {
                case GameState.Title:
                    // Draw title screen and buttons
                    title.ScreenDraw(_spriteBatch);
                    startButton.DrawButton(_spriteBatch, "Start", Color.White);
                    controlsButton.DrawButton(_spriteBatch, "Controls", Color.White);
                    quitTitleButton.DrawButton(_spriteBatch, "Quit", Color.White);
                    break;

                case GameState.Instructions:
                    // Draw instruction screen
                    controls.ScreenDraw(_spriteBatch);
                    break;

                case GameState.Selector:
                    // Draw selector screen and buttons
                    selector.ScreenDraw(_spriteBatch);
                    normalMode.DrawButton(_spriteBatch, "Normal Mode", Color.White);
                    hardMode.DrawButton(_spriteBatch, "Hard Mode", Color.White);
                    insaneMode.DrawButton(_spriteBatch, "Insane Mode", Color.White);
                    godModeTrue.DrawButton(_spriteBatch, "God Mode On", Color.White);
                    godModeFalse.DrawButton(_spriteBatch, "God Mode Off", Color.White);

                    // Draw text to press space to go back
                    selectReturn.DrawButton(
                        _spriteBatch,
                        "Press SPACE to Go Back",
                        Color.White);

                    // Show text depending on if difficulty and mode are selected
                    if (isDifficultySelected && isModeSelected)
                    {
                        selectInstruction.DrawButton(
                            _spriteBatch, 
                            "Press Enter to Start", 
                            Color.Red);
                    }
                    else if (isDifficultySelected && !isModeSelected)
                    {
                        selectInstruction.DrawButton(
                            _spriteBatch,
                            "Please select a god mode option",
                            Color.White);
                    }
                    else if (!isDifficultySelected && isModeSelected)
                    {
                        selectInstruction.DrawButton(
                            _spriteBatch,
                            "Please select a difficulty option",
                            Color.White);
                    }
                    else
                    {
                        selectInstruction.DrawButton(
                            _spriteBatch,
                            "Please select a difficulty AND god mode option",
                            Color.White);
                    }
                    break;

                case GameState.Game:
                    // Draw game related textures
                    play.ScreenDraw(_spriteBatch);
                    mapMaker.DrawMap(_spriteBatch, wallList);
                    player.Draw(_spriteBatch);
                    _spriteBatch.Draw(shadow, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

                    // Draw UI
                    uiManager.Draw(_spriteBatch);
                    time = $"Timer: {minuteTimer:0}:{secondTimer:00}:{millisecondTimer:000}";
                    _spriteBatch.DrawString(agencyFB, time, new Vector2(
                            windowWidth / 2 - 132,
                            agencyFB.MeasureString(time).Y / 4),
                        Color.White);
                    break;

                case GameState.Pause:
                    pause.ScreenDraw(_spriteBatch);
                    resumeButton.DrawButton(_spriteBatch, "Resume", Color.White);
                    titleReturn.DrawButton(_spriteBatch, "Return to Title Screen", Color.White);
                    quitPausedButton.DrawButton(_spriteBatch, "Quit", Color.White);
                    break;

                case GameState.GameOver:
                    // Draw game over screen and extra depending if the player won or lost
                    if (isSuccessful == true)
                    {
                        // Draw win screen
                        gameWin.ScreenDraw(_spriteBatch);

                        // Draw time taken
                        string timeTaken = $"Elasped Time - {elapsedMinute:0}:{elapsedSecond:00}:{elapsedMillisecond:00}";
                        winTime.DrawButton(_spriteBatch,timeTaken, Color.White);

                        // Draw win message
                        string winMessage = "You escaped! Press ENTER to go back to TITLE";
                        _spriteBatch.DrawString(
                            agencyFB,
                            winMessage,
                            new Vector2(
                                windowWidth / 2 - agencyFB.MeasureString(winMessage).X/2, 
                                windowHeight / 8),
                            Color.White);
                    }
                    else
                    {
                        // Draw lose screen
                        play.ScreenDraw(_spriteBatch);

                        // Draw lose message
                        _spriteBatch.DrawString(
                            agencyFB,
                            "Game over! Press ENTER to go back to TITLE",
                            new Vector2(windowWidth / 2, windowHeight / 2),
                            Color.White);
                    }
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        #region Reset Methods
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
        #endregion

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