using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShapeUtils;

namespace DIY_Boss_Rush_Game
{
    internal class Vertex
    {
        public bool IsUnlocked { get; set; }
        public string Name { get; set; }
        public string PDescription { get; set; }
        public string BDescription { get; set; }
        public Rectangle Rect { get; set; }

        public Vertex(string name, string pDescription, string bDescription)
        {
            IsUnlocked = false;
            Name = name;
            PDescription = pDescription;
            BDescription = bDescription;
            Rect = new Rectangle();
        }

        public void onClick()
        {
            IsUnlocked = true;
        }

        public void Draw()
        {
            Color drawColor = Color.DarkGray;
            if (IsUnlocked) drawColor = Color.Green;

            ShapeBatch.Circle(new Vector2(Rect.X, Rect.Y), 50, drawColor);
        }

    }
}
