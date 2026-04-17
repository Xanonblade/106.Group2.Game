using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShapeUtils;

namespace DIY_Boss_Rush_Game
{
    internal class Vertex
    {
        private Rectangle rect;

        public bool IsUnlocked { get; set; }
        public string Name { get; set; }
        public string PDescription { get; set; }
        public string BDescription { get; set; }
        public Rectangle Rect { get => rect; set => rect = value; }
        public int Radius { get; set; }
        public Vertex Parent { get; set; }

        public Vertex(string name, string pDescription, string bDescription)
        {
            IsUnlocked = false;
            Name = name;
            PDescription = pDescription;
            BDescription = bDescription;
            Radius = 100;
            rect = new Rectangle(0, 0, Radius * 2, Radius * 2);
            Parent = null;
        }

        public void onClick()
        {
            if (Parent != null && Parent.IsUnlocked)
                IsUnlocked = true;
        }

        public void Draw()
        {
            Color drawColor = Color.DarkGray;
            if (IsUnlocked) drawColor = Color.Green;

            ShapeBatch.Circle(new Vector2(Rect.X + Radius, Rect.Y + Radius), Radius, drawColor);
        }

        public void SetPosition(int x, int y)
        {
            rect.X = x;
            rect.Y = y;
        }
    }
}
