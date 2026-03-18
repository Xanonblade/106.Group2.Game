using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        private BulletManager bulletManager;

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

            bulletManager = BulletManager.Instance;
        }
        private void Attack(Vector2 dir)
        {
           
            float bulletSpeed = 1f;
            int bulletRadius = 3;

            //BulletManager.CreateBullet(bulletSpeed, DamageStat * attackMultiplier, Character.BulletTexture, dir, pos, bulletRadius, true);
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

            // Attacking
            MouseState mouseState = Mouse.GetState();
            // Add single click or reload functionality later, for now just hold left click to attack
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
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
