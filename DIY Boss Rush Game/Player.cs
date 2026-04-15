using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DIY_Boss_Rush_Game
{
    /// <summary>
    /// Player class , inherits from character, contains player specific methods such as movement and attacking. It also contains the player's current weapon
    /// </summary>
    internal class Player : Character
    {
        public static Vector2 pos;
        public static Texture2D texture;
        private readonly float speedMultiplier = 1; // Helps scale movement
        private readonly float attackMultiplier = 1; // Helps scale attack
        private readonly float attackSpeedDelay = 0.5f; // Helps set attack speed
        private readonly float critMultiplier = 10; // Helps scale crits to be percentage based
        private float timeSinceAttacked = 0.0f;
        private Random rng;

        /// <summary>
        /// Sets player specifics (static pos) and calls base constructor for character stats and texture and rectangle
        /// Most of the player constructor arguments should be set in picker instead of here once picker is done
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="tex"></param>
        public Player(Vector2 pos, Texture2D tex, float healthStat, float damageStat, float speedStat, float critStat) : base(healthStat, damageStat, speedStat, critStat)
        {
            Player.pos = pos;
            Player.texture = tex;

            rng = new Random();

			// TODO: REMOVE AFTER TESTING
			base.Multishot = true;
		}

        /// <summary>
        /// Attacks with a bullet
        /// </summary>
        /// <param name="dir"></param>
        private void Attack(Vector2 dir)
        {
           
            float bulletSpeed = 1000f;
            float bulletRadius = Character.BulletTexture.Width / 2;

            // Calculate damage and speed with crit chance
            float currSpeed = bulletSpeed;
            float currDamage = DamageStat * attackMultiplier;
            // Crit stat is between 0.5 and 2
            if (rng.Next(100) < CritStat * critMultiplier) // If random number is less than crit stat, it's a crit
            {
                currSpeed *= 1.5f; // Increase bullet speed for crits
                currDamage *= 2; // Double damage for crits
            }

            // Shoot two if multishot
            if (base.Multishot)
            {
                float damage = DamageStat * attackMultiplier * 3 / 4; // Reduce damage for multishot bullets
                int offset = 5;
                Vector2 perpendicular = Vector2.Rotate(dir, (float)Math.PI / 2) * offset;
				base.bulletManager.CreateBullet(bulletSpeed, damage, Character.BulletTexture, dir, new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2) + perpendicular, bulletRadius, true);
				base.bulletManager.CreateBullet(bulletSpeed, damage, Character.BulletTexture, dir, new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2) - perpendicular, bulletRadius, true);
			}
            else
                base.bulletManager.CreateBullet(bulletSpeed, DamageStat * attackMultiplier, Character.BulletTexture, dir, new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2), bulletRadius, true);
        }

        /// <summary>
        /// Keyboard and mouse input, still calls Character update
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Does nothing currently can add in character if makes sense

            
            KeyboardState currState = Keyboard.GetState();

            // Movement
            Vector2 movement = Vector2.Zero;
            if (currState.IsKeyDown(Keys.W))
            {
                movement.Y -= 1;
            }
            if (currState.IsKeyDown(Keys.S))
            {
                movement.Y += 1;
            }
            if (currState.IsKeyDown(Keys.A))
            {
                movement.X -= 1;
            }
            if (currState.IsKeyDown(Keys.D))
            {
                movement.X += 1;
            }
            if (movement != Vector2.Zero)
                movement.Normalize(); // Normalize to prevent faster diagonal movement
            movement *= SpeedStat * speedMultiplier; // Scale movement by speed stat and multiplier
            pos += movement; // Update player Position

            // Screen size - wall size
            int screenWidth = 1920 - 64;
            int screenHeight = 1024 - 64;

            // Clamp the player's position so it can't move outside of the screen
            pos.X = Math.Clamp(pos.X, 64, screenWidth - texture.Width);
            pos.Y = Math.Clamp(pos.Y, 64, screenHeight - texture.Height);

            // Attacking
            MouseState mouseState = Mouse.GetState();

            // Update timer if less than ready
            if (timeSinceAttacked < attackSpeedDelay)
                timeSinceAttacked += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Able to attack
            if (timeSinceAttacked > attackSpeedDelay && mouseState.LeftButton == ButtonState.Pressed)
            {
                // Reset timeSinceAttacked
                timeSinceAttacked -= attackSpeedDelay;

                // Get direction from player to mouse cursor
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);
                Vector2 dirAim = mousePos - pos;
                if (dirAim != Vector2.Zero)
                    dirAim.Normalize(); // Normalize to get direction only

                // Attack in the direction of the mouse cursor with the player's current position
                Attack(dirAim);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height), Color.White);
        }
    }
}
