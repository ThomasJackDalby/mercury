using Mercury.Core;
using Sloth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Verifier
{

    class Verifier
    {
        private static Logger logger = Logger.GetLogger(typeof(Verifier));

        static void Main(string[] args)
        {
            logger.Info("Verifier");

            int width = 10;
            int height = 10;
            int numPoints = 5;
            int[][] map = Map.Create(width, height);
            Map.Save("map.txt", map);
            int[][] points = Route.Create(numPoints, width, height);
            Route.Save("route.txt", points, true);
            //drawMap(map, points, null);

            logger.Info("Finished.");
        }

        private static void assertRouteIsValid(int[][] route)
        {
            try
            {
                for (int i = 1; i < route.Length; i++)
                {
                    int[] current = route[i];
                    int[] previous = route[i - 1];
                    int xDifference = Math.Abs(current[0] - previous[0]);
                    int yDifference = Math.Abs(current[1] - previous[1]);

                    // Can only step one in any direction
                    if (xDifference > 1) throw new Exception("X position differs by more than 1");
                    if (yDifference > 1) throw new Exception("Y position differs by more than 1");

                    // Can not step diagonally
                    if (xDifference == 1 && yDifference == 1) throw new Exception("Cannot move diagonally");
                }

            }

            catch (Exception e)
            {

                throw new Exception("Route is invalid", e);
            }

        }
        private static void assertRouteVisitsPoints(int[][] route, int[][] points)
        {

            int currentIndex = 0;
            for (int i = 1; i < route.Length; i++)
            {

                if (ArrayTools.AreEqual(route[i], points[currentIndex]))
                {

                    currentIndex

                    ++;
                    if (currentIndex >= points.Length) return;
                }

            }

            throw new Exception("Route does not visit all points in order.");
        }

    }
}

