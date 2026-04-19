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
        public int MaxHealth { get; private set; }
        //private Weapon[] currWeapons;

        // Stats set by picker
        public float HealthStat { get; set; }
        public float DamageStat { get; set; }

        public float SpeedStat { get; set; }
        public float CritStat { get; set; }
        public bool IsDead => CurrHealth <= 0;

        public bool Multishot { get; private set; }
        public bool Richochet { get; private set; }

		public BulletManager bulletManager { get; set; }

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
            Multishot = skillTree.CheckIfUnlocked("Multishot");
            Richochet = skillTree.CheckIfUnlocked("Richochet");
        }

        /// <summary>
        /// Lowers characters health, calling dead methods is done in player and boss
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual void TakeDamage(float damage)
        {
            CurrHealth -= damage;
            if (CurrHealth < 0)
                CurrHealth = 0;
            // Player and boss finish this method with an override and a base call
        }

        /// <summary>
        /// Placeholder for update method, will be overridden in player and boss, but we can put some shared code here if needed
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            // This will be overridden in player and boss, but we can put some shared code here if needed
        }

        /// <summary>
        /// Override if necessary, but this will be the default draw method for characters. Player and boss can add to this with an override and a base call
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }

    }
}
