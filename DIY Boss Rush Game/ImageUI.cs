using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIY_Boss_Rush_Game
{
    public class ImageUI
    {
        // Fields
        private Rectangle rectangle;
        private Texture2D texture;

        // Properties

        /// <summary>
        /// Getter and setter for rectangle
        /// </summary>
        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        /// <summary>
        /// Getter and setter for the width of the rectangle, which is used for scaling the image
        /// </summary>
        public int Width { get { return rectangle.Width; } set { rectangle.Width = value; } }

        /// <summary>
        /// Getter for texture
        /// </summary>
        public Texture2D Texture { get { return texture; } }

        // Constructor

        /// <summary>
        /// Creates default object with given parameters
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="texture"></param>
        public ImageUI(Rectangle rectangle, Texture2D texture)
        {
            this.rectangle = rectangle;
            this.texture = texture;
        }

        /// <summary>
        /// Draws the UI element on the screen using the given SpriteBatch
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);

        }
    }
}
