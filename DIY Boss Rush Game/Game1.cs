using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;


// Enum for each of the GameStates
enum GameState { Menu, Scoreboard, CustomizePlayer, CustomizeBoss, SkillTree, Game, GameOver }

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

        // Public static level to use throughout whole game
        public static int currentLevel;

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
        private Texture2D uiReSpecSprite;
        private Texture2D uiNextStageSprite;
        private Button buttonStart;
        private Button buttonScore;
        private Button buttonBack;
        private Button buttonReSpec;

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
        private Texture2D uiStaminaTop;
        private Texture2D uiStaminaNub;
        private Texture2D uiStaminaBar;
        private Texture2D uiStaminaBack;
        private SpriteFont uiText;
        private SpriteFont uiTextScore;


        // Additional battle UI for sprint bar
        private Texture2D uiPlayerSprintBarFront;
        private Texture2D uiPlayerSprintBarBack;

        // Game over texture
        private Texture2D gameOverTexture;

        private ScoreManager scoreManager;

        // Hold player and boss objects
        private Player player;
        private Boss[] boss;

        // Hold bullet manager
        private BulletManager bulletManager;

        // Hold a copy of all the boss stat's here to reference when resetting
        // the stats
        private float bossInitialHealth;
        private float bossInitialDamage;
        private float bossInitialSpeed;
        private float bossInitialCrit;

        // Hold a copy of all the player stat's here to reference when resetting 
        // the stats
        private float playerInitialHealth;
        private float playerInitialDamage;
        private float playerInitialSpeed;
        private float playerInitialCrit;

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

        // Boolean to transition between screens
        private bool isTransition = false;

        // Float for time to delay for transitions
        private float transitionDelay = 2f;

        // The number of points the player can allocate || BALANCE LATER
        private int pointsToAllocate = 4;

        private KeyboardState lastFrameState;
        private string currName = "";

        private Button buttonNextStage; // Button to switch from skill tree to player customization screen

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

            // Initialize score manager
            scoreManager = ScoreManager.Instance;
            ScoreManager.LoadScores();

            //Current level is 1
            currentLevel = 1;

            // Initialize player
            player = new Player(new Vector2(480, 417), Content.Load<Texture2D>("playerREGame"), 100f, 10f, 10f, 5f);

            player.HealthStat = 10;

            // Store initial player stats for reset purposes
            playerInitialHealth = player.HealthStat;
            playerInitialDamage = player.DamageStat;
            playerInitialSpeed = player.SpeedStat;
            playerInitialCrit = player.CritStat;

            // Initialize boss array, only 1 boss for now but can easily expand later
            boss = new Boss[1];
            boss[0] = new Boss(new Rectangle(1440, 417, 100, 100), Content.Load<Texture2D>("bossREGame"), 10, 10, 100, 5);

            // Store initial boss stats for reset purposes
            bossInitialHealth = boss[0].HealthStat;
            bossInitialDamage = boss[0].DamageStat;
            bossInitialSpeed = boss[0].SpeedStat;
            bossInitialCrit = boss[0].CritStat;

            SkillTree.Instance.ReadData();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load temporary button sprite
            buttonSprite = Content.Load<Texture2D>("tempButton");

            //Load title screen and scoreboard elements
            uiStartSprite = Content.Load<Texture2D>("uiTitleStart");
            uiScoreSprite = Content.Load<Texture2D>("uiTitleScore");
            uiBackSprite = Content.Load<Texture2D>("uiTitleBack");
            uiReSpecSprite = Content.Load<Texture2D>("uiReSpecSprite");
            uiNextStageSprite = Content.Load<Texture2D>("uiGameOverSprite");
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
            buttonReSpec = new Button(
                new Rectangle(20,
                810, uiReSpecSprite.Width, uiReSpecSprite.Height),
                "", uiReSpecSprite);
            // Load arrow button for the skill tree to advance to the next stage
            buttonNextStage = new Button(
                new Rectangle(1250, 880, uiNextStageSprite.Width, uiNextStageSprite.Height), "", uiNextStageSprite);

            // Create "continue" button
            customizeContinue = new Button(new Rectangle(50, 400, buttonSprite.Width / 4, buttonSprite.Height / 4), "HI", buttonSprite);

            // Create play again button
            gameOverTexture = Content.Load<Texture2D>("uiGameOverSprite");
            playAgain = new Button(new Rectangle(_graphics.PreferredBackBufferWidth / 2 - gameOverTexture.Width / 2,
                810, gameOverTexture.Width, gameOverTexture.Height), "Continue", gameOverTexture);

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
            uiStaminaTop = Content.Load<Texture2D>("uiStaminaTop");
            uiStaminaNub = Content.Load<Texture2D>("uiStaminaNub");
            uiStaminaBar = Content.Load<Texture2D>("uiStaminaBar");
            uiStaminaBack = Content.Load<Texture2D>("uiStaminaBack");
            uiText = Content.Load<SpriteFont>("uiText");
            uiTextScore = Content.Load < SpriteFont>("uiTextScore");

            // Load textures for sprint bar
            uiPlayerSprintBarBack = Content.Load<Texture2D>("uiCustomizeColor");
            uiPlayerSprintBarFront = Content.Load<Texture2D>("uiCustomizeColor");

            // Read in arena file
            LoadArena("Content/ArenaV1.level");

            // Bullet
            BulletManager.Configure(wallN2, player, boss[0]);
            bulletManager = BulletManager.Instance;

            // Call this right after configuring BulletManager, basically need to start bulletManager for characters once configured
            player.bulletManager = bulletManager;
            boss[0].bulletManager = bulletManager;

            // Load bullet texture and put it into character class
            bulletTexture = Content.Load<Texture2D>("bullet1");
            Character.BulletTexture = bulletTexture;

            // Initialize skill tree 
            int width = uiStartSprite.Width / 2;
            int height = uiStartSprite.Height / 2;
                
            SkillTree.Instance.Initialize(uiTextScore, uiText);

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
                UpdatePlayerCustomizationButtons(playerCustomizationButtons, previousMouseState, playerCustomizationUI, gameTime);
            }
            else if (gameState == GameState.CustomizeBoss)
            {
                // Updates the buttons in one method
                UpdateBossCustomizationButtons(bossCustomizationButtons, previousMouseState, bossCustomizationUI);
            }
            else if (gameState == GameState.Game)
            {


                // Check if GameOver state
                if (player.IsDead)
                {
                    if (transitionDelay <= 0)
                    {
                        // Change gameState & reset transition time & transition bool
                        transitionDelay = 2f;
                        gameState = GameState.GameOver;
                        isTransition = false;
                    }
                    else
                         transitionDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    lastFrameState = Keyboard.GetState();
				}
                // If boss is dead, increase level and move back to customize player state
                else if (boss[0].IsDead)
                {
                    
                    currentLevel++;

                    if (currentLevel % 2 == 0)
                    {
                        SkillTree.Instance.AddPoint();
                    }

                    //Go to skill tree next
                    gameState = GameState.SkillTree;

                    // increase score for beating lvl
                    ScoreManager.AddCurrentScore(1000 * currentLevel);

                    ResetPlayerAndBoss();
                }
                else
                {
                    player.Update(gameTime);
                    boss[0].Update(gameTime);
                    bulletManager.UpdateAllBullets(gameTime);
                }
            }
            else if (gameState == GameState.SkillTree)
            {
                SkillTree.Instance.Update(gameTime);
                if (buttonReSpec.SingleClick(previousMouseState)) SkillTree.Instance.RespecTree();

                if (buttonNextStage.SingleClick(previousMouseState))
                    gameState = GameState.CustomizePlayer;
            }
            else if (gameState == GameState.GameOver)
            {
                // Check if play again button was clicked
                if (playAgain.SingleClick(previousMouseState))
                {
					// Save the player's name and score
					ScoreManager.AddScore(currName);
                    ScoreManager.SaveScores();
					currName = "";

                    // Reset tree
					SkillTree.Instance.WipeTree();

					// Move back to menu
					gameState = GameState.Menu;

					// Reset score and level
					currentLevel = 1;
                    ScoreManager.ResetCurentScore();

					// Reset player and boss stats to default for when player goes back to customize screen
                    ResetPlayerAndBoss();
				}

				// Get player name for scoreboard
				NameInput();
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

                //Draw scoreboard title
                _spriteBatch.DrawString(uiTextScore, "~ SCOREBOARD ~", 
                    new Vector2(_graphics.PreferredBackBufferWidth/2 - 193,60), Color.White);

                //Draw scoreboard itself

                List<KeyValuePair<string, int>> scoreList = ScoreManager.GetTopFiveScore();

                //Check count of scores to know how to print
                if (scoreList.Count < 5)
                {
                    //Print however many scores exist, + dead space
                    int scoreCount = 0;
                    for (int i = 0; i < scoreList.Count; i++)
                    {
                        //Name
                        _spriteBatch.DrawString(uiTextScore, $"{scoreList[i].Key}",
                            new Vector2(_graphics.PreferredBackBufferWidth / 3 + 20, 150 + 75 * i),
                            Color.CornflowerBlue);

                        //Score
                        _spriteBatch.DrawString(uiTextScore, $"{scoreList[i].Value}",
                            new Vector2(_graphics.PreferredBackBufferWidth / 2 + 235, 150 + 75 * i),
                            Color.White);

                        //Increase scoreCount to use later for dead spaces
                        scoreCount++;
                    }

                    //Print dead spaces
                    while (scoreCount < 5)
                    {
                        //Name
                        _spriteBatch.DrawString(uiTextScore, $"___",
                            new Vector2(_graphics.PreferredBackBufferWidth / 3 + 20, 150 + 75 * scoreCount),
                            Color.CornflowerBlue);

                        //Score
                        _spriteBatch.DrawString(uiTextScore, $"-",
                            new Vector2(_graphics.PreferredBackBufferWidth / 2 + 235, 150 + 75 * scoreCount),
                            Color.White);

                        //Increase scoreCount until 5
                        scoreCount++;
                    }
                }
                else
                {
                    //Print first 5 scores because they are sorted
                    for (int i = 0; i < 5; i++)
                    {
                        //Name
                        _spriteBatch.DrawString(uiTextScore, $"{scoreList[i].Key}",
                            new Vector2(_graphics.PreferredBackBufferWidth / 3 + 20, 150 + 75 * i),
                            Color.CornflowerBlue);

                        //Score
                        _spriteBatch.DrawString(uiTextScore, $"{scoreList[i].Value}",
                            new Vector2(_graphics.PreferredBackBufferWidth / 2 + 235, 150 + 75 * i),
                            Color.White);
                    }
                }
            }
            else if (gameState == GameState.CustomizePlayer)
            {
                // Helper method to draw all the buttons of the customization state
                DrawCustomizationButtons(_spriteBatch, playerCustomizationButtons, false);

                // Helper method to draw all the rectangles and images for the customization state
                DrawCustomizationUI(_spriteBatch, playerCustomizationUI);

                // Helper method to draw all the text for the player customization state
                DrawPlayerCustomizationText(_spriteBatch);
            }
            else if (gameState == GameState.CustomizeBoss)
            {
                // Helper method to draw all the buttons of the customization state
                DrawCustomizationButtons(_spriteBatch, bossCustomizationButtons, true);

                // Helper method to draw all the rectangles and images for the customization state
                DrawCustomizationUI(_spriteBatch, bossCustomizationUI);

                // Helper method to draw all the text for the boss customization state
                DrawBossCustomizationText(_spriteBatch);

            }
            else if (gameState == GameState.SkillTree)
            {
                SkillTree.Instance.Draw(GraphicsDevice, _spriteBatch);
                _spriteBatch.Draw(uiReSpecSprite, new Vector2(20,980), null ,Color.White,0f,Vector2.Zero,.5f,SpriteEffects.None,0f);

                _spriteBatch.Draw(buttonNextStage.Texture, buttonNextStage.Rect, Color.White);

            }
            else if (gameState == GameState.Game)
            {
                // Draw arena
                DrawArena(_spriteBatch);

                // Draw player and boss
                player.Draw(_spriteBatch);
                boss[0].Draw(_spriteBatch);

                bulletManager.DrawAllBulllets(_spriteBatch);

                // Draw stamina bar if has the skill
                if (player.Sprint)
                {
                    _spriteBatch.Draw(uiStaminaBack, new Vector2(0, 0), Color.White);
                    _spriteBatch.Draw(uiStaminaBar, new Vector2(0, 0), new Rectangle(0,0,(int)(143 + 233 * (float)player.CurrStamina / (float)player.MaxStamina),1080), Color.White);
                    _spriteBatch.Draw(uiStaminaNub, new Vector2(0, 0),new Rectangle(0 + (int)(233 - 232 * (float)player.CurrStamina / (float)player.MaxStamina),0,1920,1080), Color.White);
                    _spriteBatch.Draw(uiStaminaTop, new Vector2(0, 0), Color.White);
                }

                // Draw text if player died
                if (player.IsDead)
                    _spriteBatch.DrawString(uiText, "ERROR...Player broken...", new Vector2(960, 540), Color.Green);

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
                if (player.Sprint) //When stamina bar is visible
                {
                    _spriteBatch.DrawString(uiText, $"Score: {scoreManager.CurrentScore}", new Vector2(146, 80), Color.Black);
                    _spriteBatch.DrawString(uiText, $"Score: {scoreManager.CurrentScore}", new Vector2(148, 82), Color.White);
                    _spriteBatch.DrawString(uiText, $"Level: {currentLevel}", new Vector2(141, 113), Color.Black);
                    _spriteBatch.DrawString(uiText, $"Level: {currentLevel}", new Vector2(143, 115), Color.White);
                    
                }
                else
                {
                    _spriteBatch.DrawString(uiText, $"Score: {scoreManager.CurrentScore}", new Vector2(148, 53), Color.Black);
                    _spriteBatch.DrawString(uiText, $"Score: {scoreManager.CurrentScore}", new Vector2(150, 55), Color.White);
                    _spriteBatch.DrawString(uiText, $"Level: {currentLevel}", new Vector2(145, 86), Color.Black);
                    _spriteBatch.DrawString(uiText, $"Level: {currentLevel}", new Vector2(147, 88), Color.White);
                }
                    
            }
            else if (gameState == GameState.GameOver)
            {
                // Draw play again button
                _spriteBatch.Draw(gameOverTexture, playAgain.Rect, Color.White);

                // Draw final score and level reached
                int finalScore = scoreManager.CurrentScore;
                _spriteBatch.DrawString(uiText, $"Final Score: {finalScore}", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 100, 300), Color.White);
                _spriteBatch.DrawString(uiText, $"Final Level: {currentLevel}", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 100, 350), Color.White);

				// Draw prompt to enter name for scoreboard 
			    _spriteBatch.DrawString(uiText, $"Enter Name: {currName} and click continue to save score", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 100, 400), Color.White);
			}

			_spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Gets the player's name in game over during update
        /// </summary>
        private void NameInput() // possibly keyboard state needs to be updated
        {
            KeyboardState state = Keyboard.GetState();

            // All letters
			for (Keys key = Keys.A; key <= Keys.Z; key++)
			{
				if (state.IsKeyDown(key) && !lastFrameState.IsKeyDown(key))
				{
					currName += key.ToString().ToUpper();
				}
			}

			// Update lastFrameState
            lastFrameState = state;

			// 3 letter limit for names on scoreboard so cut off the first letter if they go over
			if (currName.Length > 3)
                currName = currName.Substring(1, 3);

            // Set up scores
            ScoreManager.SaveScores();
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
            player.HealthStat = playerInitialHealth;
            player.DamageStat = playerInitialDamage;
            player.SpeedStat = playerInitialSpeed;
            player.CritStat = playerInitialCrit;

            //Reset player status and stamina in case they are relevant
            player.IsInfected = false;
            player.IsSlowed = false;
            player.CurrStamina = player.MaxStamina;

            //Character health values back to max
            player.CurrHealth = player.MaxHealth;
            boss[0].CurrHealth = boss[0].MaxHealth;

            // Reset the boss's stats
            boss[0].HealthStat = bossInitialHealth;
            boss[0].DamageStat = bossInitialDamage;
            boss[0].SpeedStat = bossInitialSpeed;
            boss[0].CritStat = bossInitialCrit;

            // Reset player and boss position
            Player.pos = new Vector2(480, 540 - (Player.texture.Height / 2));
            Boss.pos = new Vector2(1440, 540 - (Boss.texture.Height / 2));

            //Clear all bullets currently left in game
            bulletManager.ClearBullets();
            boss[0].StopAction();

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
        public void UpdatePlayerCustomizationButtons(List<Button> buttonArray, MouseState mouseState, List<ImageUI> userInterface, GameTime gameTime)
        {
            // If statement to check for transition
            if (isTransition)
            {
                if (transitionDelay <= 0)
                {
                    // Change gameState & reset transition time & transition bool
                    transitionDelay = 2f;
                    gameState = GameState.Game;
                    isTransition = false;
                }
                else
                    transitionDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            // Loop through each button and check if it was clicked
            for (int i = 0; i < buttonArray.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            // Let game1 know to transition
                            isTransition = true;
                            // Apply the multipliers to the stats of the player and boss
                            ApplyMultipliers();
                        }
                        break;
                    case 1:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            // Cap the multiplier at 2x
                            if (playerHealthMultiplier != 2 && pointsToAllocate != 0) 
                            {
                                playerHealthMultiplier += .25f;
                                userInterface[1].Width += 91;
                                // Decrement points to allocate
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 2:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            // Cap the multiplier at .5x
                            if (playerHealthMultiplier != .5f)
                            {
                                playerHealthMultiplier -= .25f;
                                userInterface[1].Width -= 91;
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 3:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerDamageMultiplier != 2 && pointsToAllocate != 0)
                            {
                                playerDamageMultiplier += .25f;
                                userInterface[2].Width += 91;
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 4:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerDamageMultiplier != .5f)
                            {
                                playerDamageMultiplier -= .25f;
                                userInterface[2].Width -= 91;
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 5:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerSpeedMultiplier != 2 && pointsToAllocate != 0)
                            {
                                playerSpeedMultiplier += .25f;
                                userInterface[3].Width += 91;
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 6:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerSpeedMultiplier != .5f)
                            {
                                playerSpeedMultiplier -= .25f;
                                userInterface[3].Width -= 91;
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 7:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerCritMultiplier != 2 && pointsToAllocate != 0)
                            {
                                playerCritMultiplier += .25f;
                                userInterface[4].Width += 91;
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 8:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerCritMultiplier != .5f)
                            {
                                playerCritMultiplier -= .25f;
                                userInterface[4].Width -= 91;
                                pointsToAllocate++;
                            }
                        }

                        break;
                    case 9:
                        if (buttonArray[i].SingleClick(mouseState))
                            gameState = GameState.CustomizeBoss;
                        break;
                    case 10:
                        if (buttonArray[i].SingleClick(mouseState))
                            gameState = GameState.Menu;
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
                else if (i == 10)
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X, buttonRect.Y + buttonRect.Height, buttonRect.Width, buttonRect.Height), null, Color.White, (float)(-Math.PI / 2), new Vector2(0, 0), SpriteEffects.None, 0f);
                else
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X + buttonRect.Width, buttonRect.Y + buttonRect.Height, buttonRect.Width, buttonRect.Height), null, Color.White, (float)(Math.PI), new Vector2(0, 0), SpriteEffects.None, 0f);
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
                            // Cap the multiplier at 2x
                            if (bossHealthMultiplier != 2)
                            {
                                bossHealthMultiplier += .25f;
                                userInterface[1].Width += 91;
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 2:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            // Cap the multiplier at .5x
                            if (bossHealthMultiplier != .5f && pointsToAllocate != 0)
                            {
                                bossHealthMultiplier -= .25f;
                                userInterface[1].Width -= 91;
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 3:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossDamageMultiplier != 2)
                            {
                                bossDamageMultiplier += .25f;
                                userInterface[2].Width += 91;
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 4:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossDamageMultiplier != .5f && pointsToAllocate != 0)
                            {
                                bossDamageMultiplier -= .25f;
                                userInterface[2].Width -= 91;
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 5:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossSpeedMultiplier != 2)
                            {
                                bossSpeedMultiplier += .25f;
                                userInterface[3].Width += 91;
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 6:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossSpeedMultiplier != .5f && pointsToAllocate != 0)
                            {
                                bossSpeedMultiplier -= .25f;
                                userInterface[3].Width -= 91;
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 7:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossCritMultiplier != 2)
                            {
                                bossCritMultiplier += .25f;
                                userInterface[4].Width += 91;
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 8:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossCritMultiplier != .5f && pointsToAllocate != 0)
                            {
                                bossCritMultiplier -= .25f;
                                userInterface[4].Width -= 91;
                                pointsToAllocate--;
                            }
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
            this.boss[0].HealthStat = (int)(this.boss[0].HealthStat * bossHealthMultiplier);
            this.boss[0].DamageStat = (int)(this.boss[0].DamageStat * bossDamageMultiplier);
            this.boss[0].SpeedStat = (int)(this.boss[0].SpeedStat * bossSpeedMultiplier);
            this.boss[0].CritStat = (int)(this.boss[0].CritStat * bossCritMultiplier);
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
            AddButton(new Button(new Rectangle(1164, 158, 99, 73), "", buttonTexture), playerCustomizationButtons);
        }

        /// <summary>
        /// Helper method to load all the UI for the player customization screen
        /// </summary>
        public void LoadPlayerCustomizationUI()
        {
            Texture2D barTexture = Content.Load<Texture2D>("uiCustomizeColor");
            Texture2D playerDisplayTexture = Content.Load<Texture2D>("playerRECustomize");
            playerCustomizationUI.Add(new ImageUI(new Rectangle(1145, 430, playerDisplayTexture.Width * 2 / 3, playerDisplayTexture.Height * 2 / 3), playerDisplayTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(257, 113, 368, 90), barTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(257, 359, 368, 90), barTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(257, 643, 368, 90), barTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(257, 912, 368, 90), barTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(1086, 0, 10, 1080), barTexture));
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
            Texture2D barTexture = Content.Load<Texture2D>("uiCustomizeColor");
            bossCustomizationUI.Add(new ImageUI(new Rectangle(180, 443, 527, 608), Content.Load<Texture2D>("bossRECustomize")));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1098, 80, 368, 113), barTexture));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1098, 354, 368, 113), barTexture));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1098, 618, 368, 113), barTexture));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1098, 882, 368, 113), barTexture));   
            bossCustomizationUI.Add(new ImageUI(new Rectangle(766, 0, 10, 1080), barTexture));
        }

        /// <summary>
        /// Helper method to draw the text for the player customization screen
        /// </summary>
        /// <param name="sb"></param>
        public void DrawPlayerCustomizationText(SpriteBatch sb)
        {
            sb.DrawString(uiText, "To Boss Customization", new Vector2(1563, 47), Color.White);
            sb.DrawString(uiText, "Points Left: " + pointsToAllocate, new Vector2(1176, 345), Color.White);
            sb.DrawString(uiText, "Continue", new Vector2(1699, 831), Color.White);
            sb.DrawString(uiText, "Health Multiplier: " + playerHealthMultiplier, new Vector2(354, 42), Color.White);
            sb.DrawString(uiText, "Damage Multiplier: " + playerDamageMultiplier, new Vector2(354, 305), Color.White);
            sb.DrawString(uiText, "Speed Multiplier: " + playerSpeedMultiplier, new Vector2(354, 567), Color.White);
            sb.DrawString(uiText, "Crit Multiplier: " + playerCritMultiplier, new Vector2(354, 831), Color.White);
            sb.DrawString(uiText, "Back to Menu", new Vector2(1140, 50), Color.White);
            sb.DrawString(uiText, "Level: " + currentLevel, new Vector2(1176, 300), Color.White);

            // Draw transition text if transitioning
            if (isTransition)
            {
                sb.DrawString(uiText, "Going to arena...", new Vector2(960, 540), Color.Green);
            }
        }

        /// <summary>
        /// Helper method to draw the text for the boss customization screen
        /// </summary>
        /// <param name="sb"></param>
        public void DrawBossCustomizationText(SpriteBatch sb)
        {
            sb.DrawString(uiText, "To Player Customization", new Vector2(35, 36), Color.White);
            sb.DrawString(uiText, "Points Left: " + pointsToAllocate, new Vector2(219, 276), Color.White);
            sb.DrawString(uiText, "Health Multiplier: " + bossHealthMultiplier, new Vector2(1169, 42), Color.White);
            sb.DrawString(uiText, "Damage Multiplier: " + bossDamageMultiplier, new Vector2(1169, 305), Color.White);
            sb.DrawString(uiText, "Action Speed Multiplier: " + bossSpeedMultiplier, new Vector2(1169, 567), Color.White);
            sb.DrawString(uiText, "Crit Multiplier: " + bossCritMultiplier, new Vector2(1169, 831), Color.White);


        }
    }


}
