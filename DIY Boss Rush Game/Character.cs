using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DIY_Boss_Rush_Game
{
    /// <summary>
    /// The class that outlines characters in the scene, such as the player and the bosses. It will contain stats such as health, attack power, and defense.
    /// </summary>
    internal class Character
    {
        public static Texture2D BulletTexture;
        public float CurrHealth { get; set; }
        public int MaxHealth { get; set; }
        //private Weapon[] currWeapons;

        // Stats set by picker
        public float HealthStat { get; set; }
        public float DamageStat { get; set; }

        public float SpeedStat { get; set; }
        public float CritStat { get; set; }
        public bool IsDead => CurrHealth <= 0;

        public bool Multishot { get => SkillTree.Instance.CheckIfUnlocked("More bullets!");}
        public bool Richochet { get => SkillTree.Instance.CheckIfUnlocked("This really is a bullet... hell"); }
        public bool Sprint { get => SkillTree.Instance.CheckIfUnlocked("Oiled up gears"); }
        public bool Dash { get => SkillTree.Instance.CheckIfUnlocked("Boost dash"); }

        public BulletManager bulletManager { get; set; }

		public Color CurrTint { get; private set; } = Color.White;
		private float tintTimer = 0.1f;

		/// <summary>
		/// Sets defaults mostly based on what player chooses in the picker
		/// </summary>
		/// <param name="healthStat"></param>
		/// <param name="damageStat"></param>
		/// <param name="speedStat"></param>
		/// <param name="critStat"></param>
		public Character(float healthStat, float damageStat, float speedStat, float critStat)
        {
            this.HealthStat = healthStat;
            this.DamageStat = damageStat;
            this.SpeedStat = speedStat;
            this.CritStat = critStat;
            MaxHealth = (int)(healthStat * 50f); // Set current health to max health at the start
            CurrHealth = MaxHealth;

            // Set skill tree values
            SkillTree skillTree = SkillTree.Instance;
        }


        /// <summary>
        /// Lowers characters health, calling dead methods is done in player and boss
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="statusEffect">Takes in a status effect from the bullet when colliding</param>
        public virtual void CollideWithBullet(float damage, BulletState statusEffect)
        {
			//Status effects from bullets
			switch (statusEffect)
			{
				case BulletState.Shock:
					CurrTint = Color.Yellow;
					break;
				case BulletState.Virus:
					CurrTint = Color.MediumPurple;
					break;
				case BulletState.Crit:
					CurrTint = Color.Red;
					break;
				case BulletState.Neutral:
					CurrTint = Color.LightSalmon;
					break;
			}

			//Damage
			CurrHealth -= damage;
            if (CurrHealth < 0)
                CurrHealth = 0;
        }

        /// <summary>
        /// Placeholder for update method, will be overridden in player and boss, but we can put some shared code here if needed
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
			// This will be overridden in player and boss, but we can put some shared code here if needed
			// Tints
			if (CurrTint != Color.White)
			{
				tintTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (tintTimer <= 0)
				{
					CurrTint = Color.White;
					tintTimer = 0.1f;
				}
			}
		}

        /// <summary>
        /// Override if necessary, but this will be the default draw method for characters. Player and boss can add to this with an override and a base call
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
			// Should have had a draw here but oh well, this will be overridden in player and boss, but we can put some shared code here if needed
		}

	}
}
