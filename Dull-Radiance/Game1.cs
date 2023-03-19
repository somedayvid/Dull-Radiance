using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

/* Game: Dull Radience
 * Dev Team: Rice Bowls
 * Rice in the Bowl: David Gao, Jason Chen, Thaw Thaw, Will Forbes
 */

namespace Dull_Radiance
{
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

        //Player/Enemy Related variables

        private KeyboardState kbState;
        private KeyboardState prevkbState;
        private MouseState mState;
        private MouseState prevmState;

        private GameState currentState;

        //textures
        private Texture2D player;
        private Texture2D key;
        private Texture2D doors;
        private Texture2D walls;
        private Texture2D floors;
        private Texture2D lights;
        private Texture2D hearts;
        private Texture2D deadhearts;

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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            currentState = GameState.Title;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            windowHeight = _graphics.PreferredBackBufferHeight;
            windowWidth = _graphics.PreferredBackBufferWidth;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            base.Initialize();

            buttonList = new List<Button>
            {
                startButton,
                controlsButton,
                quitButton,
                quitButton2,
                resumeButton,
                titleReturn
            };

            screensList = new List<Screens>
            {
                title,
                controls,
                //play,
                pause
            };
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //player
            player = Content.Load<Texture2D>("Player");

            //screens
            titleScreen = Content.Load<Texture2D>("StartMenu");
            controlsScreen = Content.Load<Texture2D>("ControlsScreen");
            pauseScreen = Content.Load<Texture2D>("PauseScreen");
            //playScreen = Content.Load<Texture2D>("PlayScreen");

            title = new Screens(titleScreen, _graphics);
            controls = new Screens(controlsScreen, _graphics);
            pause = new Screens(pauseScreen, _graphics); 
            //play = new Screens(playScreen, _graphics);

            //buttons
            buttonTexture = Content.Load<Texture2D>("BUTTON_UNHOVER");

            //fonts
            agencyFB = Content.Load<SpriteFont>("Agency FB");
            
            //button initializations
            startButton = new Button(
                windowWidth/10,                                           //width
                windowHeight/3 + windowHeight/36,                         //height
                buttonTexture,                                            //skin
                _graphics,                                                //gets window height and width 
                agencyFB);                                                //font
            controlsButton = new Button(windowWidth/10,                   //TODO find way to not have to include _graphics constructor for button class
                windowHeight/2 + windowHeight/36,                          
                buttonTexture,
                _graphics,
                agencyFB);
            quitButton = new Button(windowWidth/10,
                windowHeight/2 + windowHeight/6 + windowHeight/36,
                buttonTexture,
                _graphics, agencyFB);
            resumeButton = new Button( windowHeight/3 + windowHeight/9,
                buttonTexture,
                _graphics,
                agencyFB);
            titleReturn = new Button(windowHeight/2 + windowHeight/9,
                buttonTexture,
                _graphics,
                agencyFB);
            quitButton2 = new Button(windowHeight/2 + windowHeight/6 + windowHeight/9,
                buttonTexture,
                _graphics,
                agencyFB);
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            kbState = Keyboard.GetState();
            mState = Mouse.GetState();

            foreach (Button button in buttonList)
            {
                button.ButtonsUpdate(gameTime);
            }

            switch (currentState)
            {
                case GameState.Title:
                    if (startButton.Click())
                    {
                        currentState = GameState.Game;
                    }
                    if(controlsButton.Click())
                    {
                        currentState = GameState.Instructions;
                    }
                    if (quitButton.Click())
                    {
                        quitButton.Click();
                        Exit();
                    }
                    break;
                case GameState.Instructions:
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        currentState = GameState.Title;
                    }
                    break;
                case GameState.Game:
                    if(kbState.IsKeyDown(Keys.Escape))
                    {
                        currentState = GameState.Pause;
                    }
                    break;
                case GameState.Pause:
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
                case GameState.GameOver:
                    if (SingleKeyPress(kbState, prevkbState))
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
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin();

            switch (currentState)
            {
                case GameState.Title:
                    title.ScreenDraw(_spriteBatch);
                    startButton.DrawButton(_spriteBatch, "Start");
                    controlsButton.DrawButton(_spriteBatch, "Controls");
                    quitButton.DrawButton(_spriteBatch, "Quit");
                    break;
                case GameState.Instructions:
                    controls.ScreenDraw(_spriteBatch);
                    break;
                case GameState.Game:
                    //play.ScreenDraw(_spriteBatch);
                    _spriteBatch.Draw(player,
                        new Rectangle(0, 0, player.Width, player.Height),
                        Color.White);
                    break;
                case GameState.Pause:
                    pause.ScreenDraw(_spriteBatch);
                    resumeButton.DrawButton(_spriteBatch, "Resume");
                    titleReturn.DrawButton(_spriteBatch, "Return to Title Screen");
                    quitButton2.DrawButton(_spriteBatch, "Quit");
                    break;
                case GameState.GameOver:
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
        public bool SingleKeyPress(KeyboardState firstPress, KeyboardState secondPress)
        {
            if (firstPress.IsKeyDown(Keys.Enter) && secondPress.IsKeyUp(Keys.Enter))
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