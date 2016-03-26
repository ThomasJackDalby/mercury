using System;
using System.Collections.Generic;
using Drawing = System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sloth;

namespace Mercury.Core
{
    public static class Map
    {
        private static Logger logger = Logger.GetLogger(typeof(Map));
        public static int[][] Create(int width, int height)
        {

            Random r = new Random();
            int[][] map = new int[height][];
            for (int i = 0; i < height; i++)
            {
                int[] line = new int[width];
                for (int j = 0; j < width; j++) line[j] = r.Next(10);
                map[i] = line;
            }

            return map;
        }
        public static void Save(string filename, int[][] map)
        {
            int height = map[0].Length;
            int width = map.Length;
            string[] contents = new string[height];
            for (int i = 0; i < height; i++)
            {
                char[] line = new char[width];
                for (int j = 0; j < width; j++) line[j] = map[i][j].ToString()[0];
                contents[i] = new string(line);
            }
            File.WriteAllLines(filename, contents);
        }
        public static int[][] Load(string filename)
        {
            logger.Info("Loading map from {0}", filename);

            string[] contents = File.ReadAllLines(filename);
            int height = contents.Length;
            int width = contents[0].Length;

            int[][] map = new int[height][];
            for (int i = 0; i < height; i++)
            {
                string content = contents[i];      
                int[] line = new int[width];
                for (int j = 0; j < width; j++) line[j] = Int16.Parse(content[j].ToString());
                map[i] = line;
            }

            logger.Info("Loaded map of {0} x {1}", width, height);
            return map;
        }
        private static void Draw(string filename, int[][] map)
        {
            Draw(filename, map, null, null);
        }
        private static void Draw(string filename, int[][] map, int[][] route, int[][] actual)
        {
            int height = map[0].Length;
            int width = map.Length;
            Drawing.Bitmap bitmap = new Drawing.Bitmap(width, height);
            double delta = (255.0 / 10);
            double hue = 0;
            double sat = 0;
            //double val = 255;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int value = map[i][j];
                    double val = value * delta;
                    IList<double> rgb = Tools.HSVtoRGB(new List<double>() { hue, sat, val });
                    Drawing.Color colour = Drawing.Color.FromArgb(255, (byte)rgb[0], (byte)rgb[1], (byte)rgb[2]);
                    bitmap.SetPixel(j, i, colour);
                }
            }

            Drawing.Graphics g = Drawing.Graphics.FromImage(bitmap);
            Drawing.Pen p = new Drawing.Pen(Drawing.Brushes.Red);
            for (int i = 1; i < route.Length; i++)
            {
                g.DrawLine(p, new Drawing.Point(route[i - 1][0], route[i - 1][1]), new Drawing.Point(route[i][0], route[i][1]));
            }
            bitmap.Save(filename);
        }
    }
}
