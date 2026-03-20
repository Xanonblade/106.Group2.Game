using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;


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

        // Array to read in external files
        private Texture2D[,] tiles;

        // Textures for background tiles to hold
        private Texture2D wallN0;
        private Texture2D wallN1;
        private Texture2D wallN2;
        private Texture2D wallE0;
        private Texture2D wallE1;
        private Texture2D wallE2;
        private Texture2D wallS0;
        private Texture2D wallS1;
        private Texture2D wallS2;
        private Texture2D wallW0;
        private Texture2D wallW1;
        private Texture2D wallW2;
        private Texture2D cornerNW;
        private Texture2D cornerNE;
        private Texture2D cornerSW;
        private Texture2D cornerSE;
        private Texture2D ground;

        // Hold player and boss objects
        private Player player;
        private Boss[] boss;

        // Hold bullet manager
        private BulletManager bulletManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // change graphics settings here
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            // Menu is default state
            gameState = GameState.Menu;

            // Temporary stat is 0
            stat = 0;

            // Initialize boss & player
            player = new Player(new Vector2(100, 100), Content.Load<Texture2D>("PlayerUC"));
            boss = new Boss[1];
            boss[0] = new Boss(new Rectangle(100, 100, 100, 100), Content.Load<Texture2D>("bossUC"), 10, 10, 5, 5);

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

            // Load in textures for arena
            wallN0 = Content.Load<Texture2D>("wallN0V0");
            wallN1 = Content.Load<Texture2D>("wallN1V0");
            wallN2 = Content.Load<Texture2D>("wallN2V0");
            wallE0 = Content.Load<Texture2D>("wallE0V0");
            wallE1 = Content.Load<Texture2D>("wallE1V0");
            wallE2 = Content.Load<Texture2D>("wallE2V0");
            wallS0 = Content.Load<Texture2D>("wallS0V0");
            wallS1 = Content.Load<Texture2D>("wallS1V0");
            wallS2 = Content.Load<Texture2D>("wallS2V0");
            wallW0 = Content.Load<Texture2D>("wallW0V0");
            wallW1 = Content.Load<Texture2D>("wallW1V0");
            wallW2 = Content.Load<Texture2D>("wallW2V0");
            cornerNW = Content.Load<Texture2D>("cornerNWV0");
            cornerNE = Content.Load<Texture2D>("cornerNEV0");
            cornerSW = Content.Load<Texture2D>("cornerSWV0");
            cornerSE = Content.Load<Texture2D>("cornerSEV0");
            ground = Content.Load<Texture2D>("groundV0");


            // Read in arena file
            LoadArena("Content/ArenaV1.level");

            Boss.texture = Content.Load<Texture2D>("bossUC");


            // Bullet
            BulletManager.Configure(wallN2, player, boss[0]);
            bulletManager = BulletManager.Instance;

            // Call this right after configuring BulletManager, basically need to start bulletManager for characters once configured
            player.bulletManager = bulletManager;
            boss[0].bulletManager = bulletManager;

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
                player.Update(gameTime);
                boss[0].Update(gameTime);
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
            GraphicsDevice.Clear(Color.Black);

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
                // Draw arena
                DrawArena(_spriteBatch);

                // Draw player and boss
                player.Draw(_spriteBatch);
                boss[0].Draw(_spriteBatch);
            }
            else if (gameState == GameState.GameOver)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // Method to read in arena files
        public void LoadArena(string file)
        {
            // Open stream reader
            StreamReader reader = new StreamReader(file);

            // try/catch for file reading
            try
            {
                // Determine height and width of the arena
                int width = int.Parse(reader.ReadLine());
                int height = int.Parse(reader.ReadLine());

                // Initialize tiles array
                tiles = new Texture2D[width, height];

                // Loop through each line of the file and read in the tile types
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        // Read in tile type
                        int tileType = int.Parse(reader.ReadLine());

                        // Assign tile type to the correct texture
                        switch (tileType)
                        {
                            case 0:
                                tiles[i, j] = wallN0;
                                break;
                            case 1:
                                tiles[i, j] = wallN1;
                                break;
                            case 2:
                                tiles[i, j] = wallN2;
                                break;
                            case 3:
                                tiles[i, j] = wallE0;
                                break;
                            case 4:
                                tiles[i, j] = wallE1;
                                break;
                            case 5:
                                tiles[i, j] = wallE2;
                                break;
                            case 6:
                                tiles[i, j] = wallS0;
                                break;
                            case 7:
                                tiles[i, j] = wallS1;
                                break;
                            case 8:
                                tiles[i, j] = wallS2;
                                break;
                            case 9:
                                tiles[i, j] = wallW0;
                                break;
                            case 10:
                                tiles[i, j] = wallW1;
                                break;
                            case 11:
                                tiles[i, j] = wallW2;
                                break;
                            case 12:
                                tiles[i, j] = cornerNW;
                                break;
                            case 13:
                                tiles[i, j] = cornerNE;
                                break;
                            case 14:
                                tiles[i, j] = cornerSW;
                                break;
                            case 15:
                                tiles[i, j] = cornerSE;
                                break;
                            case 16:
                                tiles[i, j] = ground;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Do something here with the exception
                
            }

            // Close reader
            reader.Close();

        }

        // Method to draw arena
        public void DrawArena(SpriteBatch sb)
        {
            for (int i = 0; i < tiles.GetLength(1); i++)
            {
                for (int j = 0; j < tiles.GetLength(0); j++)
                {
                    sb.Draw(tiles[j, i], new Vector2(j * 64, i * 64), Color.White);
                }
            }
        }
    }
}
