using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIY_Boss_Rush_Game
{
    /// <summary>
    /// An attack used by player and bosses
    /// </summary>
    internal class Bullet
    {
        private Texture2D texture;
        private float speed;
        public int Damage { get; private set; }
        public Vector2 UnitDir { get; private set; }
        public Vector2 Pos { get; private set; }
        public int Radius { get; private set; }

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
        public Bullet(float speed, int damage, Texture2D attackTex, Vector2 unitDir, Vector2 pos, int radius)
        {
            this.Damage = damage;
            this.texture = attackTex;
            this.speed = speed;
            this.UnitDir = unitDir;
            this.Pos = pos;
            this.Radius = radius;
        }

        /// <summary>
        /// Moves bullets and removes them at out of bounds. Doesn't check for collisions here. 
        /// 
        /// Add removal from list, and change magic numbers for bounds later, hopefully a static number or something
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Move bullet in the direction of unitDir scaled by speed and elapsed time
            Pos += UnitDir * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check out of bounds
            if (Pos.X < -Radius || Pos.X > 800 + Radius || Pos.Y < -Radius || Pos.Y > 600 + Radius)
            {
                // Remove from bulletManager's list
                BulletManager.Instance.RemoveBullet(this);
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
