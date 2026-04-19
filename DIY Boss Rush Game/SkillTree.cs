using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShapeUtils;
using System;
using System.Collections.Generic;
using System.IO;

namespace DIY_Boss_Rush_Game
{
    internal class SkillTree
    {
        private static SkillTree instance = null;
        private Dictionary<string, List<Vertex>> links;
        private List<Vertex> vertices;
        private int currentPoints;
        private int maxPoints;
        private MouseState lastMouseState;

        public int CurrentPoints { get => currentPoints; }
        public int MaxPoints { get => maxPoints; }

        public static SkillTree Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SkillTree();
                }

                return instance;
            }
        }

        private SkillTree()
        {
            links = new Dictionary<string, List<Vertex>>();
            vertices = new List<Vertex>();
            currentPoints = 2;
            maxPoints = 0;
        }

        public void AddPoint()
        {
            currentPoints++;
            maxPoints++;
        }

        public void WipeTree()
        {
            maxPoints = 0;
            RespecTree();
        }

        public void RespecTree()
        {
            foreach(Vertex vertex in vertices)
            {
                vertex.IsUnlocked = false;
            }

            vertices[0].IsUnlocked = true;

            currentPoints = maxPoints;
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                if (currentPoints < 1) return;
                foreach (Vertex vertex in vertices)
                {
                    if (vertex.Rect.Contains(mousePos))
                    {
                        vertex.onClick();
                        currentPoints--;
                        Console.WriteLine($"Vertex {vertex.Name} was pressed");
                    }
                }
            }

            lastMouseState = mouseState;
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            ShapeBatch.Begin(graphicsDevice);

            foreach (Vertex vertex in vertices)
            {
                Vertex parent = vertex.Parent;
                if (parent != null)
                {
                    Vector2 start = new Vector2(vertex.Rect.X + vertex.Radius, vertex.Rect.Y + vertex.Radius);
                    Vector2 end = new Vector2(parent.Rect.X + parent.Radius, parent.Rect.Y + parent.Radius);
                    ShapeBatch.Line(start, end, Color.White);
                }
                
            }
            foreach (Vertex vertex in vertices)
                vertex.Draw();



            ShapeBatch.End();
        }

        public List<Vertex> GetUnlocked()
        {
            return null;
        }

        public bool CheckIfUnlocked(string name)
        {
            return false;
        }

        public void ReadData()
        {
            StreamReader sr = new StreamReader("../../../TreeData.txt");

            string line = "";

            List<string> lines = new List<string>();

            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
                string[] data = line.Split("|");
                vertices.Add(new Vertex(data[0], data[1], data[2]));
            }

            for (int i = 0; i < lines.Count; i++)
            {
                string name = lines[i].Split("|")[0];
                string[] relations = lines[i].Split("|")[3].Split(":");

                Vertex curr = FindVertexWithName(name);

                if (!links.ContainsKey(name))
                    links.Add(name, new List<Vertex>());

                for (int j = 0; j < relations.Length; j++)
                {
                    links[name].Add(FindVertexWithName(relations[j]));
                }
            }

            sr.Close();
            vertices[0].IsUnlocked = true;
            InitializeVerticies();
        }

        public Vertex FindVertexWithName(string name)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Name == name) return vertices[i];
            }

            return null;
        }

        private void InitializeVerticies()
        {
            Vector2 startPos = new Vector2(500, 100);
            int offsetY = 300;
            int offsetX = 300;

            // Base
            vertices[0].SetPosition((int)startPos.X, (int)startPos.Y);
            // Oiled Up Gears
            vertices[1].SetPosition((int)startPos.X - offsetX, (int)startPos.Y + offsetY);
            vertices[1].Parent = vertices[0];
            // Boost Dash
            vertices[2].SetPosition((int)startPos.X - offsetX, (int)startPos.Y + offsetY * 2);
            vertices[2].Parent = vertices[1];
            // More bullets
            vertices[3].SetPosition((int)startPos.X, (int)startPos.Y + offsetY);
            vertices[3].Parent = vertices[0];
            // This really is a bullet... hell
            vertices[4].SetPosition((int)startPos.X, (int)startPos.Y + offsetY * 2);
            vertices[4].Parent = vertices[3];
            // Shock shot
            vertices[5].SetPosition((int)startPos.X + offsetX, (int)startPos.Y + offsetY);
            vertices[5].Parent = vertices[0];
            // Viral Shot
            vertices[6].SetPosition((int)startPos.X + offsetX, (int)startPos.Y + offsetY * 2);
            vertices[6].Parent = vertices[5];
        }
    }
}
