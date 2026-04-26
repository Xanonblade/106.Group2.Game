using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

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
        private readonly float critMultiplier = 4; // Helps scale crits to be percentage based
        private float timeSinceAttacked = 0.0f;
        private Random rng;
        private KeyboardState previousKeyboardState; // Holds previousKeyboardState for a single click function
        private float staminaTimer = 3f; // Time to reduce speed
        private float resetSpeed; // Holds the reducedSpeed stat to slow down the player
        public bool IsSlowed { get; set; }

        // Hold the max stamina and current stamina of the player, and the rate at which stamina regenerates
        private int maxStamina;
        private int currStamina;

		//Status effects
		public bool IsInfected { get; set; }
        private float infectedTimer;

        // Holds the amount of time for a dash
        private float dashWindow = .2f;
        private bool isDashing = false;

        /// <summary>
        /// Getter for MaxStamina
        /// </summary>
        public int MaxStamina { get { return maxStamina; } }

        /// <summary>
        /// Getter and setter for CurrStamina
        /// </summary>
        public int CurrStamina { get { return currStamina; }  set { currStamina = value; } }


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
            //Skilltree values
            IsSlowed = false;
            maxStamina = 303;
            currStamina = maxStamina;
            IsInfected = false;
            infectedTimer = 1.5f;

            rng = new Random();
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
            bool isCrit = false;
			// Crit stat is between 0.5 and 2
			if (rng.Next(100) < CritStat * critMultiplier) // If random number is less than crit stat, it's a crit
            {
                currSpeed *= 1.5f; // Increase bullet speed for crits
                currDamage *= 2; // Double damage for crits
                isCrit = true;
			}

            // Shoot two if multishot
            if (base.Multishot)
            {
                float damage = DamageStat * attackMultiplier * 3 / 4; // Reduce damage for multishot bullets
                int offset = 15;
                Vector2 perpendicular = Vector2.Rotate(dir, (float)Math.PI / 2) * offset;
				base.bulletManager.CreateBullet(currSpeed, damage, Character.BulletTexture, dir, new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2) + perpendicular, bulletRadius, true, isCrit);
				base.bulletManager.CreateBullet(currSpeed, damage, Character.BulletTexture, dir, new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2) - perpendicular, bulletRadius, true, isCrit);
			}
            else
                base.bulletManager.CreateBullet(currSpeed, DamageStat * attackMultiplier, Character.BulletTexture, dir, new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2), bulletRadius, true, isCrit);
        }

        /// <summary>
        /// Keyboard and mouse input, still calls Character update
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Does nothing currently can add in character if makes sense

			KeyboardState currState = Keyboard.GetState();

            // Check if the stamina ever hits zero, speed temporarily decreases
            if (currStamina < 0 || IsSlowed)
            {
                // Set to resetSpeed to change later
                if (!IsSlowed)
                    resetSpeed = SpeedStat;

                // set to slow down if stamina == 0
                IsSlowed = true;

                SpeedStat = 3;

                staminaTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (staminaTimer <= 0)
                {
                    IsSlowed = false;
                    currStamina++;
                    staminaTimer = 3f;

                    SpeedStat = resetSpeed;
                }

            }
            // Update current stamina when shift isn't held down
            else if (currStamina <= maxStamina && !currState.IsKeyDown(Keys.LeftShift))
            {
                currStamina += 1;
            }
            
            //Virus status effect
            if (IsInfected)
            {
                //Deal damage
                CurrHealth -= MaxHealth*.0015f;

                infectedTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (infectedTimer <= 0)
                {
                    IsInfected = false;
                    infectedTimer = 1.5f;
                }
            }

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

            // Sprint
            if (Sprint && currStamina > 0 && currState.IsKeyDown(Keys.LeftShift) )
            {
                pos += movement / 2;
                currStamina -= 4;
            }

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

            // Dash

            // Make a single click method
            if (Dash && currState.IsKeyDown(Keys.Space) && previousKeyboardState != currState && currStamina - 150 >= 0)
            {
                // Reduce stamina
                currStamina -= 150;

                // let game know that the player is dashing
                isDashing = true;
            }

            //
            if (isDashing)
            {
                // Find the relation between the mouse and the player
                Vector2 direction = new Vector2();

                direction.X = mouseState.X - pos.X;
                direction.Y = mouseState.Y - pos.Y;

                // Normalize to get direction only
                direction.Normalize();

                direction *= SpeedStat * speedMultiplier * 4;
                pos += direction;

                dashWindow -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Reset dash
                if (dashWindow < 0)
                {
                    dashWindow = .2f;
                    isDashing = false;
                }
            }

            // Collected previous keyboard state to do single click
            previousKeyboardState = currState;

        }

        /// <summary>
        /// This override handles status effects from bullets before calling base
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="statusEffect"></param>
        public override void CollideWithBullet(float damage, BulletState statusEffect)
        {
            //Status effects from bullets
            switch (statusEffect)
            {
                case BulletState.Shock:
                    //Slows player by reusing stamina exhaustion mechanic
                    currStamina = -1;
                    break;
                case BulletState.Virus:
                    //Damage over time
                    IsInfected = true;
                    break;
			}

            //Damage effect
            base.CollideWithBullet(damage, statusEffect);
        }

        /// <summary>
        /// Draws with tint
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height), CurrTint);
        }
    }
}
