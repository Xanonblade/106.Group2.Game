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
        // Fields
        private static SkillTree instance = null;
        private Dictionary<string, List<Vertex>> links;
        private List<Vertex> vertices;
        private int currentPoints;
        private int maxPoints;
        private MouseState lastMouseState;
        private SpriteFont titleText, bodyText;
        private Vertex highlightedVertex;
        private Button respecButton;
        private Button continueButton;

        public bool continueToCustomization;

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

        // Constructor
        private SkillTree()
        {
            links = new Dictionary<string, List<Vertex>>();
            vertices = new List<Vertex>();
            currentPoints = 0;
            maxPoints = currentPoints;
            highlightedVertex = null;
            continueToCustomization = false;
        }

        /// <summary>
        /// Sets up the fonts for skill tree to display with
        /// </summary>
        /// <param name="titleText"></param>
        /// <param name="bodyText"></param>
        public void Initialize(SpriteFont titleText, SpriteFont bodyText)
        {
            this.titleText = titleText;
            this.bodyText = bodyText;
        }

        /// <summary>
        /// Adds a single point to current points and max points
        /// </summary>
        public void AddPoint()
        {
            currentPoints++;
            maxPoints++;
        }

        /// <summary>
        /// Completely resets the tree without giving back skill points
        /// </summary>
        public void WipeTree()
        {
            maxPoints = 0;
            RespecTree();
        }

        /// <summary>
        /// Resets the tree and gives all skill points back
        /// </summary>
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

            highlightedVertex = null;

            // Check if any of the vertices are being pressed
            foreach (Vertex vertex in vertices)
            {
                if (vertex.Rect.Contains(mousePos))
                {
                    highlightedVertex = vertex;
                    if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        // Check if it can be unlocked
                        if (vertex.CanUnlock && currentPoints > 0)
                        {
                            vertex.onClick();
                            currentPoints--;
                        }
                    } 
                }
            }

            lastMouseState = mouseState;
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch sb)
        {
            ShapeBatch.Begin(graphicsDevice);

            // Draw all of the lines between vertices
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

            // Draw all of the vertices
            foreach (Vertex vertex in vertices)
                vertex.Draw();

            // Draw the display info of the highlighted vertex
            if (highlightedVertex != null)
            {
                sb.DrawString(titleText, highlightedVertex.Name,
                    new Vector2(1200, 20), Color.White);

                string[] lines = highlightedVertex.PDescription.Split("\\n");

                for (int i = 0; i < lines.Length; i++)
                    sb.DrawString(bodyText, lines[i],
                        new Vector2(1200, 200 + i * 40), Color.White);

                lines = highlightedVertex.BDescription.Split("\\n");

                for (int i = 0; i < lines.Length; i++)
                    sb.DrawString(bodyText, lines[i],
                        new Vector2(1200, 400 + i * 40), Color.White);
            }

            // Display current skill points
            sb.DrawString(titleText, $"Current Skill Points: {currentPoints}", Vector2.Zero, Color.White);

            ShapeBatch.End();
        }

        /// <summary>
        /// Gets a list of all unlocked vertices in the tree
        /// </summary>
        /// <returns>All unlocked vertices</returns>
        public List<Vertex> GetUnlocked()
        {
            List<Vertex> unlocked = new List<Vertex>();

            foreach(Vertex vertex in vertices)
            {
                if (vertex.IsUnlocked) unlocked.Add(vertex);
            }

            return unlocked;
        }

        /// <summary>
        /// Check if a certain vertex is unlocked
        /// </summary>
        /// <param name="name">Name of the vertex to check</param>
        /// <returns>True if the vertex is unlocked, false otherwise</returns>
        public bool CheckIfUnlocked(string name)
        {
            List<Vertex> unlocked = GetUnlocked();

            foreach(Vertex vertex in unlocked)
            {
                if (vertex.Name == name) return true;
            }


            return false;
        }

        /// <summary>
        /// Reads in all of the data from the tree data
        /// </summary>
        public void ReadData()
        {
            StreamReader sr = new StreamReader("../../../TreeData.txt");

            string line = "";

            List<string> lines = new List<string>();

            // Split the data
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
                string[] data = line.Split("|");
                vertices.Add(new Vertex(data[0], data[1], data[2]));
            }

            // Splite the links
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

        /// <summary>
        /// Gets a vertex from its name, case sensitive
        /// </summary>
        /// <param name="name">Name of the vertex to find</param>
        /// <returns>Vertex with correct name if found, null otherwise</returns>
        public Vertex FindVertexWithName(string name)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Name == name) return vertices[i];
            }

            return null;
        }

        /// <summary>
        /// Sets up all of the positons for all of the verticies 
        /// </summary>
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
