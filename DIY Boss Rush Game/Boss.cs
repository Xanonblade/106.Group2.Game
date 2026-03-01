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
    /// <summary>
    /// Holds all of the boss's information and handles its AI
    /// </summary>
    internal class Boss : Character
    {
        public static Vector2 position;

        // Fields for the boss's actions
        private List<Queue<Action>> sequences;
        private Queue<Action> currentSequence;
        private Action currentAction;
        private bool isActionFinished;

        // 
        private GameTime gameTime;
        private Random random;

        private float waitTime;
        private Vector2 selectedMovePos;
        private Vector2 playerPos;

        public Boss(Rectangle rect, Texture2D texture, int healthStat, int damageStat, int speedStat, int critStat) : 
            base(rect, texture, healthStat, damageStat, speedStat, critStat)
        {
            sequences = new List<Queue<Action>>();
            ReadSequences("BossSequences.txt");

            waitTime = 0;
            isActionFinished = true;
            random = new Random();
        }

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
            base.Draw(sb);
        }

        private void DetermineAction()
        {
            if (currentSequence == null || currentSequence.Count <= 0)
            {
                currentSequence = sequences[random.Next(0, sequences.Count)];
            }

            currentAction = currentSequence.Dequeue();

            isActionFinished = false;
            waitTime = 0;


            int width = 1920;
            int height = 1080;
            int buffer = 50;

            playerPos = Player.pos;

            if (currentAction == Action.Move)
            {
                selectedMovePos = new Vector2(random.Next(buffer, width - buffer), 
                    random.Next(buffer, height - buffer));
            }
            else if (currentAction == Action.Retreat)
            {
                Vector2 awayDirection = Vector2.Normalize(position - playerPos);

                float retreatDistance = 300f;
                Vector2 targetRetreatPos = position + (awayDirection * retreatDistance);
                
                // Clamp position to stay within bounds
                selectedMovePos = new Vector2(
                    Math.Clamp(targetRetreatPos.X, buffer, width - buffer),
                    Math.Clamp(targetRetreatPos.Y, buffer, height - buffer)
                );
            }
        }

        private void DoAction()
        {
            switch (currentAction)
            {
                case Action.Move:
                    Move(selectedMovePos, 1);
                    break;
                case Action.Charge:
                    Move(playerPos, 1.5f);
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

        private void Move(Vector2 destination, float speedMult = 1)
        {
            Vector2 direction = Vector2.Normalize(destination - position);
            Vector2 movement = direction * speedMult * SpeedStat * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += movement;

            if (Vector2.DistanceSquared(position, destination) <= 25f)
            {
                isActionFinished = true;
            }
        }

        private void Attack()
        {

        }

        private void Wait(float timeToWait)
        {
            waitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (waitTime >= 3f)
            {
                isActionFinished = true;
            }
        }

        private void ReadSequences(string fileName)
        {
            StreamReader sr = new StreamReader("../../../" + fileName);

            sequences.Clear();

            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] seq = line.Split(',');

                sequences.Add(new Queue<Action>());

                for (int i = 0; i < seq.Length; i++)
                {
                    sequences[^1].Enqueue(Action.Parse<Action>(seq[i]));
                }
            }

            sr.Close();
        }
    }
}
