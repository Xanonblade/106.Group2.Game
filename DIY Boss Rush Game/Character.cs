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
        //private Rectangle rect;
        //public Rectangle Rect { get => rect; }
        public static Rectangle rect;
        public static Vector2 pos;
        private Texture2D texture;
        private int currHealth;
        //private Weapon[] currWeapons;

        // Stats set by picker
        public int HealthStat { get; private set; }
        public int DamageStat { get; private set; }

        public int SpeedStat { get; private set; }
        public int CritStat { get; private set; }

        /// <summary>
        /// Sets defaults mostly based on what player chooses in the picker
        /// </summary>
        /// <param name="rect">keep X and Y as 0,0, we only need width and height</param>
        /// <param name="texture"></param>
        /// <param name="healthStat"></param>
        /// <param name="damageStat"></param>
        /// <param name="speedStat"></param>
        /// <param name="critStat"></param>
        public Character(Rectangle rect, Vector2 pos, Texture2D texture, int healthStat, int damageStat, int speedStat, int critStat)
        {
            Character.rect = rect;
            Character.pos = pos;
            this.texture = texture;
            this.HealthStat = healthStat;
            this.DamageStat = damageStat;
            this.SpeedStat = speedStat;
            this.CritStat = critStat;
            this.currHealth = healthStat; // Set current health to max health at the start
        }

        /// <summary>
        /// Lowers characters health, calling dead methods is done in player and boss
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual void TakeDamage(int damage)
        {
            currHealth -= damage;
            if (currHealth < 0)
                currHealth = 0;
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
            spriteBatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, rect.Width, rect.Height), Color.White);
        }

        /// <summary>
        /// Add WEAPON parameter later
        /// </summary>
        /// <param name="damageMultiplier"> The ammount extra damage this boss or player should do</param>
        /// <param name="dir">UNIT VECTOR direction of attack</param>
        /// <param name="pos"></param>
        public void Attack(int damageMultiplier, Vector2 unitDir, Vector2 pos)
        {
            // Create bullet based on WEAPON and other parameters
            //Bullet newBullet = new Bullet(damage, attackTex, widthHeightRect, speed, unitDir, pos, radius);
        }
    }
}
