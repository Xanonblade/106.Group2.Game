using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DIY_Boss_Rush_Game
{
    /// <summary>
    /// Class to keep track of bullets and limit collisions
    /// For setup, call Configure() with necessary parameters before accessing the instance. This allows for lazy initialization of the singleton, ensuring that it is only created when needed and with the correct parameters.
    /// </summary>
    internal class BulletManager
    {
        private static BulletManager instance;
        private List<Bullet> playerBullets;
        private List<Bullet> enemyBullets;

        private Texture2D bulletTexture;
        private bool isInitialized = false;

        // Need instances to have them take damage
        private Player player;
        private Boss boss;

        /// <summary>
        /// Private constructor for singleton
        /// </summary>
        private BulletManager() {
            // Initialize bullet lists
            playerBullets = new List<Bullet>();
            enemyBullets = new List<Bullet>();
        }

        /// <summary>
        /// Property to get the singleton instance of BulletManager. If it doesn't exist, it creates one. This ensures that there is only one instance of BulletManager throughout the game, which is important for managing bullets and their collisions consistently.
        /// </summary>
        public static BulletManager Instance
        {
            get
            {
                if (instance == null || !instance.isInitialized)
                {
                    if (instance.isInitialized) // This is saying object reference not set to an instance of an object
                        throw new Exception("variable not set to initialized");
                    // Throw exception if instance is accessed before being configured to ensure proper initialization
                    throw new InvalidOperationException("BulletManager is not configured. Call Configure() with the necessary parameters before accessing the instance.");
                    // Without parameterization, we could just create the instance here without needing to check for initialization, but since we need parameters, we want to ensure that it's properly set up before use.
                }
                return instance;
            }
        }

        /// <summary>
        /// Initializes the singleton with parameter texture.
        /// Remove if no longer needing a parameter
        /// </summary>
        /// <param name="bulletTexture"></param>
        public static void Configure(Texture2D bulletTexture, Player player, Boss boss)
        {
            
            if (instance == null)
            {
                instance = new BulletManager();
                instance.bulletTexture = bulletTexture;
                instance.player = player;
                instance.boss = boss;
                instance.isInitialized = true;
            }
            else
            {
                // Optional: handle re-initialization attempts
                Console.WriteLine("Singleton already configured. Ignoring new parameters.");
            }
        }

        /// <summary>
        /// Creates a new bullet and adds it to the appropriate list based on whether it's from the player or an enemy.
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="attackTex"></param>
        /// <param name="widthHeightRect"></param>
        /// <param name="unitDir"></param>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="fromPlayer"></param>
        public void CreateBullet(float speed, int damage, Texture2D attackTex, Vector2 unitDir, Vector2 pos, float radius, bool fromPlayer)
        {
            Bullet newBullet = new Bullet(speed, damage, attackTex, unitDir, pos, radius);

            if (fromPlayer)
            {
                playerBullets.Add(newBullet);
            }
            else
            {
                enemyBullets.Add(newBullet);
            }
        }

        /// <summary>
        /// Removes a bullet for being out of bounds or colliding with something
        /// </summary>
        /// <param name="bullet"></param>
        public void RemoveBullet(Bullet bullet)
        {
            playerBullets.Remove(bullet);
            enemyBullets.Remove(bullet);
        }

        /// <summary>
        /// Call all bullets update, and also check for collisions here
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateAllBullets(GameTime gameTime)
        {
            // Player bullets
            for (int i = 0; i < playerBullets.Count; i++)
            {
                Bullet bullet = playerBullets[i];
                bullet.Update(gameTime);
                if (CheckCircleRectCollision(bullet.Pos, bullet.Radius, Boss.pos, Boss.texture))
                {
                    //Boss.TakeDamage(bullet.Damage);
                    RemoveBullet(bullet);
                    i--; // Decrement index to account for removed bullet
                }
            }

            // Enemy bullets
            for (int i = 0; i < enemyBullets.Count; i++)
            {
                Bullet bullet = enemyBullets[i];
                bullet.Update(gameTime);
                if (CheckCircleRectCollision(bullet.Pos, bullet.Radius, Player.pos, Player.texture))
                {
                    //Player.TakeDamage(bullet.Damage);
                    RemoveBullet(bullet);
                    i--; // Decrement index to account for removed bullet
                }
            }
        }

        /// <summary>
        /// Checks collisions between a circle and rectangle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="rectTopLeft"></param>
        /// <param name="widthHeightRectangle">Doesn't need position</param>
        /// <returns></returns>
        private bool CheckCircleRectCollision(Vector2 center, float radius, Vector2 rectTopLeft, Texture2D texture)
        {
            // Find the closest point on the rectangle to the circle's center
            float closestX = Math.Clamp(center.X, rectTopLeft.X, rectTopLeft.X + texture.Width);
            float closestY = Math.Clamp(center.Y, rectTopLeft.Y, rectTopLeft.Y + texture.Height);

            Vector2 closestPoint = new Vector2(closestX, closestY);

            // Calculate the distance squared between the closest point and the circle's center
            float distance = Vector2.DistanceSquared(center, closestPoint);

            // If the distance squared is less than the circle's radius squared, there is a collision
            return distance < radius * radius;
        }

        /// <summary>
        /// Draws all bullets to display
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawAllBulllets(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in playerBullets)
            {
                bullet.Draw(spriteBatch, true);
            }
            foreach (Bullet bullet in enemyBullets)
            {
                bullet.Draw(spriteBatch, false);
            }
        }
    }
}
