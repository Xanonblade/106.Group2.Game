using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Linq.Expressions;


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

        // Hold playAgain button for game over state
        private Button playAgain;

        // Placeholder for stats
        private int stat;

        // Public static level to use throughout whole game
        public static int currentLevel;

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

        //Textures for battle UI
        private Texture2D uiBossMain;
        private Texture2D uiBossTop;
        private Texture2D uiBossNub;
        private Texture2D uiBossBar;
        private Texture2D uiPlayerMain;
        private Texture2D uiPlayerTop;
        private Texture2D uiPlayerNub;
        private Texture2D uiPlayerBar;
        private SpriteFont uiText;

        // Hold player and boss objects
        private Player player;
        private Boss[] boss;

        // Hold bullet manager
        private BulletManager bulletManager;

        // Hold a copy of all the boss stat's here to reference when resetting
        // the stats
        private int bossHealthStat;
        private int bossDamageStat;
        private int bossSpeedStat;
        private int bossCritStat;

        // Hold a copy of all the player stat's here to reference when resetting 
        // the stats
        private int playerHealthStat;
        private int playerDamageStat;
        private int playerSpeedStat;
        private int playerCritStat;

        // Bullet texture
        private Texture2D bulletTexture;


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

            //Current level is 1
            currentLevel = 1;

            // Initialize player
            player = new Player(new Vector2(100, 100), Content.Load<Texture2D>("playerC2x"));

            // Store initial player stats for reset purposes
            playerHealthStat = player.HealthStat;
            playerDamageStat = player.DamageStat;
            playerSpeedStat = player.SpeedStat;
            playerCritStat = player.CritStat;

            // Initialize boss array, only 1 boss for now but can easily expand later
            boss = new Boss[1];
            boss[0] = new Boss(new Rectangle(100, 100, 100, 100), Content.Load<Texture2D>("bossUC"), 10, 10, 5, 5);

            // Store initial boss stats for reset purposes
            bossHealthStat = boss[0].HealthStat;
            bossDamageStat = boss[0].DamageStat;
            bossSpeedStat = boss[0].SpeedStat;
            bossCritStat = boss[0].CritStat;

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
            menuButton = new Button(new Rectangle(100, 100, buttonSprite.Width / 4, buttonSprite.Height / 4), "Play", buttonSprite);

            // Create increase & decrease button
            increaseButton = new Button(new Rectangle(0, 0, buttonSprite.Width / 4, buttonSprite.Height / 4), "Play", buttonSprite);
            decreaseButton = new Button(new Rectangle(0, 50, buttonSprite.Width / 4, buttonSprite.Height / 4), "Play", buttonSprite);

            // Create "continue" button
            customizeContinue = new Button(new Rectangle(50, 400, buttonSprite.Width / 4, buttonSprite.Height / 4), "HI", buttonSprite);

            // Create play again button
            playAgain = new Button(new Rectangle(1256, 940, buttonSprite.Width / 4, buttonSprite.Height / 4), "", buttonSprite);

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
            cornerSW = Content.Load<Texture2D>("cornerSWV2");
            cornerSE = Content.Load<Texture2D>("cornerSEV0");
            ground = Content.Load<Texture2D>("groundV1");

            //Load textures for battle UI
            uiBossMain = Content.Load<Texture2D>("uiBossMain");
            uiBossTop = Content.Load<Texture2D>("uiBossTop");
            uiBossNub = Content.Load<Texture2D>("uiBossHealthNub");
            uiBossBar = Content.Load<Texture2D>("uiBossBar");
            uiPlayerMain = Content.Load<Texture2D>("uiPlayerMain");
            uiPlayerTop = Content.Load<Texture2D>("uiPlayerTop");
            uiPlayerNub = Content.Load<Texture2D>("uiPlayerHealthNub");
            uiPlayerBar = Content.Load<Texture2D>("uiPlayerBar");
            uiText = Content.Load<SpriteFont>("uiText");

            // Read in arena file
            LoadArena("Content/ArenaV1.level");

            Boss.texture = Content.Load<Texture2D>("bossC2x");


            // Bullet
            BulletManager.Configure(wallN2, player, boss[0]);
            bulletManager = BulletManager.Instance;

            // Call this right after configuring BulletManager, basically need to start bulletManager for characters once configured
            player.bulletManager = bulletManager;
            boss[0].bulletManager = bulletManager;

            // Load bullet texture and put it into character class
            bulletTexture = Content.Load<Texture2D>("bullet1");
            Character.BulletTexture = bulletTexture;

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
                bulletManager.UpdateAllBullets(gameTime);

                // Check if the player has 0 health to test GameOver state
                if (player.HealthStat <= 0)
                    gameState = GameState.GameOver;

            }
            else if (gameState == GameState.GameOver)
            {
                // Check if play again button was clicked
                if (playAgain.SingleClick(previousMouseState))
                {
                    // Reset player and boss stats here
                    ResetPlayerAndBoss();
                    // Move gameState back to the customize state
                    gameState = GameState.Customize;
                }
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
                
                bulletManager.DrawAllBulllets(_spriteBatch);

                //Draw battle UI - INCOMPLETE
                _spriteBatch.Draw(uiPlayerMain, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(uiBossMain, new Vector2(0, 0), Color.White);

                //REPLACEMENTS: boss and player current/max health properties || Count: 5 unresolved
                _spriteBatch.Draw(uiPlayerBar, new Vector2(0), new Rectangle(0, 0, (int)(143 + 285 * (float)(/*INSERT current/max */ .85f)), 1080), Color.White);
                _spriteBatch.Draw(uiPlayerNub, new Vector2(0), new Rectangle(0 + (int)(285 - 285 * (float)(/*INSERT current/max */ .85f)),0,1920,1080), Color.White);
                _spriteBatch.Draw(uiBossBar, new Vector2(1492 + (285 - (285 * (float)(/*INSERT current/max */ .5))), 0),
                    new Rectangle((int)(1492 + (285 - (285 * (float)(/*INSERT current/max */ .5)))),0,428,1080), Color.White);
                _spriteBatch.Draw(uiBossNub, new Vector2(1479 + (285 - (285 * (float)(/*INSERT current/max */ .5))), -1), 
                    new Rectangle(1478, 0,14,1080), Color.White);
                _spriteBatch.Draw(uiPlayerTop, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(uiBossTop, new Vector2(0, 0), Color.White);

                //Text for battle UI with drop shadow
                _spriteBatch.DrawString(uiText, $"Score: __", new Vector2(148, 53), Color.Black);
                _spriteBatch.DrawString(uiText, $"Score: __", new Vector2(150,55), Color.White);
                _spriteBatch.DrawString(uiText, $"Level: {currentLevel}", new Vector2(145, 86), Color.Black);
                _spriteBatch.DrawString(uiText, $"Level: {currentLevel}", new Vector2(147, 88), Color.White);
            }
            else if (gameState == GameState.GameOver)
            {
                // Draw play again button
                _spriteBatch.Draw(buttonSprite, playAgain.Rect, Color.White);
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

        /// <summary>
        /// Method to draw arena
        /// </summary>
        /// <param name="sb"></param>
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

        /// <summary>
        /// Reset the stats of the player and boss to their initial values when the player clicks the play again button on the game over screen
        /// </summary>
        public void ResetPlayerAndBoss()
        {
            // Reset the player's stats
            player.HealthStat = playerHealthStat;
            player.DamageStat = playerDamageStat;
            player.SpeedStat = playerSpeedStat;
            player.CritStat = playerCritStat;

            // Reset the boss's stats
            boss[0].HealthStat = bossHealthStat;
            boss[0].DamageStat = bossDamageStat;
            boss[0].SpeedStat = bossSpeedStat;
            boss[0].CritStat = bossCritStat;

            //Increase level by one
            currentLevel++;
        }
    }
}
