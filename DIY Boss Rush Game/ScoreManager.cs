using System.Collections.Generic;
using System.IO;

namespace DIY_Boss_Rush_Game
{
    internal class ScoreManager
    {
        // Fields
        private static ScoreManager instance;
        private int currentScore;
        private Dictionary<string, int> scores;

        public static ScoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScoreManager();
                }

                return instance;
            }

        }

        // Properties
        public int CurrentScore { get => currentScore; }
        public Dictionary<string, int> Scores { get => scores; }

        // Constructor
        private ScoreManager()
        {
            scores = new Dictionary<string, int>();
        }

        /// <summary>
        /// Adds to the current score
        /// </summary>
        /// <param name="score">Score to add</param>
        public static void AddCurrentScore(int score)
        {
            Instance.currentScore += score;
        }

        /// <summary>
        /// Sets current score to 0
        /// </summary>
        public static void ResetCurentScore()
        {
            Instance.currentScore = 0;
        }

        /// <summary>
        /// Saves all of the scores in the dictionary to a text file
        /// </summary>
        public static void SaveScores()
        {
            StreamWriter sw = new StreamWriter("../../../Score.txt");

            foreach (KeyValuePair<string, int> score in Instance.scores)
            {
                sw.WriteLine(score.Key + ":" + score.Value);
            }

            if (sw != null)
            {
                sw.Close();
            }
        }

        /// <summary>
        /// Loads all of the scores from a text file and saves them 
        /// to the dictionary
        /// </summary>
        public static void LoadScores()
        {
            StreamReader sr = new StreamReader("../../../Score.txt");

            Instance.scores.Clear();

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] data = line.Split(":");
                Instance.scores.Add(data[0], int.Parse(data[1]));
            }
            
            if (sr != null)
            {
                sr.Close();
            }

        }

        /// <summary>
        /// Adds the current score to the dictionary along with the player's names
        /// </summary>
        /// <param name="name">Player's name</param>
        public static void AddScore(string name)
        {
            Instance.scores.Add(name, Instance.currentScore);
        }

        /// <summary>
        /// Gets the top 5 scores in the dictionary
        /// </summary>
        /// <returns>Top 5 scores as a list of key value pairs</returns>
        public static List<KeyValuePair<string, int>> GetTopFiveScore()
        {
            // Convert the dictionary to a list so it can be sorted
            List<KeyValuePair<string, int>> scoreList = new List<KeyValuePair<string, int>>();

            foreach (KeyValuePair<string, int> score in Instance.scores)
            {
                scoreList.Add(score);
            }

            // Sort the list using Bubble Sort
            for (int i = 0; i < scoreList.Count - 1; i++)
            {
                for (int j = 0; j < scoreList.Count - 1; j++)
                {
                    if (scoreList[j].Value < scoreList[j + 1].Value)
                    {
                        KeyValuePair<string, int> temp = scoreList[j];
                        scoreList[j] = scoreList[j + 1];
                        scoreList[j + 1] = temp;
                    }
                }
            }

            // Return the sorted list
            return scoreList;

        }
    }
}
