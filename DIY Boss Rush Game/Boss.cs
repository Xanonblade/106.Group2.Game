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
        Random
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
        private Vector2 playerPos;

        // Constructor for the boss
        public Boss(Rectangle rect, Texture2D texture, int healthStat, int damageStat, int speedStat, int critStat) : 
            base(healthStat, damageStat, speedStat, critStat)
        {
            sequences = new List<List<Action>>();

            // Reads in all of the possile sequences the boss can do
            ReadSequences("BossSequences.txt");

            // Set initial values
            waitTime = 0;
            isActionFinished = true;
            random = new Random();
            pos = new Vector2(400, 400);
            Boss.texture = texture;
        }

        /// <summary>
        /// Updates the game state and determines the next action if the previous action is finished.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.gameTime = gameTime;

            if (isActionFinished)
            {
                DetermineAction();
            }
            DoAction();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height), Color.White);
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
            switch (currentAction)
            {
                case Action.Move:
                    Move(selectedMovePos, 1);
                    break;
                case Action.Charge:
                    Move(selectedMovePos, 4f);
                    break;
                case Action.Retreat:
                    Move(selectedMovePos, 1);
                    break;
                case Action.Attack:
                    Attack();
                    break;
                case Action.Wait:
                    Wait(1.5f);
                    break;
            }
        }

        /// <summary>
        /// Moves the entity toward the specified destination at a speed modified by the given multiplier.
        /// </summary>
        /// <param name="destination">The target position to move toward.</param>
        /// <param name="speedMult">The multiplier applied to the movement speed. Defaults to 1.</param>
        private void Move(Vector2 destination, float speedMult = 1)
        {
            // Find the direction to move in
            Vector2 direction = Vector2.Normalize(destination - pos);

            float moveSpeed = 20f;

            // Move a small amount towards the move position based on the speedMult and SpeedStat
            Vector2 movement = direction * speedMult * SpeedStat * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Add the movement vector to the boss's position
            pos += movement;

            // If the distance to the destination is less than 5, end the action
            if (Vector2.DistanceSquared(pos, destination) <= 25f)
            {
                isActionFinished = true;
            }
        }

        private void Attack()
        {
            // Get the direction towards the player
            Vector2 playerDirection = Vector2.Normalize(pos + playerPos);
            float bulletSpeed = 1000f;
            int bulletRadius = 3;

            Random random = new Random();

            AttackType attackType = (AttackType)(random.Next(0, 5));

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
            }

            isActionFinished = true;
        }

        private void AddBullet(float bulletSpeed, int bulletRadius, Vector2 direction)
        {
            // Check if the bullet crits
            int chance = random.Next(100);

            int crit = 1;

            if (chance >= CritStat * 5) crit = 2;

            base.bulletManager.CreateBullet(bulletSpeed, 7 * DamageStat * crit, BulletTexture, direction, 
                new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2), bulletRadius, false);
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
                isActionFinished = true;
            }
        }

        /// <summary>
        /// Reads action sequences from a file and populates the sequences collection.
        /// </summary>
        /// <param name="fileName">The name of the file containing action sequences.</param>
        private void ReadSequences(string fileName)
        {
            // Creates a stream reader to read the file
            StreamReader sr = new StreamReader("../../../" + fileName);
            
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
    }
}
