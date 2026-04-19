using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Enum for status effect of bullet
enum BulletState { Neutral, Shock, Virus}

namespace DIY_Boss_Rush_Game
{
    /// <summary>
    /// An attack used by player and bosses
    /// </summary>
    internal class Bullet
    {
        private Texture2D texture;
        private float speed;
        public float Damage { get; private set; }
        public Vector2 UnitDir { get; private set; }
        public Vector2 Pos { get; private set; }
        // This doesn't change texture, and needs to be set manually to reflect textures width and height
        public float Radius { get; private set; }

        private int bounces;

        private readonly int percentChangeBossBounce = 25;
        public BulletState StatusEffect { get; private set; }

        /// <summary>
        /// Sets every field
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="attackTex"></param>
        /// <param name="widthHeightRect"></param>
        /// <param name="unitDir"></param>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        public Bullet(float speed, float damage, Texture2D attackTex, Vector2 unitDir, Vector2 pos, float radius, BulletState statusEffect)
        {
            this.Damage = damage;
            this.texture = attackTex;
            this.speed = speed;
            this.UnitDir = unitDir;
            this.Pos = pos;
            this.Radius = radius;
            this.StatusEffect = statusEffect;

            bounces = 0;
        }

        /// <summary>
        /// Moves bullets and removes them at out of bounds. Doesn't check for collisions here. 
        /// 
        /// Add removal from list, and change magic numbers for bounds later, hopefully a static number or something
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, bool richochet, bool isPlayers)
        {
            // Move bullet in the direction of unitDir scaled by speed and elapsed time
            Pos += UnitDir * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check out of bounds
            // 64 border but 0 to 1920, 0 to 1024
            if (Pos.X < -Radius + 64 || Pos.X > 1920 + Radius - 64 || Pos.Y < -Radius + 64 || Pos.Y > 1024 + Radius - 64)
            {
				// If richochet is enabled, reverse direction instead of removing
				if (richochet && bounces <= 2)
                {
                    // Differences based on boss/player
                    if (isPlayers)
                    {
                        // Update damage
                        Damage *= 1.5f; // Increase damage by 50% on each bounce
                    }
                    else
                    {
                        Damage *= 0.75f; // Decrease damage by 25% on each bounce

                        // Also remove if not passing test to prevent too many bullets
                        Random rng = new Random();
                        if (rng.Next(100) > percentChangeBossBounce)
                        {
                            BulletManager.Instance.RemoveBullet(this);

                            // exit because bullet is gone
                            return;
                        }
                    }

                    // Check which bounds it hit and reverse the appropriate direction
                    if (Pos.X < -Radius + 64 || Pos.X > 1920 + Radius - 64)
                    {
                        UnitDir = new Vector2(-UnitDir.X, UnitDir.Y); // Reverse X direction
                        Pos = new Vector2(Math.Clamp(Pos.X, -Radius + 64, 1920 + Radius - 64), Pos.Y); // Clamp position to prevent sticking out of bounds
                    }
                    if (Pos.Y < -Radius + 64 || Pos.Y > 1024 + Radius - 64)
                    {
						UnitDir = new Vector2(UnitDir.X, -UnitDir.Y); // Reverse Y direction
						Pos = new Vector2(Pos.X, Math.Clamp(Pos.Y, -Radius + 64, 1024 + Radius - 64)); // Clamp position to prevent sticking out of bounds
                    }

                    // Update bounces
                    bounces++;

                    // Randomize direction slightly
                    UnitDir = Vector2.Transform(UnitDir, Matrix.CreateRotationZ((float)(new Random().NextDouble() * 0.4 - 0.2))); // Rotate direction by -0.2 to 0.2 radians
				}
                else
                {
					// Remove from bulletManager's list
					BulletManager.Instance.RemoveBullet(this);
				}
            }
        }

        /// <summary>
        /// Draw the bullet centered, color codes bullets simply for now, we can add more complex textures and stuff later
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="isPlayers">Value whether its a players bullet or not</param>
        public void Draw(SpriteBatch spriteBatch, bool isPlayers)
        {
            if (isPlayers)
                spriteBatch.Draw(texture, new Rectangle((int)(Pos.X - Radius), (int)(Pos.Y - Radius), texture.Width, texture.Height), Color.Purple);
            else
                spriteBatch.Draw(texture, new Rectangle((int)(Pos.X - Radius), (int)(Pos.Y - Radius), texture.Width, texture.Height), Color.LightYellow);
        }
    }
}
