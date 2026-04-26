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

        // UI for customization info pop-up
        private Texture2D uiInfoBubble;
        private Texture2D uiInfoOverlay;
        private Button buttonInfo;
        private bool overlayShowing;

        //Elements for title screen and scoreboard
        private Texture2D uiStartSprite;
        private Texture2D uiScoreSprite;
        private Texture2D uiTitleSprite; //Use once we have a title and drawn sprite
        private Texture2D uiBackSprite;
        private Texture2D uiReSpecSprite;
        private Texture2D uiNextStageSprite;
        private Texture2D uiGoToSkillSprite;
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
        private Texture2D uiBossMain2;
        private Texture2D uiBossMain3;
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

        private List<Texture2D> bossGameSprites;
        private List<Texture2D> bossCustomizeSprites;

        // Additional battle UI for sprint bar
        private Texture2D uiPlayerSprintBarFront;
        private Texture2D uiPlayerSprintBarBack;

        // Game over texture
        private Texture2D gameOverTexture;

        private ScoreManager scoreManager;

        // Hold player and boss objects
        private Player player;
        private Boss[] boss;

        private int bossArchetype;

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
        private int pointsToAllocate = 0;

        private KeyboardState lastFrameState;
        private string currName = "";

        private Button buttonNextStage; // Button to switch from skill tree to player customization screen

        // Hold texture for the transition banners 
        private Texture2D defeatBanner;
        private Texture2D victoryBanner;

        // Holo shield
        private List<HoloShield> smallShields;
        private List<HoloShield> largeShields;
        private Texture2D smallShieldTexture;
        private Texture2D largeShieldTexture;


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

            //Customize info overlay
            overlayShowing = false;

            SkillTree.Instance.ReadData();

            smallShields = new List<HoloShield>();
            largeShields = new List<HoloShield>();

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
            uiGoToSkillSprite = Content.Load<Texture2D>("uiGoToSkillTree");
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
            // Load button for the skill tree to advance to the next stage
            buttonNextStage = new Button(
                new Rectangle(1250, 880, uiNextStageSprite.Width, uiNextStageSprite.Height), "", uiNextStageSprite);

            // Create "continue" button
            customizeContinue = new Button(new Rectangle(50, 400, buttonSprite.Width / 4, buttonSprite.Height / 4), "HI", buttonSprite);

            // Create play again button
            gameOverTexture = Content.Load<Texture2D>("uiGameOverSprite");
            playAgain = new Button(new Rectangle(_graphics.PreferredBackBufferWidth / 2 - gameOverTexture.Width / 2,
                810, gameOverTexture.Width, gameOverTexture.Height), "Continue", gameOverTexture);

            //Load content for customization info overlay
            uiInfoBubble = Content.Load<Texture2D>("infoBubble");
            uiInfoOverlay = Content.Load<Texture2D>("infoOverlay");
            buttonInfo = new Button(new Rectangle(1742,236, uiInfoBubble.Width, uiInfoBubble.Height), "", uiInfoBubble);

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
            ground = Content.Load<Texture2D>("groundV3");

            //Load textures for battle UI
            uiBossMain = Content.Load<Texture2D>("uiBossMain");
            uiBossMain2 = Content.Load<Texture2D>("uiBossMain2");
            uiBossMain3 = Content.Load<Texture2D>("uiBossMain3");
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

            // Load boss sprites
            bossGameSprites = new List<Texture2D> {
                Content.Load<Texture2D>("bossREGame"),
                Content.Load<Texture2D>("bossREGame2"),
                Content.Load<Texture2D>("bossREGame3"),
            };
            bossCustomizeSprites = new List<Texture2D> {
                Content.Load<Texture2D>("bossRECustomize"),
                Content.Load<Texture2D>("bossRECustomize2"),
                Content.Load<Texture2D>("bossRECustomize3"),
            };

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

            // Load banners
            defeatBanner = Content.Load<Texture2D>("bannerDefeat");
            victoryBanner = Content.Load<Texture2D>("bannerVictory");

            SelectRandomBoss();
            

            // Load shield textures
            smallShieldTexture = Content.Load<Texture2D>("holeShieldS");
            largeShieldTexture= Content.Load<Texture2D>("holoShieldL");
            SetupShields();


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

                //Check info button click
                if (buttonInfo.SingleClick(previousMouseState))
                {
                    overlayShowing = !overlayShowing;
                }
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
                    if (transitionDelay <= 0)
                    {
                        // Change gameState & reset transition time & transition bool
                        transitionDelay = 2f;
                        gameState = GameState.GameOver;
                        isTransition = false;

                        currentLevel++;
                        pointsToAllocate++;

                        if (currentLevel % 2 == 0)
                        {
                            SkillTree.Instance.AddPoint();
                        }

                        //Go to skill tree next
                        gameState = GameState.SkillTree;

                        // increase score for beating lvl
                        ScoreManager.AddCurrentScore(1000 * currentLevel);

                        ResetPlayerAndBoss();

                        SelectRandomBoss();
                        boss[0].IncrementBossStats();
                        SetupShields();
                    }
                    else
                        transitionDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                   
                }
                // To freeze the screen if the player or the boss dies
                else
                {
                    player.Update(gameTime);
                    boss[0].Update(gameTime);
                    bulletManager.UpdateAllBullets(gameTime);

                    foreach (HoloShield holoShield in smallShields) holoShield.Update();
                    foreach (HoloShield holoShield in largeShields) holoShield.Update();
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

                // Helper method to draw all the text for the player customization state
                DrawPlayerCustomizationText(_spriteBatch);

                //Draw info bubble
                _spriteBatch.Draw(uiInfoBubble, buttonInfo.Rect, Color.White);

                // Helper method to draw all the rectangles and images for the customization state
                DrawCustomizationUI(_spriteBatch, playerCustomizationUI);

                //Check whether overlay should be showing
                if (overlayShowing)
                {
                    _spriteBatch.Draw(uiInfoOverlay, new Rectangle(0, 0, 1920, 1080),Color.White);
                }

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

                foreach (HoloShield holoShield in smallShields) holoShield.Draw(_spriteBatch);
                foreach (HoloShield holoShield in largeShields) holoShield.Draw(_spriteBatch);

                // Draw stamina bar if has the skill
                if (player.Sprint)
                {
                    _spriteBatch.Draw(uiStaminaBack, new Vector2(0, 0), Color.White);
                    _spriteBatch.Draw(uiStaminaBar, new Vector2(0, 0), new Rectangle(0,0,(int)(143 + 233 * (float)player.CurrStamina / (float)player.MaxStamina),1080), Color.White);
                    _spriteBatch.Draw(uiStaminaNub, new Vector2(0, 0),new Rectangle(0 + (int)(233 - 232 * (float)player.CurrStamina / (float)player.MaxStamina),0,1920,1080), Color.White);
                    _spriteBatch.Draw(uiStaminaTop, new Vector2(0, 0), Color.White);
                }

                // Draw banner if player died
                if (player.IsDead)
                    _spriteBatch.Draw(defeatBanner, new Rectangle(960 - (defeatBanner.Width / 2), 540 - (defeatBanner.Height / 2), defeatBanner.Width, defeatBanner.Height), Color.White);

                // Draw text if boss died
                if (boss[0].IsDead)
                    _spriteBatch.Draw(victoryBanner, new Rectangle(960 - (victoryBanner.Width / 2), 540 - (victoryBanner.Height / 2), victoryBanner.Width, victoryBanner.Height), Color.White);

                //Draw battle UI
                _spriteBatch.Draw(uiPlayerMain, new Vector2(0, 0), Color.White);

                if (bossArchetype == 0)
                {
                _spriteBatch.Draw(uiBossMain, new Vector2(0, 0), Color.White);
                }
                else if (bossArchetype == 1)
                {
                    _spriteBatch.Draw(uiBossMain2, new Vector2(0, 0), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(uiBossMain3, new Vector2(0, 0), Color.White);
                }
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
                    case 2:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            // Cap the multiplier at 2x
                            if (playerHealthMultiplier < 3.1 && pointsToAllocate != 0) 
                            {
                                playerHealthMultiplier += .15f;
                                userInterface[1].Width = (int)(1000 * ((playerHealthMultiplier-1) / 3.1f));
                                // Decrement points to allocate
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 1:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            // Cap the multiplier at .5x
                            if (playerHealthMultiplier > 1.1f)
                            {
                                playerHealthMultiplier -= .15f;
                                userInterface[1].Width = (int)(1000 * ((playerHealthMultiplier-1) / 3.1f));
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 4:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerDamageMultiplier < 3.1 && pointsToAllocate != 0)
                            {
                                playerDamageMultiplier += .15f;
                                userInterface[2].Width = (int)(1000 * ((playerDamageMultiplier-1) / 3.1f));
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 3:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerDamageMultiplier > 1.1f)
                            {
                                playerDamageMultiplier -= .15f;
                                userInterface[2].Width = (int)(1000 * ((playerDamageMultiplier-1) / 3.1f));
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 6:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerSpeedMultiplier < 3.1 && pointsToAllocate != 0)
                            {
                                playerSpeedMultiplier += .15f;
                                userInterface[3].Width = (int)(1000 * ((playerSpeedMultiplier-1) / 3.1f));
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 5:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerSpeedMultiplier > 1.1f)
                            {
                                playerSpeedMultiplier -= .15f;
                                userInterface[3].Width = (int)(1000 * ((playerSpeedMultiplier-1) / 3.1f));
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 8:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerCritMultiplier < 3.1 && pointsToAllocate != 0)
                            {
                                playerCritMultiplier += .15f;
                                userInterface[4].Width = (int)(1000 * ((playerCritMultiplier-1) / 3.1f));
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 7:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (playerCritMultiplier > 1.1f)
                            {
                                playerCritMultiplier -= .15f;
                                userInterface[4].Width = (int)(1000 * ((playerCritMultiplier-1) / 3.1f));
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
                    case 11:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            gameState = GameState.SkillTree;
                        }
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
                else if (i == 11)
                    sb.Draw(buttonArray[i].Texture, buttonArray[i].Rect, Color.White);
                else if (i == 9)
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X + buttonRect.Width, buttonRect.Y - (buttonRect.Height / 2), buttonRect.Width, buttonRect.Height), null, Color.White, (float)(Math.PI / 2), new Vector2(0, 0), SpriteEffects.None, 0f);
                else if (i % 2 == 1)
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X + buttonRect.Width / 4, buttonRect.Y + buttonRect.Height * 5 / 4, buttonRect.Width, buttonRect.Height), null, Color.White, (float)(-Math.PI / 2), new Vector2(0, 0), SpriteEffects.None, 0f);
                else if (i == 10)
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X, buttonRect.Y + buttonRect.Height, buttonRect.Width, buttonRect.Height), null, Color.White, (float)(-Math.PI / 2), new Vector2(0, 0), SpriteEffects.None, 0f);

                else
                    sb.Draw(buttonArray[i].Texture, new Rectangle(buttonRect.X + buttonRect.Width, buttonRect.Y, buttonRect.Width, buttonRect.Height), null, Color.White, (float)(Math.PI / 2), new Vector2(0, 0), SpriteEffects.None, 0f);
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

            if (isTransition)
            {
                Texture2D startBanner = Content.Load<Texture2D>("bannerGameStart");
                sb.Draw(startBanner, new Rectangle(960 - (startBanner.Width / 2), 540 - (startBanner.Height / 2), startBanner.Width, startBanner.Height), Color.White);
            }

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
                    case 2:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossHealthMultiplier != 1)
                            {
                                bossHealthMultiplier += .045f;
                                userInterface[1].Width = (int)(1400 * ((bossHealthMultiplier - .37)));
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 1:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            // Cap the multiplier at .5x
                            if (bossHealthMultiplier != .37f && pointsToAllocate != 0)
                            {
                                bossHealthMultiplier -= .045f;
                                userInterface[1].Width = (int)(1400 * ((bossHealthMultiplier - .37)));
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 4:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossDamageMultiplier != 1)
                            {
                                bossDamageMultiplier += .045f;
                                userInterface[2].Width = (int)(1400 * ((bossDamageMultiplier - .37)));
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 3:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossDamageMultiplier != .37f && pointsToAllocate != 0)
                            {
                                bossDamageMultiplier -= .045f;
                                userInterface[2].Width = (int)(1400 * ((bossDamageMultiplier - .37)));
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 6:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossSpeedMultiplier != 1)
                            {
                                bossSpeedMultiplier += .045f;
                                userInterface[3].Width = (int)(1400 * ((bossSpeedMultiplier - .37)));
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 5:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossSpeedMultiplier != .37f && pointsToAllocate != 0)
                            {
                                bossSpeedMultiplier -= .045f;
                                userInterface[3].Width = (int)(1400 * ((bossSpeedMultiplier - .37)));
                                pointsToAllocate--;
                            }
                        }
                        break;
                    case 8:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossCritMultiplier != 1)
                            {
                                bossCritMultiplier += .045f;
                                userInterface[4].Width = (int)(1400 * ((bossCritMultiplier - .37)));
                                pointsToAllocate++;
                            }
                        }
                        break;
                    case 7:
                        if (buttonArray[i].SingleClick(mouseState))
                        {
                            if (bossCritMultiplier != .37f && pointsToAllocate != 0)
                            {
                                bossCritMultiplier -= .045f;
                                userInterface[4].Width = 91;
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
            AddButton(new Button(new Rectangle(0, 107, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(907, 107, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(0, 270, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(907, 270, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(0, 431, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(907, 431, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(0, 587, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(907, 587, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(1723, 158, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(1164, 158, 99, 73), "", buttonTexture), playerCustomizationButtons);
            AddButton(new Button(new Rectangle(1145, 880, (int)(uiGoToSkillSprite.Width), (int)(uiGoToSkillSprite.Height)), "", uiGoToSkillSprite), playerCustomizationButtons);
        }

        /// <summary>
        /// Helper method to load all the UI for the player customization screen
        /// </summary>
        public void LoadPlayerCustomizationUI()
        {
            Texture2D barTexture = Content.Load<Texture2D>("uiCustomizeColor");
            Texture2D playerDisplayTexture = Content.Load<Texture2D>("playerRECustomize");
            playerCustomizationUI.Add(new ImageUI(new Rectangle(1345, 280, playerDisplayTexture.Width * 2 / 3, playerDisplayTexture.Height * 2 / 3), playerDisplayTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(170, 107, 0, 90), barTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(170, 270, 0, 90), barTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(170, 431, 0, 90), barTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(170, 587, 0, 90), barTexture));
            playerCustomizationUI.Add(new ImageUI(new Rectangle(1086, 0, 10, 1080), barTexture));
        }

        /// <summary>
        /// Helper method to load all the buttons for the boss customization screen
        /// </summary>
        public void LoadBossCustomizationButtons()
        {
            Texture2D buttonTexture = Content.Load<Texture2D>("uiCustomizeButton");
            AddButton(new Button(new Rectangle(45, 96, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(839, 119, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(1746, 119, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(839, 290, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(1746, 290, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(839, 450, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(1746, 450, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(839, 611, 99, 73), "", buttonTexture), bossCustomizationButtons);
            AddButton(new Button(new Rectangle(1746, 611, 99, 73), "", buttonTexture), bossCustomizationButtons);
        }

        /// <summary>
        /// Helper method to load all the buttons for the boss customization screen
        /// </summary>
        public void LoadBossCustomizationUI()
        {
            Texture2D barTexture = Content.Load<Texture2D>("uiCustomizeColor");
            bossCustomizationUI.Add(new ImageUI(new Rectangle(180, 443, 527, 608), Content.Load<Texture2D>("bossRECustomize")));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1011, 119, 368, 113), barTexture));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1011, 290, 368, 113), barTexture));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1011, 450, 368, 113), barTexture));
            bossCustomizationUI.Add(new ImageUI(new Rectangle(1011, 611, 368, 113), barTexture));   
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
            sb.DrawString(uiText, "Health Multiplier: " + (1 + ( 9 * ((playerHealthMultiplier - 1) / 2.1f))).ToString("F2") +"x", new Vector2(269, 42), Color.White);
            sb.DrawString(uiText, "Damage Multiplier: " + (1 + (9 * ((playerDamageMultiplier - 1) / 2.1f))).ToString("F2") + "x", new Vector2(269, 206), Color.White);
            sb.DrawString(uiText, "Speed Multiplier: " + (1 + (9 * ((playerSpeedMultiplier - 1) / 2.1f))).ToString("F2") + "x", new Vector2(269, 368), Color.White);
            sb.DrawString(uiText, "Crit Chance Multiplier: " + (1 + (9 * ((playerCritMultiplier - 1) / 2.1f))).ToString("F2") + "x", new Vector2(269, 530), Color.White);
            sb.DrawString(uiText, "Back to Menu", new Vector2(1140, 50), Color.White);
            sb.DrawString(uiText, "Level: " + currentLevel, new Vector2(1176, 300), Color.White);

            int playerHealth = (int)(100 * playerHealthMultiplier);
            int playerDamage = (int)(10 * playerDamageMultiplier);
            float playerSpeed = 1 * playerSpeedMultiplier;
            float playerCrit = 5 * playerCritMultiplier;

            sb.DrawString(uiTextScore, "Stats:", new Vector2(200, 700), Color.White);

            sb.DrawString(uiText, "Health: " + playerHealth, new Vector2(200, 800), Color.White);
            sb.DrawString(uiText, "Damage: " + playerDamage, new Vector2(200, 950), Color.White);
            sb.DrawString(uiText, "Speed: " + playerSpeed.ToString("F2"), new Vector2(650, 800), Color.White);
            sb.DrawString(uiText, "Crit Chance: " + playerCrit.ToString("F2") + "%", new Vector2(650, 950), Color.White);
        }

        /// <summary>
        /// Helper method to draw the text for the boss customization screen
        /// </summary>
        /// <param name="sb"></param>
        public void DrawBossCustomizationText(SpriteBatch sb)
        {
            sb.DrawString(uiText, "To Player Customization", new Vector2(35, 36), Color.White);
            sb.DrawString(uiText, "Points Left: " + pointsToAllocate, new Vector2(219, 276), Color.White);
            sb.DrawString(uiText, "Health Multiplier: " + bossHealthMultiplier, new Vector2(1088, 83), Color.White);
            sb.DrawString(uiText, "Damage Multiplier: " + bossDamageMultiplier, new Vector2(1088, 251), Color.White);
            sb.DrawString(uiText, "Action Speed Multiplier: " + bossSpeedMultiplier, new Vector2(1088, 417), Color.White);
            sb.DrawString(uiText, "Crit Chance Multiplier: " + bossCritMultiplier, new Vector2(1088, 585), Color.White);


        }

        public void SelectRandomBoss()
        {
            // Options: Heath, Damage, Speed, or Crit

            Random random = new Random();

            bossArchetype = random.Next(3);

            float health = 50f;
            float damage = 7f;
            float speed = 1f;
            float crit = 5f;

            switch (bossArchetype)
            {
                // Health
                case 0:
                    health *= 2f;
                    damage *= 1.5f;
                    speed *= 0.6f;
                    break;
                // Damage
                case 1:
                    health *= 0.85f;
                    damage *= 3f;
                    speed *= 1.1f;
                    crit *= 2f;
                    break;
                // Speed
                case 2:
                    health *= 0.8f;
                    damage *= 0.75f;
                    speed *= 2f;
                    break;
            }

            Boss.texture = bossGameSprites[bossArchetype];
            bossCustomizationUI[0].Texture = bossCustomizeSprites[bossArchetype];

            boss[0].SetInitialValues(health, damage, speed, crit);
        }        

        public void SetupShields()
        {
            smallShields.Clear();
            largeShields.Clear();

            Random random = new Random();

            int numOfShields = random.Next(1, 3);
            for (int i = 0; i < numOfShields; i++)
            {
                Point pos = GetRandomPosInBounds(smallShieldTexture, smallShields, largeShields);
                smallShields.Add(new HoloShield(smallShieldTexture,
                    new Rectangle(pos.X, pos.Y, smallShieldTexture.Width, smallShieldTexture.Height)));
            }

            numOfShields = random.Next(0, 2);
            for (int i = 0; i < numOfShields; i++)
            {
                Point pos = GetRandomPosInBounds(largeShieldTexture, smallShields, largeShields);
                largeShields.Add(new HoloShield(largeShieldTexture,
                    new Rectangle(pos.X, pos.Y, largeShieldTexture.Width, largeShieldTexture.Height)));
            }
        }

        public Point GetRandomPosInBounds(Texture2D texture, List<HoloShield> smallShields, List<HoloShield> largeShields)
        {
            Random random = new Random();

            int width = 1920;
            int height = 1024;
            int buffer = 64 + Math.Max(texture.Width, texture.Height);

            Point pos;
            bool overlapping;
            int maxAttempts = 50;
            int attempts = 0;

            do
            {
                overlapping = false;
                pos = new Point(random.Next(buffer, width - buffer),
                        random.Next(buffer, height - buffer));

                Rectangle newShieldRect = new Rectangle(pos.X, pos.Y, texture.Width, texture.Height);

                // Check collision with all small shields
                foreach (HoloShield shield in smallShields)
                {
                    if (newShieldRect.Intersects(shield.Rect))
                    {
                        overlapping = true;
                        break;
                    }
                }

                // Check collision with all large shields
                if (!overlapping)
                {
                    foreach (HoloShield shield in largeShields)
                    {
                        if (newShieldRect.Intersects(shield.Rect))
                        {
                            overlapping = true;
                            break;
                        }
                    }
                }

                attempts++;

            } while (overlapping && attempts < maxAttempts);

            return pos;
        }
    }


}
