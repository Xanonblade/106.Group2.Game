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
        private readonly int speedMultiplier = 1; // Helps scale movement
        private readonly int attackMultiplier = 1; // Helps scale attack
        private readonly float attackSpeedDelay = 0.5f; // Helps set attack speed
        private float timeSinceAttacked = 0.0f;

        /// <summary>
        /// Sets player specifics (static pos) and calls base constructor for character stats and texture and rectangle
        /// Most of the player constructor arguments should be set in picker instead of here once picker is done
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="tex"></param>
        public Player(Vector2 pos, Texture2D tex) : base(10, 10, 5, 5)
        {
            Player.pos = pos;
            Player.texture = tex;
        }
        private void Attack(Vector2 dir)
        {
           
            float bulletSpeed = 1000f;
            float bulletRadius = Character.BulletTexture.Width / 2;

            // The "15"s are just hardcoded magic numbers, I don't quite understand why those values work
            base.bulletManager.CreateBullet(bulletSpeed, DamageStat * attackMultiplier, Character.BulletTexture, dir, new Vector2(pos.X + texture.Width/2, pos.Y + texture.Height/2), bulletRadius, true);
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
                pos.Y -= 1;
            }
            if (currState.IsKeyDown(Keys.S))
            {
                pos.Y += 1;
            }
            if (currState.IsKeyDown(Keys.A))
            {
                pos.X -= 1;
            }
            if (currState.IsKeyDown(Keys.D))
            {
                pos.X += 1;
            }
            if (movement != Vector2.Zero)
                movement.Normalize(); // Normalize to prevent faster diagonal movement
            movement *= SpeedStat * speedMultiplier; // Scale movement by speed stat and multiplier
            pos += movement; // Update player Position

            // Update timer
            timeSinceAttacked += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Attack if timer available
            // Screen size - wall size
            int screenWidth = 1920 - 64;
            int screenHeight = 1024 - 64;

            // Clamp the player's position so it can't move outside of the screen
            pos.X = Math.Clamp(pos.X, 64, screenWidth - texture.Width);
            pos.Y = Math.Clamp(pos.Y, 64, screenHeight - texture.Height);

            // Attacking
            MouseState mouseState = Mouse.GetState();
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
