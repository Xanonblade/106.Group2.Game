using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


// Enum for each of the GameStates
enum GameState { Menu, Customize, Game, GameOver }

namespace DIY_Boss_Rush_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Holds the gameState, defaults to Menu State
        private GameState gameState;

        // Temporary button Sprite
        private Texture2D buttonSprite;

        // Hold Menu button
        private Button menuButton;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Menu is default state
            gameState = GameState.Menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load temporary button sprite
            buttonSprite = Content.Load<Texture2D>("tempButton");

            // Create button
            menuButton = new Button(new Rectangle(100, 100, buttonSprite.Width/4, buttonSprite.Height/4), "Play", buttonSprite);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Finite State Machine
            if (gameState == GameState.Menu)
            {
                // Create Menu button to play
                if (menuButton.Click())
                {
                    gameState = GameState.Customize;
                }
                
            }
            else if (gameState == GameState.Customize)
            {

            }
            else if (gameState == GameState.Game)
            {

            }
            else if (gameState == GameState.GameOver)
            {

            }


                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Finite State Machine for Draw Method
            // Finite State Machine
            if (gameState == GameState.Menu)
            {
                // Draw menu button
                _spriteBatch.Draw(buttonSprite, menuButton.Rect, Color.White);
            }
            else if (gameState == GameState.Customize)
            {

            }
            else if (gameState == GameState.Game)
            {

            }
            else if (gameState == GameState.GameOver)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
