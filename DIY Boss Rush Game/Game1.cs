using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;


// Enum for each of the GameStates
enum GameState { Menu, Scoreboard, CustomizePlayer, CustomizeBoss, Game, GameOver }

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

        // Buttons for player customization state
        private List<Button> playerCustomizationButtons = new List<Button>();

        // UI for player customization state
        private List<ImageUI> playerCustomizationUI = new List<ImageUI>();

        // Buttons for boss customization state
        private List<Button> bossCustomizationButtons = new List<Button>();

        // UI for boss customization state
        private List<ImageUI> bossCustomizationUI = new List<ImageUI>();

        //Elements for title screen and scoreboard
        private Texture2D uiStartSprite;
        private Texture2D uiScoreSprite;
        private Texture2D uiTitleSprite; //Use once we have a title and drawn sprite
        private Texture2D uiBackSprite;
        private Button buttonStart;
        private Button buttonScore;
        private Button buttonBack;

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
        private int playerMaxHealthStat;
        private int playerDamageStat;
        private int playerSpeedStat;
        private int playerCritStat;

        // Bullet texture
        private Texture2D bulletTexture;

        // Multipliers for the stats of the boss and the player
        private float playerHealthMultiplier = 1;
        private float playerDamageMultiplier = 1;
        private float playerSpeedMultiplier = 1;
        private float playerCritMultiplier = 1;
        private float bossHealthMultiplier = 1;
        private float bossDamageMultiplier = 1;
        private float bossSpeedMultiplier = 1;
        private float bossCritMultiplier = 1;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // change graphics settings here
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;

            // Set full screen to true
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

            player.HealthStat = 10;

            // Store initial player stats for reset purposes
            playerMaxHealthStat = player.HealthStat;
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

            //Load title screen and scoreboard elements
            uiStartSprite = Content.Load<Texture2D>("uiTitleStart");
            uiScoreSprite = Content.Load<Texture2D>("uiTitleScore");
            uiBackSprite = Content.Load<Texture2D>("uiTitleBack");
            buttonStart = new Button(
                new Rectangle(240,810,uiStartSprite.Width,uiStartSprite.Height),
                "", uiStartSprite);
            buttonScore = new Button(
                new Rectangle(_graphics.PreferredBackBufferWidth - 240 - uiScoreSprite.Width,
                810,uiScoreSprite.Width,uiScoreSprite.Height),
                "", uiScoreSprite);
            buttonBack = new Button(
                new Rectangle(_graphics.PreferredBackBufferWidth/2 - uiBackSprite.Width/2,
                810,uiBackSprite.Width,uiBackSprite.Height),
                "", uiBackSprite);

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

            // Load the buttons for the player customization state
            LoadPlayerCustomizationButtons();

            // Load the UI for the player customization state
            LoadPlayerCustomizationUI();

            // Load the buttons for the boss customization state
            LoadBossCustomizationButtons();

            // Load the UI for the boss customization state
            LoadBossCustomizationUI();


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Finite State Machine
            if (gameState == GameState.Menu)
            {
                // Create Start button to play
                if (buttonStart.SingleClick(previousMouseState))
                {
                    gameState = GameState.CustomizePlayer;
                }
                // Scoreboard button
                if (buttonScore.SingleClick(previousMouseState))
                {
                    gameState = GameState.Scoreboard;
                }
            }
            else if (gameState == GameState.Scoreboard)
            {
                //Go back to title page
                if (buttonBack.SingleClick(previousMouseState))
                {
                    gameState = GameState.Menu;
                }
            }
            else if (gameState == GameState.CustomizePlayer)
            {
                // Updates the buttons in one method
                UpdatePlayerCustomizationButtons(playerCustomizationButtons, previousMouseState, playerCustomizationUI);
            }
            else if (gameState == GameState.CustomizeBoss)
            {
                // Updates the buttons in one method
                UpdateBossCustomizationButtons(bossCustomizationButtons, previousMouseState, bossCustomizationUI);
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
                    gameState = GameState.CustomizePlayer;
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
                // Draw title page buttons
                _spriteBatch.Draw(uiStartSprite, buttonStart.Rect, Color.White);
                _spriteBatch.Draw(uiScoreSprite, buttonScore.Rect, Color.White);

                //Logo texture - for now, just spritefont
                _spriteBatch.DrawString(uiText, "<Title here>",new Vector2(850, 240),
                    Color.CornflowerBlue);
            }
            else if (gameState == GameState.Scoreboard)
            {
                //Draw back button
                _spriteBatch.Draw(uiBackSprite, buttonBack.Rect, Color.White);

                //Draw scoreboard itself
            }
            else if (gameState == GameState.CustomizePlayer)
            {
                DrawCustomizationButtons(_spriteBatch, playerCustomizationButtons, false);

                DrawCustomizationUI(_spriteBatch, playerCustomizationUI);
            }
            else if (gameState == GameState.CustomizeBoss)
            {
                DrawCustomizationButtons(_spriteBatch, bossCustomizationButtons, true);

                DrawCustomizationUI(_spriteBatch, bossCustomizationUI);

            }
            else if (gameState == GameState.Game)
            {
                // Draw arena
                DrawArena(_spriteBatch);

                // Draw player and boss
                player.Draw(_spriteBatch);
                boss[0].Draw(_spriteBatch);



                bulletManager.DrawAllBulllets(_spriteBatch);

                //Draw battle UI
                _spriteBatch.Draw(uiPlayerMain, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(uiBossMain, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(uiPlayerBar, new Vector2(0), new Rectangle(0, 0, (int)(143 + 285 * (float)player.CurrHealth / (float)player.MaxHealth), 1080), Color.White);
                _spriteBatch.Draw(uiPlayerNub, new Vector2(0), new Rectangle(0 + (int)(285 - 285 * (float)player.CurrHealth / (float)player.MaxHealth), 0, 1920, 1080), Color.White);
                _spriteBatch.Draw(uiBossBar, new Vector2(1492 + (285 - (285 * (float)boss[0].CurrHealth / (float)boss[0].MaxHealth)), 0),
                    new Rectangle((int)(1492 + (285 - (285 * (float)boss[0].CurrHealth / (float)boss[0].MaxHealth))), 0, 428, 1080), Color.White);
                _spriteBatch.Draw(uiBossNub, new Vector2(1479 + (285 - (285 * (float)boss[0].CurrHealth / (float)boss[0].MaxHealth)), -1),
                    new Rectangle(1478, 0, 14, 1080), Color.White);
                _spriteBatch.Draw(uiPlayerTop, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(uiBossTop, new Vector2(0, 0), Color.White);

                //Text for battle UI with drop shadow
                _spriteBatch.DrawString(uiText, $"Score: __", new Vector2(148, 53), Color.Black);
                _spriteBatch.DrawString(uiText, $"Score: __", new Vector2(150, 55), Color.White);
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

        /// <summary>
        /// Method to read in arena files
        /// </summary>
        /// <param name="file"></param>
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
            catch
            {
                //Nothing to do

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
            player.HealthStat = playerMaxHealthStat;
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

        /// <summary>
        /// Adds the given button to the given button array
        /// </summary>
        /// <param name="button"></param>
        /// <param name="buttonArray"></param>
        public void AddButton(Button button, List<Button> buttonArray)
        {
            // Add the button to the array
            buttonArray.Add(button);
        }


        /// <summary>
        /// Updates the buttons and the UI for the player customization state
        /// </summary>
        /// <param name="buttonArray"></param>
        public void UpdatePlayerCustomizationButtons(List<Button> buttonArray, MouseState mouseState, List<ImageUI> userInterface)
        {
            // Loop through each button and check if it was clicked
            for (int i = 0; i < buttonArray.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            gameState = GameState.Game;
                            // Apply the multipliers to the stats of the player and boss
                            ApplyMultipliers();
                        }
                        break;
                    case 1:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            playerHealthMultiplier += .5f;
                            userInterface[1].Width += 183;
                        }
                        break;
                    case 2:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            playerHealthMultiplier -= .5f;
                            userInterface[1].Width -= 183;
                        }
                        break;
                    case 3:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            playerDamageMultiplier += .5f;
                            userInterface[2].Width += 183;
                        }
                        break;
                    case 4:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            playerDamageMultiplier -= .5f;
                            userInterface[2].Width -= 183;
                        }
                        break;
                    case 5:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            playerSpeedMultiplier += .5f;
                            userInterface[3].Width += 183;
                        }
                        break;
                    case 6:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            playerSpeedMultiplier -= .5f;
                            userInterface[3].Width -= 183;
                        }
                        break;
                    case 7:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            playerCritMultiplier += .5f;
                            userInterface[4].Width += 183;
                        }
                        break;
                    case 8:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            playerCritMultiplier -= .5f;
                            userInterface[4].Width -= 183;
                        }

                        break;
                    case 9:
                        if (buttonArray[i].SingleClick(mouseState))
                            gameState = GameState.CustomizeBoss;
                        break;
                }
            }
        }

        /// <summary>
        /// Draws all the buttons for the customization state with given parameters
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="buttonArray"></param>
        public void DrawCustomizationButtons(SpriteBatch sb, List<Button> buttonArray, bool isBoss)
        {
            for (int i = 0; i < buttonArray.Count; i++)
            {
                // Get button rect to make code cleaner
                Rectangle buttonRect = buttonArray[i].Rect;

                if (i == 0 && !isBoss)
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X + buttonRect.Width, buttonRect.Y - (buttonRect.Height / 2), buttonRect.Width, buttonRect.Height), null, Color.White, (float)(Math.PI / 2), new Vector2(0, 0), SpriteEffects.None, 0f);
                else if (i == 0 && isBoss)
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X, buttonRect.Y + buttonRect.Height, buttonRect.Width, buttonRect.Height), null, Color.White, (float)(-Math.PI / 2), new Vector2(0, 0), SpriteEffects.None, 0f);
                else if (i == 9)
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X + buttonRect.Width, buttonRect.Y - (buttonRect.Height / 2), buttonRect.Width, buttonRect.Height), null, Color.White, (float)(Math.PI / 2), new Vector2(0, 0), SpriteEffects.None, 0f);
                else if (i % 2 == 1)
                    sb.Draw(buttonArray[i].Texture, buttonRect, Color.White);
                else
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X + buttonRect.Width, buttonRect.Y + (buttonRect.Height / 2), buttonRect.Width, buttonRect.Height), null, Color.White, (float)(Math.PI), new Vector2(0, 0), SpriteEffects.None, 0f);
            }
                
        }

        /// <summary>
        /// Draws all the UI for the customization state with given parameters
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="userInterface"></param>
        public void DrawCustomizationUI(SpriteBatch sb, List<ImageUI> userInterface)
        {
            for (int i = 0; i < userInterface.Count; i++)
                sb.Draw(userInterface[i].Texture, userInterface[i].Rectangle, Color.White);

        }

        /// <summary>
        /// Updates the buttons and the UI for the boss customization state
        /// </summary>
        /// <param name="buttonArray"></param>
        /// <param name="mouseState"></param>
        /// <param name="userInterface"></param>
        public void UpdateBossCustomizationButtons(List<Button> buttonArray, MouseState mouseState, List<ImageUI> userInterface)
        {
            // Loop through each button and check if it was clicked
            for (int i = 0; i < buttonArray.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        if (buttonArray[i].SingleClick(mouseState))
                            gameState = GameState.CustomizePlayer;
                        break;
                    case 1:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            bossHealthMultiplier += .5f;
                            userInterface[1].Width += 183;
                        }
                        break;
                    case 2:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            bossHealthMultiplier -= .5f;
                            userInterface[1].Width -= 183;
                        }
                        break;
                    case 3:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            bossDamageMultiplier -= .5f;
                            userInterface[2].Width += 183;
                        }
                        break;
                    case 4:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            bossDamageMultiplier -= .5f;
                            userInterface[2].Width -= 183;
                        }
                        break;
                    case 5:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            bossSpeedMultiplier += .5f;
                            userInterface[3].Width += 183;
                        }
                        break;
                    case 6:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            bossSpeedMultiplier -= .5f;
                            userInterface[3].Width -= 183;
                        }
                        break;
                    case 7:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            bossCritMultiplier += .5f;
                            userInterface[4].Width += 183;
                        }
                        break;
                    case 8:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            bossCritMultiplier -= .5f;
                            userInterface[4].Width -= 183;
                        }

                        break;
                }
                
            }
        }

        /// <summary>
        /// Helper method to apply the multipliers to the player and boss
        /// </summary>
        public void ApplyMultipliers()
        {
            // Apply multiplier to player
            this.player.HealthStat = (int)(this.player.HealthStat * playerHealthMultiplier);
            this.player.DamageStat = (int)(this.player.DamageStat * playerDamageMultiplier);
            this.player.SpeedStat = (int)(this.player.SpeedStat * playerSpeedMultiplier);
            this.player.CritStat = (int)(this.player.CritStat * playerCritMultiplier);

            // Apply multiplier to boss
            this.boss[0].HealthStat = (int)(this.boss[0].HealthStat * playerHealthMultiplier);
            this.boss[0].DamageStat = (int)(this.boss[0].DamageStat * playerDamageMultiplier);
            this.boss[0].SpeedStat = (int)(this.boss[0].SpeedStat * playerSpeedMultiplier);
            this.boss[0].CritStat = (int)(this.boss[0].CritStat * playerCritMultiplier);
        }

        /// <summary>
        /// Helper method to load all the buttons for the player customization
        /// </summary>
        public void LoadPlayerCustomizationButtons()
        {
            Texture2D buttonTexture = Content.Load<Texture2D>("uiCustomizeButton");
            AddButton(new Button(new Rectangle(1723, 987, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(56, 15, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(56, 161, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(56, 311, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(56, 447, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(56, 585, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(56, 721, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(56, 859, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(56, 995, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(1723, 158, 99, 73), "", buttonTexture), playerCustomizationButtons);
        }

        /// <summary>
        /// Helper method to load all the UI for the player customization screen
        /// </summary>
        public void LoadPlayerCustomizationUI()
        {
            playerCustomizationUI.Add(new ImageUI(new Rectangle(1145, 523, 527, 608), Content.Load<Texture2D>("playerC2x")));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(257, 63, 368, 113), ground));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(257, 359, 368, 113), ground));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(257, 643, 368, 113), ground));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(257, 912, 368, 113), ground));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(1086, 0, 10, 1080), ground));
        }

        /// <summary>
        /// Helper method to load all the buttons for the boss customization screen
        /// </summary>
        public void LoadBossCustomizationButtons()
        {
            Texture2D buttonTexture = Content.Load<Texture2D>("uiCustomizeButton");
            AddButton(new Button(new Rectangle(45, 96, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(883, 28, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(883, 160, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(883, 304, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(883, 435, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(883, 568, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(883, 699, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(883, 832, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(883, 963, 99, 73), "", buttonTexture), bossCustomizationButtons);
        }

        /// <summary>
        /// Helper method to load all the buttons for the boss customization screen
        /// </summary>
        public void LoadBossCustomizationUI()
        {
            bossCustomizationUI.Add(new ImageUI(new Rectangle(180, 443, 527, 608), Content.Load<Texture2D>("bossC2x")));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1098, 80, 368, 113), ground));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1098, 354, 368, 113), ground));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1098, 618, 368, 113), ground));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1098, 882, 368, 113), ground));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(766, 0, 10, 1080), ground));
        }
    }


}
