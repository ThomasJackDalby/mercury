using Sloth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
    public static class Route
    {
        private static Logger logger = Logger.GetLogger(typeof(Route));
        public static int[][] Create(int number, int width, int height)
        {
            Random r = new Random();
            int[][] points = new int[number][];
            for (int i = 0; i < number; i++)
            {
                int[] point = { r.Next(width), r.Next(height) };
                points[i] = point;
            }
            return points;
        }
        public static int[][] Load(string filename, bool withId)
        {
            logger.Info("Loading route from {0}", filename);

            string[] data = File.ReadAllLines(filename);
            int[][] route = new int[data.Length][];

            int[] indexs = (withId) ? new int[] { 1, 2 } : new int[] { 0, 1 };

            for (int i = 0; i < data.Length - 1; i++)
            {
                string line = data[i + 1];
                string[] coords = line.Split(',');
                int x = Int32.Parse(coords[indexs[0]].Trim());
                int y = Int32.Parse(coords[indexs[1]].Trim());
                route[i] = new int[] { x, y };
            }

            logger.Info("Loaded route of {0} points", route.Length);
            return route;
        }
        public static void Save(string filename, int[][] route)
        {
            Save(filename, route, false);
        }
        public static void Save(string filename, int[][] route, bool withId)
        {
            string[] lines = new string[route.Length + 1];
            string format = (withId) ? "{2}, {0}, {1}" : "{0}, {1}";
            lines[0] = (withId) ? "id, x, y" : "x, y";
            for (int i = 0; i < route.Length; i++) lines[i + 1] = String.Format(format, i, route[i][0], route[i][1]);
            File.WriteAllLines(filename, lines);
        }
    }
}
