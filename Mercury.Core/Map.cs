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
                for (int j = 0; j < width; j++) line[j] = r.Next(1, 10);
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
        public static void Draw(string filename, int[][] map)
        {
            Draw(filename, map, null, null);
        }
        public static void Draw(string filename, int[][] map, Point[] route, Point[] actual)
        {
            Draw(filename, map, route, actual, true);
        }
        
        public static void Draw(string filename, int[][] map, Point[] route, Point[] actual, bool grayscale)
        {
            int height = map[0].Length;
            int width = map.Length;
            Drawing.Bitmap bitmap = new Drawing.Bitmap(width, height);

            int min = ArrayTools.Min(map);
            int max = ArrayTools.Max(map, Int32.MaxValue);

            double range = (grayscale) ? 1 : 360.0;
            double delta = range / (max - min);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int value = map[y][x];
                    double hue;
                    double sat;
                    double val;

                    if (value >= Int32.MaxValue)
                    {
                        hue = 0;
                        sat = 1;
                        val = 0;
                    }
                    else
                    {
                        if (grayscale)
                        {
                            hue = 0;
                            sat = 0;
                            val = (value * delta);
                        }
                        else
                        {
                            hue = (value * delta);
                            sat = 1;
                            val = 1;
                        }
                    }

                    double[] rgb = Tools.HSVtoRGB(hue, sat, val);
                    Drawing.Color colour = Drawing.Color.FromArgb(255, (byte)(rgb[0] * 255), (byte)(rgb[1] * 255), (byte)(rgb[2] * 255));
                    bitmap.SetPixel(x, y, colour);
                }
            }

            Drawing.Graphics g = Drawing.Graphics.FromImage(bitmap);
            Drawing.Pen p = (grayscale) ? new Drawing.Pen(Drawing.Brushes.Red) : new Drawing.Pen(Drawing.Brushes.Black);
            Drawing.Color actualColour = Drawing.Color.Blue;
            if (route != null)
            {
                for (int i = 1; i < route.Length; i++)
                {
                    g.DrawLine(p, new Drawing.Point(route[i - 1].X, route[i - 1].Y), new Drawing.Point(route[i].X, route[i].Y));
                }
            }
            if (actual != null)
            {
                for (int i = 0; i < actual.Length; i++)
                {
                    bitmap.SetPixel(actual[i].X, actual[i].Y, actualColour);
                }
            }
            bitmap.Save(filename);
        }
        public static void Log(int[][] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                string[] line = new string[map[i].Length];
                for (int j = 0; j < map[i].Length; j++)
                {
                    line[j] = map[i][j].ToString();
                }
                logger.Info(String.Join(",", line));
            }
        }

        public static bool IsPointInsideMap(int[][] map, Point point)
        {
            if (point.X < 0) return false;
            else if (point.X >= map.Length) return false;
            else if (point.Y < 0) return false;
            else if (point.Y >= map[0].Length) return false;
            else return true;
        }
    }
}
