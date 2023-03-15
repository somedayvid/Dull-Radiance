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
        private Texture2D titleScreen;

        //button items
        private List<Button> buttonList;
        private Button startButton;
        private Button quitButton;
        private Button controlsButton;
        private Texture2D buttonHoverTexture;
        private Texture2D buttonUnhoverTexture;
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
                quitButton
            };
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            titleScreen = Content.Load<Texture2D>("StartMenu");
            buttonUnhoverTexture = Content.Load<Texture2D>("BUTTON_UNHOVER");
            buttonHoverTexture = Content.Load<Texture2D>("BUTTON_HOVER");
            agencyFB = Content.Load<SpriteFont>("Agency FB");
            

            startButton = new Button(windowWidth/10, windowHeight/5, buttonHoverTexture, buttonUnhoverTexture, _graphics, agencyFB);
            controlsButton = new Button(windowWidth/10, windowHeight/3, buttonHoverTexture, buttonUnhoverTexture, _graphics, agencyFB);
            quitButton = new Button(windowWidth/10, windowHeight/2, buttonHoverTexture, buttonUnhoverTexture, _graphics, agencyFB);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            kbState = Keyboard.GetState();
            mState = Mouse.GetState();

            switch (currentState)
            {
                case GameState.Title:
                    foreach(Button button in buttonList)
                    {
                        button.ButtonsUpdate(gameTime);
                    }
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
                        System.Threading.Thread.Sleep(1000);
                        Exit();
                    }
                    
                    break;
                case GameState.Instructions:
                    break;
                case GameState.Game:
                    break;
                case GameState.Pause:
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
                    _spriteBatch.Draw(titleScreen,
                        new Rectangle(0, 0, windowWidth, windowHeight),
                        Color.White);
                    startButton.DrawButton(_spriteBatch, "Start");
                    controlsButton.DrawButton(_spriteBatch, "Controls");
                    quitButton.DrawButton(_spriteBatch, "Quit");
                    break;
                case GameState.Instructions:
                    break;
                case GameState.Game:
                    break;
                case GameState.Pause:
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