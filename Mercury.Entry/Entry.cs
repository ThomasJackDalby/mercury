using Mercury.Core;
using Sloth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury
{
    public static class Entry
    {
        private static Logger logger = Logger.GetLogger(typeof(Entry));

        static void Main(string[] args)
        {
            Logger.MinLevel = Level.INFO;

            logger.Info("Mercury.Entry");
            int[][] map = Map.Load(args[0]);
            int[][] requiredRoute = Route.Load(args[1], true);
            int[][] route = calculateShortestRouteBetweenTwoPoints(map, requiredRoute[0], requiredRoute[1]);
        }
        
        private static void calcaulteShortedRouteFollowingPoints(int[][] map, int[][] requiredRoute)
        {
            logger.Info("Calculating shortest route for list of {0} points", requiredRoute.Length);
            for (int i = 0; i < requiredRoute.Length; i++)
            {
                int[] start = requiredRoute[i - 1];
                int[] end = requiredRoute[i];
            }
        }
        private static int[][] calculateShortestRouteBetweenTwoPoints(int[][] map, int[] start, int[] end)
        {
            logger.Info("Calculating distance between {0} and {1}", Point.GetString(start), Point.GetString(end));
            int[][] distanceMap = calculateDistanceMap(start, map);
            int[][] route = calculateShortestRouteThroughDistanceMap(end, start, distanceMap);
            return route;
        }
        private static int[] addPoints(int[] a, int[] b)
        {
            return new int[] { a[0] + b[0], a[1] + b[1] };
        }
        private static int[][] calculateDistanceMap(int[] point, int[][] map)
        {
            logger.Info("Calculating a distance map...");
            int[][] distanceMap = new int[map.Length][];
            for (int i = 0; i < map.Length; i++)
            {
                distanceMap[i] = new int[map[i].Length];
                for (int j = 0; j < map[i].Length; j++)
                {
                    distanceMap[i][j] = Int32.MaxValue;
                }
            }

            logger.Info("Generating point list.");
            List<int[]> pointQueue = new List<int[]>();
            pointQueue.Add(point);
            distanceMap[point[1]][point[0]] = 0;

            // check neighbouring points
            while (pointQueue.Count > 0)
            {
                int[] currentPoint = pointQueue[0];
                int cx = currentPoint[0];
                int cy = currentPoint[1];
                int currentDistance = distanceMap[cy][cx];
                logger.Debug("Processing {0}", Point.GetString(currentPoint));

                // Add connected points if needed
                int[][] directions = getDirections();
                for (int i = 0; i < 3; i++)
                {
                    int[] pointToCheck = addPoints(currentPoint, directions[i]);
                    int px = pointToCheck[0];
                    int py = pointToCheck[1];
                    logger.Debug("Checking {0}", Point.GetString(pointToCheck));

                    // check point is inside map
                    if (px < 0 || px >= map.Length || py < 0 || py >= map[0].Length) continue;

                    int effort = map[py][px];
                    int distance = currentDistance + effort;

                    if (distance < distanceMap[py][px])
                    {
                        distanceMap[py][px] = distance;
                        pointQueue.Add(pointToCheck);
                    }
                }

                pointQueue.Remove(currentPoint);
            }
            logger.Info("Created distance map.");
            return distanceMap;
        }
        private static int[][] getDirections()
        {
            int[][] directions = new int[4][];
            directions[0] = new int[] { 1, 0 };
            directions[1] = new int[] { 0, -1 };
            directions[2] = new int[] { -1, 0 };
            directions[3] = new int[] { 0, 1 };
            return directions;
        }
        private static int[][] calculateShortestRouteThroughDistanceMap(int[] b, int[] a, int[][] distanceMap)
        {
            logger.Info("Calculating route through distance map...");
            List<int[]> route = new List<int[]>();
            int[] currentPoint = b;
            while (!currentPoint.Equals(a))
            {
                int[] distanceOptions = new int[4];
                for (int i = 0; i < distanceOptions.Length; i++)
                {
                    distanceOptions[i] = Int32.MaxValue;
                }
                int[][] pointOptions = new int[4][];
                int[][] directions = getDirections();
                for (int i = 0; i < 3; i++)
                {
                    int[] pointToCheck = addPoints(currentPoint, directions[i]);
                    int px = pointToCheck[0];
                    int py = pointToCheck[1];

                    // check point is inside map
                    if (px < 0 || px > distanceMap.Length || py < 0 || py < distanceMap[0].Length) continue;

                    distanceOptions[i] = distanceMap[py][px];
                    pointOptions[i] = pointToCheck;
                }

                int minIndex = calculateMinIndex(distanceOptions);
                int[] bestPoint = pointOptions[minIndex];

                route.Add(bestPoint);
                currentPoint = bestPoint;
            }
            logger.Info("Calculated route.");
            return route.ToArray();
        }
        private static int calculateMinValue(params int[] nums)
        {
            int minIndex = calculateMinIndex(nums);
            return nums[minIndex];
        }
        private static int calculateMinIndex(int[] inputs)
        {
            int minValue = inputs[0];
            int minIndex = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] < inputs[minIndex])
                {
                    minValue = inputs[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }
    }
}
