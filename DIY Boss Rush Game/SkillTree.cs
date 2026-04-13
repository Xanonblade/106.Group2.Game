using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DIY_Boss_Rush_Game
{
    internal class SkillTree
    {
        private static SkillTree instance;
        private Dictionary<string, List<Vertex>> links;
        private List<Vertex> vertices;
        private int currentPoints;
        private int maxPoints;

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
            currentPoints = 0;
            maxPoints = 0;

            ReadData();
        }

        public void WipeTree()
        {

        }

        public void RespecTree()
        {

        }

        public void Update()
        {

        }

        public void Draw(GraphicsDevice graphicsDevice)
        {

        }

        public List<Vertex> GetUnlocked()
        {
            return null;
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

                links.Add(name, new List<Vertex>());

                for (int j = 0; j < relations.Length; j++)
                {
                    links[name].Add(FindVertexWithName(relations[j]));
                }
            }

            sr.Close();
            Console.WriteLine();
        }

        public Vertex FindVertexWithName(string name)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Name == name) return vertices[i];
            }

            return null;
        }
    }
}
