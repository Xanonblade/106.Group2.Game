using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Button Class
namespace DIY_Boss_Rush_Game
{
    internal class Button
    {
        // Fields

        // Holds the position, and clickable area for the rectangle
        private Rectangle rect;

        // Holds the Text on the sprite
        private string text;

        // Holds the button sprite
        private Texture2D sprite;
        private Microsoft.Xna.Framework.Rectangle rectangle;
        private string v;
        private Texture2D buttonSprite;

        // Properties

        /// <summary>
        /// Getter for rect variable
        /// </summary>
        public Rectangle Rect { get { return rect; } }

        // Constructor

        /// <summary>
        /// Constructor for a basic button
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="text"></param>
        /// <param name="sprite"></param>
        public Button(Rectangle rect, string text, Texture2D sprite)
        {
            this.rect = rect;
            this.text = text;
            this.sprite = sprite;
        }

        // Methods

        /// <summary>
        /// Checks the mouse position and state with the button
        /// </summary>
        /// <returns></returns>
        public bool Click()
        {
            // Get Mouse state
            MouseState ms = Mouse.GetState();

            // Check if the mouse was left clicked
            if (ms.LeftButton == ButtonState.Pressed)
            {
                // Check if the mouse is over the button
                if (ms.X >= rect.X && ms.X <= rect.X + rect.Width && ms.Y >= rect.Y && ms.Y <= rect.Y + rect.Width)
                {
                    return true;
                }
            }
            // In all other cases, return false
            return false;
        }
    }
}
