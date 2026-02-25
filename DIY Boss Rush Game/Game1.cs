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

        // Hold menuPlay button
        private Button menuButton;

        // Hold increase & decrease buttons for customize state
        private Button increaseButton;
        private Button decreaseButton;

        // Placeholder for stats
        private int stat;

        // Temporary text
        private SpriteFont font;

        // Hold previousMouse state to enable single click
        private MouseState previousMouseState;

        // Hold customizeContinue button
        private Button customizeContinue;

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

            // Temporary stat is 0
            stat = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load temporary font
            font = Content.Load<SpriteFont>("Arial");

            // Load temporary button sprite
            buttonSprite = Content.Load<Texture2D>("tempButton");

            // Create menu button
            menuButton = new Button(new Rectangle(100, 100, buttonSprite.Width/4, buttonSprite.Height/4), "Play", buttonSprite);

            // Create increase & decrease button
            increaseButton = new Button(new Rectangle(0, 0, buttonSprite.Width / 4, buttonSprite.Height / 4), "Play", buttonSprite);
            decreaseButton = new Button(new Rectangle(0, 50, buttonSprite.Width / 4, buttonSprite.Height / 4), "Play", buttonSprite);

            // Create "continue" button
            customizeContinue = new Button(new Rectangle(50, 400, buttonSprite.Width / 4, buttonSprite.Height / 4), "HI", buttonSprite);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Finite State Machine
            if (gameState == GameState.Menu)
            {
                // Create Menu button to play
                if (menuButton.SingleClick(previousMouseState))
                {
                    gameState = GameState.Customize;
                }
                
            }
            else if (gameState == GameState.Customize)
            {
                // Check if either button was pressed
                if (increaseButton.SingleClick(previousMouseState))
                {
                    // Update text
                    stat++;
                }
                else if (decreaseButton.SingleClick(previousMouseState)) 
                {
                    // Update text
                    stat--;
                }

                // Continue button
                if (customizeContinue.SingleClick(previousMouseState))
                {
                    // Move GameState
                    gameState = GameState.Game;
                }
            }
            else if (gameState == GameState.Game)
            {

            }
            else if (gameState == GameState.GameOver)
            {

            }

            // Collect previous mouseState
            previousMouseState = Mouse.GetState();

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
                // Draw increase and decrease buttons
                _spriteBatch.Draw(buttonSprite, increaseButton.Rect, Color.Red);
                _spriteBatch.Draw(buttonSprite, decreaseButton.Rect, Color.Blue);

                // Draw stat number
                _spriteBatch.DrawString(font, stat + "", new Vector2(100, 100), Color.Red);

                // Draw customizeContinue button
                _spriteBatch.Draw(buttonSprite, customizeContinue.Rect, Color.White);
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
