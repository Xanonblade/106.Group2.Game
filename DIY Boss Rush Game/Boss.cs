using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace DIY_Boss_Rush_Game
{
    public enum Action
    {
        Move,
        Charge,
        Retreat,
        Attack,
        Wait
    }
    internal class Boss : Character
    {
        public static Vector2 position;

        private List<Queue<Action>> sequences;
        private Queue<Action> currentSequence;
        private float timeForNextAction;
        private bool isActionFinished;

        private GameTime gameTime;

        public Boss() : base()
        {
            sequences = new List<Queue<Action>>();
            ReadSequences("BossSequences.txt");

            timeForNextAction = 0;
            isActionFinished = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (isActionFinished)
            {
                DetermineAction();
            }

            this.gameTime = gameTime;
        }

        public override void Draw(SpriteBatch sb)
        {

        }

        private void DetermineAction()
        {
            if (currentSequence == null)
            {
                Random random = new Random();

                currentSequence = sequences[random.Next(0, sequences.Count)];
                Action currentAction = currentSequence.Dequeue();

                switch (currentAction)
                {
                    case Action.Move:
                        break;
                    case Action.Charge:
                        break;
                    case Action.Retreat:
                        break;
                    case Action.Attack:
                        break;
                    case Action.Wait:
                        break;
                }
            }

            
        }

        private void Move(Vector2 destination, float speedMult = 1)
        {
            Vector2 direction = Vector2.Normalize(destination - position);
            Vector2 movement = direction * speedMult /* * speedStat*/ * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += movement;

            if (position == destination)
            {
                isActionFinished = true;
            }
        }

        private void Attack()
        {

        }

        private void Wait()
        {

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
        }
    }
}
