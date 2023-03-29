﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

/* Game: Dull Radience
 * Dev Team: Rice Bowls
 * Rice in the Bowl: David Gao, Jason Chen, Thaw Thaw, Will Forbes
 */

namespace Dull_Radiance
{
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
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Variables
        MapCreator mapMaker;

        //Player/Enemy Related variables

        private KeyboardState kbState;
        private KeyboardState prevkbState;
        private MouseState mState;
        private MouseState prevmState;

        private GameState currentState;
        private PlayerState playerState;

        //textures
        private Texture2D key;
        private Texture2D doors;
        private Texture2D walls;
        private Texture2D floors;
        private Texture2D lights;
        private Texture2D aliveHeart;
        private Texture2D deadHeart;

        //player
        private Player player;
        private Texture2D playerTexture;

        //menus
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

        //button items
        private List<Button> buttonList;
        private Button startButton;
        private Button quitButton;
        private Button controlsButton;
        private Button resumeButton;
        private Button titleReturn;
        private Button quitButton2;
        private Texture2D buttonTexture;
        private SpriteFont agencyFB;

        //Window Numbers
        private int windowHeight;
        private int windowWidth;

        //UI elements
        private PlayerHealth hearts;

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
            windowHeight = _graphics.PreferredBackBufferHeight;
            windowWidth = _graphics.PreferredBackBufferWidth;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            base.Initialize();

            //huh comments
            buttonList = new List<Button>
            {
                startButton,
                controlsButton,
                quitButton,
                quitButton2,
                resumeButton,
                titleReturn
            };

            //Screen where
            screensList = new List<Screens>
            {
                title,
                controls,
                play,
                pause
            };

            //Intialize 2d Map
            mapMaker = new MapCreator();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Ui elements
            aliveHeart = Content.Load<Texture2D>("LiveHeart");
            deadHeart = Content.Load<Texture2D>("DeadHeart");
            hearts = new PlayerHealth(aliveHeart, deadHeart);
            inventory = new Inventory(deadHeart, _graphics);


            // Player
            playerTexture = Content.Load<Texture2D>("Player");
            player = new Player(playerTexture, hearts);
            uiManager = new UIManager(hearts, player, inventory);

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kbState = Keyboard.GetState();
            mState = Mouse.GetState();

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
                    player.Movement();
                    uiManager.Update(gameTime, kbState, prevkbState);

                    if (kbState.IsKeyDown(Keys.P))
                    {
                        currentState = GameState.Pause;
                    }
                    if (hearts.CurrentHealth == 0)
                    {
                        currentState = GameState.GameOver;
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
            base.Update(gameTime);

            prevkbState = Keyboard.GetState();
            prevmState = Mouse.GetState();
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
                    play.ScreenDraw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    uiManager.Draw(_spriteBatch);
                    _spriteBatch.DrawString(agencyFB,
                        "PRESS ENTER TO TEST DMG TAKING",
                        new Vector2(windowWidth / 2, windowHeight / 2),
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
                    _spriteBatch.DrawString(
                        agencyFB,
                        "Game over! PRESS ENTER TO GO TO TITLE",
                        new Vector2(windowWidth / 2, windowHeight / 2),
                        Color.White);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
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
    }
}