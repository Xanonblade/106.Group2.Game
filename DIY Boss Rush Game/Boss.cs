using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace DIY_Boss_Rush_Game
{
    /// <summary>
    /// All of the possible actions the boss can take
    /// </summary>
    public enum Action
    {
        Move,
        Charge,
        Retreat,
        Attack,
        Wait
    }
    
    public enum AttackType
    {
        Shotgun,
        Single,
        Circle,
        DoubleCircle,
        Random,
        MachineGun = 5,
        MachineGunX2 = 6 // Second machine gun attack just to increase chance of it happening when multishot is on
    }

    /// <summary>
    /// Holds all of the boss's information and handles its AI
    /// </summary>
    internal class Boss : Character
    {
        public static Vector2 pos;
        public static Texture2D texture;

        // Fields for the boss's actions
        private List<List<Action>> sequences;
        private Queue<Action> currentSequence;
        private Action currentAction;
        private bool isActionFinished;

        // Useful fields
        private GameTime gameTime;
        private Random random;

        // Fields used by actions
        private float waitTime;
        private Vector2 selectedMovePos;
        private Vector2 startPos;
        private Vector2 playerPos;
        private bool hasAttackedWhileMove;
        // Values used in action methods
        private float bulletDamage;
        private float speed;
        // From 0-100
        private float critChance;

        // Sprint speed: The speed of the boss when the player has the "Oiled up gears" skill
        private float sprintSpeed;

        // Machine gun
        private bool currMachineGunning;
        private float timeBetweenBullets;
        private int bulletsLeftToShoot;

        //Status effects
        private bool isInfected;
        private float infectedTimer;
        private bool waiting;

        // Damage multipliers for attacks, helps balance the boss's attacks without changing the boss's actual damage stat
        public float BodyMultiplier { get; private set; }

        // Save pre-multiplier stats for scaling with level
        private float roundStartHealth = 30; // 50
		private float roundStartDamage = 4; //7
        private float roundStartSpeed = 0.85f; // 1
        private float roundStartCrit = 2; // 5

		/// <summary>
		/// Getter method for currentAction
		/// </summary>
		public Action CurrentAction { get { return currentAction; } }

        // Constructor for the boss
        public Boss(Rectangle rect, Texture2D texture, int healthStat, int damageStat, int speedStat, int critStat) : 
            base(healthStat, damageStat, speedStat, critStat)
        {
            sequences = new List<List<Action>>();

            // Reads in all of the possile sequences the boss can do
            ReadSequences("Content/BossSequences.txt");

            // Set initial values
            waitTime = 0;
            isActionFinished = true;
            random = new Random();
            pos = new Vector2(rect.X, rect.Y);
            Boss.texture = texture;
            BodyMultiplier = 42;
            isInfected = false;
            infectedTimer = 2f;
            waiting = false;

            // Set sprint speed
            sprintSpeed = speedStat * 1.1f;
        }

		/// <summary>
		/// At the start of a boss, the health, damage, etc. need to be calculated using past health * architype multiplier * player chosen stat multiplier. This method sets the initial values for the boss's stats and calculates the starting health, damage, etc. based on those initial values and the player's chosen stat multipliers. This is done to make sure that the boss's stats scale properly with the player's stats and the chosen architype multipliers, while also allowing for easier balancing by adjusting the initial values and multipliers separately.
		/// </summary>
		/// <param name="health"></param>
		/// <param name="damage"></param>
		/// <param name="speed"></param>
		/// <param name="crit"></param>
		public void SetArchitypeMultipliers(float healthMult, float damageMult, float speedMult, float critMult)
        {
            MaxHealth = (int)(roundStartHealth * healthMult * HealthStat);
            CurrHealth = MaxHealth;

			bulletDamage = roundStartDamage * damageMult * DamageStat;

            this.speed = roundStartSpeed * speedMult * SpeedStat;

            critChance = roundStartCrit * critMult * CritStat;
        }

        /// <summary>
        /// Multiply stats by a fair multiplier
        /// </summary>
        public void IncrementBossStats()
        {
            int level = Game1.currentLevel;

            //MaxHealth += 10 * level;
            roundStartHealth += roundStartHealth / 10 * level;
            
            //bulletDamage += 0.5f * level;
			roundStartDamage += roundStartDamage / 10 * level;

			//speed += 0.1f * level;
			roundStartSpeed += roundStartSpeed / 10 * level;

			roundStartCrit += roundStartCrit / 10 * level;
        }

        /// <summary>
        /// Updates the game state and determines the next action if the previous action is finished.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.gameTime = gameTime;

            // Increase speed if the player has the sprint in the skill tree
            if (Sprint)
            {
                this.SpeedStat = sprintSpeed;
            }
            else
            {
                this.SpeedStat = sprintSpeed * 11 / 10;
            }

            if (isActionFinished)
            {
                DetermineAction();
            }
            // Complete machine gun attack if we're in the middle of it, otherwise do the current action
            if (currMachineGunning)
            {
                Wait(timeBetweenBullets);
            }
            else
                DoAction();

            //Virus status effect
            if (isInfected)
            {
                //Deal damage
                CurrHealth -= MaxHealth * .0015f;

                infectedTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (infectedTimer <= 0)
                {
                    isInfected = false;
                    infectedTimer = 1.5f;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height), CurrTint);
        }

        /// <summary>
        /// Chooses a new action from the current action sequence and sets movement variables
        /// </summary>
        private void DetermineAction()
        {
            // If the current action sequence is empty/completed, choose a new one
            if (currentSequence == null || currentSequence.Count <= 0)
            {
                List<Action> actions = sequences[random.Next(0, sequences.Count)];

                currentSequence = new Queue<Action>();

                for (int i = 0; i < actions.Count; i++)
                {
                    currentSequence.Enqueue(actions[i]);
                }

                
            }

            currentAction = currentSequence.Dequeue();

            isActionFinished = false;
            waitTime = 0;

            // Used for determining where the boss will move 
            // Play screen's width and height
            int width = 1920;
            int height = 1024;
            // How many pixels the boss will stay away from the edge of the screen 
            // when determining movement
            int buffer = 64 + Math.Max(texture.Width, texture.Height);

            playerPos = Player.pos;
            startPos = pos;
            hasAttackedWhileMove = false;

            if (currentAction == Action.Move)
            {
                // Choose a random position on the screen to move to
                selectedMovePos = new Vector2(random.Next(buffer, width - buffer), 
                    random.Next(buffer, height - buffer));
            }
            else if (currentAction == Action.Retreat)
            {
                // Get the direction away from the player
                Vector2 awayDirection = Vector2.Normalize(pos - playerPos);

                // Find the position a distance away from the player in the correct direction
                float retreatDistance = 300f;
                Vector2 targetRetreatPos = pos + (awayDirection * retreatDistance);
                
                // Clamp position to stay within bounds
                selectedMovePos = new Vector2(
                    Math.Clamp(targetRetreatPos.X, buffer, width - buffer),
                    Math.Clamp(targetRetreatPos.Y, buffer, height - buffer)
                );
            }
            else if (currentAction == Action.Charge)
            {
                Vector2 towardsDirection = Vector2.Normalize(playerPos - pos);

                // Find the position a distance away from the player in the correct direction
                float chargeDistance = 2000;
                Vector2 targetRetreatPos = pos + (towardsDirection * chargeDistance);

                // Clamp position to stay within bounds
                selectedMovePos = new Vector2(
                    Math.Clamp(targetRetreatPos.X, buffer, width - buffer),
                    Math.Clamp(targetRetreatPos.Y, buffer, height - buffer)
                );
            }

           
        }

        /// <summary>
        /// Performs the current action based on the value of currentAction.
        /// </summary>
        private void DoAction()
        {
            if (isActionFinished) return;

            if (waiting)
            {
                Wait(2f);
            }
            else
            {
                switch (currentAction)
                {
                    case Action.Move:
                        Move(selectedMovePos, 3f);
                        break;
                    case Action.Charge:
                        Move(selectedMovePos, 4f);
                        break;
                    case Action.Retreat:
                        Move(selectedMovePos, 3f);
                        break;
                    case Action.Attack:
                        Attack(true);
                        break;
                    case Action.Wait:
                        Wait(1f);
                        break;
                }
            } 
        }
        
        /// <summary>
        /// Moves the entity toward the specified destination at a speed modified by the given multiplier.
        /// </summary>
        /// <param name="destination">The target position to move toward.</param>
        /// <param name="speedMult">The multiplier applied to the movement speed. Defaults to 1.</param>
        private void Move(Vector2 destination, float speedMult = 20)
        {
            // Find the direction to move in
            Vector2 direction = Vector2.Normalize(destination - pos);

            //float moveSpeed = 20f;

            // Move a small amount towards the move position based on the speedMult and SpeedStat
            Vector2 movement = direction * speedMult * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Add the movement vector to the boss's position
            pos += movement;

            // If the distance to the destination is less than 5, end the action
            if (Vector2.DistanceSquared(pos, destination) <= 100f)
            {
                isActionFinished = true;
            }

            // If the distance is halfway to the destination attack
            float distanceCoveredSq = Vector2.DistanceSquared(startPos, pos);
            float distanceRemainingSq = Vector2.DistanceSquared(pos, destination);

            if (distanceCoveredSq >= distanceRemainingSq && !hasAttackedWhileMove)
            {
                Attack(false);
                hasAttackedWhileMove = true;
            }

        }

        private void Attack(bool endAction)
        {
            playerPos = Player.pos;
            // Get the direction towards the player
            Vector2 playerDirection = Vector2.Normalize(pos + playerPos);
            float bulletSpeed = 1000f;
            int bulletRadius = 3;

            Random random = new Random();

            AttackType attackType;
            if (Multishot)
                attackType = (AttackType)(random.Next(0, 7));
            else
                attackType = (AttackType)(random.Next(0, 5));

            switch (attackType)
            {
                case AttackType.Shotgun:
                    // Shoot one bullet at the player with one bullets on either side of it

                    float spreadAngle = MathHelper.Pi / 12; // 15 degrees in radians
                    Vector2 mainDir = Vector2.Normalize(playerPos - pos);
                    Vector2 leftDir = Vector2.Transform(mainDir, Matrix.CreateRotationZ(-spreadAngle));
                    Vector2 rightDir = Vector2.Transform(mainDir, Matrix.CreateRotationZ(spreadAngle));

                    AddBullet(bulletSpeed, bulletRadius, leftDir);
                    AddBullet(bulletSpeed, bulletRadius, mainDir);
                    AddBullet(bulletSpeed, bulletRadius, rightDir);

                    break;
                case AttackType.Single:
                    // Shoot a single bullet at the player
                    AddBullet(bulletSpeed, bulletRadius, Vector2.Normalize(playerPos - pos));
                    break;
                case AttackType.Circle:
                    // Shoot (12)? bullets in a circle around the boss
                    int bulletNumCircle = 12;
                    float angleStep = MathHelper.TwoPi / bulletNumCircle;

                    for (int i = 0; i < bulletNumCircle; i++)
                    {
                        float angle = i * angleStep;
                        Vector2 direction = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(angle));

                        AddBullet(bulletSpeed, bulletRadius, direction);
                    }

                    break;
                case AttackType.DoubleCircle:
                    // Shoot (12)? bullets in a circle around the boss
                    int bulletNumDoubleCircle = 12;
                    float angleStepDouble = MathHelper.TwoPi / bulletNumDoubleCircle;

                    for (int i = 0; i < bulletNumDoubleCircle; i++)
                    {
                        float angle = i * angleStepDouble;
                        Vector2 direction = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(angle));

                        AddBullet(bulletSpeed, bulletRadius, direction);
                        AddBullet(bulletSpeed * 0.5f, bulletRadius, direction);
                    }

                    break;
                case AttackType.Random:
                    // Shoot (12)? bullets in random directions
                    int bulletNumRandom = 12;

                    for (int i = 0; i < bulletNumRandom; i++)
                    {
                        double angle = random.NextDouble() * MathHelper.TwoPi;
                        Vector2 direction = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ((float)angle));

                        AddBullet(bulletSpeed, bulletRadius, direction);
                    }
                    break;
                case AttackType.MachineGun:
                case AttackType.MachineGunX2:
                    // Setup repeated bullet shooting handled elsewhere
                    currMachineGunning = true;
                    bulletsLeftToShoot = attackType == AttackType.MachineGun ? 15 : 20;
                    float attackDuration = 2f;
                    timeBetweenBullets = attackDuration / bulletsLeftToShoot;
                    //BulletOfMachineGun();
                    AddBullet(bulletSpeed, bulletRadius, Vector2.Normalize(playerPos - pos));
                    Wait(timeBetweenBullets); // Wait for next bullet

                    // Skip ending of attack here because we have to wait for end of machine gun
                    return;
            }

            if (endAction)
                isActionFinished = true;
        }

		/// <summary>
		/// Adds a bullet to the bullet manager with the given speed, radius, and direction. Also handles crits for the bullet.
		/// </summary>
		/// <param name="bulletSpeed"></param>
		/// <param name="bulletRadius"></param>
		/// <param name="direction"></param>
		private void AddBullet(float bulletSpeed, int bulletRadius, Vector2 direction)
        {
			// Check if the bullet crits
			float currSpeed = bulletSpeed;
			float currDamage = bulletDamage;
			bool isCrit = false;
			int chance = random.Next(100);
            if (chance <= critChance){
				currSpeed *= 1.5f; // Increase bullet speed for crits
				currDamage *= 2; // Double damage for crits
				isCrit = true;
			}

			base.bulletManager.CreateBullet(currSpeed, currDamage, BulletTexture, direction, 
                new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2), bulletRadius, false, isCrit);
        }

        /// <summary>
        /// Waits a number of seconds before continuing 
        /// </summary>
        /// <param name="timeToWait">Time in seconds to wait</param>
        private void Wait(float timeToWait)
        {
            // Add time since last frame to waitTime
            waitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // End the action if enough time has passed
            if (waitTime >= timeToWait)
            {
                // If we're machine gunning, shoot another bullet and reset wait time, otherwise end the action
                if (currMachineGunning)
                {
                    BulletOfMachineGun();
                    bulletsLeftToShoot--;
                    if (bulletsLeftToShoot > 0)
                    {
                        waitTime = 0; // reset timer
                        Wait(timeBetweenBullets); // Wait for next bullet
                    }
                    else
                    {
                        currMachineGunning = false;
                        isActionFinished = true;
                    }
                }

                // Normal (not machine gun) wait ending
                else
                    isActionFinished = true;

            }

            if (waitTime >= timeToWait)
            {
                waiting = false;
            }
        }

        /// <summary>
        /// Shoots a singular bullet towards the player, used for machine gun attack. This method along with update and wait form the machine gun sequence
        /// </summary>
        private void BulletOfMachineGun()
        {
            playerPos = Player.pos;
            // Get the direction towards the player
            Vector2 playerDirection = Vector2.Normalize(pos + playerPos);
            float bulletSpeed = 1000f;
            int bulletRadius = 3;
            AddBullet(bulletSpeed, bulletRadius, Vector2.Normalize(playerPos - pos));
        }

        /// <summary>
        /// Reads action sequences from a file and populates the sequences collection.
        /// </summary>
        /// <param name="fileName">The name of the file containing action sequences.</param>
        private void ReadSequences(string fileName)
        {
            // Creates a stream reader to read the file
            StreamReader sr = new StreamReader(fileName);
            
            // Clear all sequences
            sequences.Clear();

            string line = "";

            // Read all lines of the file
            while ((line = sr.ReadLine()) != null)
            {
                string[] seq = line.Split(',');

                // Add a new sequence
                sequences.Add(new List<Action>());

                // Fill the last sequence with actions
                for (int i = 0; i < seq.Length; i++)
                {
                    sequences[^1].Add(Action.Parse<Action>(seq[i]));
                }
            }

            sr.Close();
        }

        /// <summary>
        /// This override handles status effects from bullets before calling base
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="statusEffect"></param>
        public override void CollideWithBullet(float damage, BulletState statusEffect)
        {
            //Status effects from bullets
            switch (statusEffect)
            {
                case BulletState.Shock:
                    //Freezes boss
                    isActionFinished = false;
                    waiting = true;
                    break;
                case BulletState.Virus:
                    //Damage over time
                    isInfected = true;
                    break;
                    //Ignores neutral state because it's meant to do nothing then
            }

            //Damage and score
            base.CollideWithBullet(damage, statusEffect);
            ScoreManager.AddCurrentScore(100);
        }

        public void StopAction()
        {
            isActionFinished = true;
            currMachineGunning = false;
            waiting = false;           
        }
    }
}
