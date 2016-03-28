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

        public static readonly Point[] Directions = getDirections();

        static void Main(string[] args)
        {
            Logger.MinLevel = Level.INFO;

            int width = 100;
            int height = 100;
            int numPoints = 10;

            Map.Save("map.txt", Map.Create(width, height));

            Point[] points = Route.Create(numPoints, width, height);
            Route.Save("route.txt", points, true);

            logger.Info("Mercury.Entry");
            int[][] map = Map.Load(args[0]);
            Point[] requiredRoute = Route.Load(args[1], true);
            
            // Task 3
            calculateShortestRouteToVisitAllPoints(map, points);


            //// Task 1
            //Point[] route1 = calculateShortestRouteBetweenTwoPoints(map, requiredRoute[0], requiredRoute[1]);
            //Route.Save("task1.txt", route1);
            //// Task 2
            //Point[] route2 = calcaulteShortedRouteFollowingPoints(map, requiredRoute);
            //Route.Save("task2.txt", route2);
            //Map.Draw("task2.bmp", map, requiredRoute, route2);
        }
        
        private static Point[] calculateShortestRouteToVisitAllPoints(int[][] map, Point[] requiredPoints)
        {
            logger.Info("Calculating shortest route to visit all {0} points", requiredPoints.Length);

            int[][] routeMap = calculateRouteMap(map, requiredPoints);

            Point[] finalRoute = new Point[0];
            return finalRoute;
        }

        private static Point[] calculateOrderToVisitPoints(int[][] routeMap, Point[] requiredPoints)
        {
            return bruteForce(routeMap, requiredPoints);
        }
        private static Point[] bruteForce(int[][] routeMap, Point[] pointsLeft, Point[] pointsVisited, int distance)
        {

            for (int i = 0; i < pointsLeft.Length; i++)
            {
                    


            }
        }

        private static int[][] calculateRouteMap(int[][] map, Point[] requiredPoints)
        {
            logger.Info("Calculating route map");
            int[][] routeMap = ArrayTools.Create(Int32.MaxValue, requiredPoints.Length, requiredPoints.Length);
            for (int i = 0; i < requiredPoints.Length; i++) routeMap[i][i] = 0;

            for (int i = 0; i < requiredPoints.Length; i++)
            {
                Point start = requiredPoints[i];
                logger.Info("Searching from {0}", start);

                Point[] ends = new Point[requiredPoints.Length - i - 1];
                for (int j = 0; j < requiredPoints.Length - i - 1; j++)
                {
                    ends[j] = requiredPoints[i + j + 1];
                }

                int[][] distanceMap = calculateDistanceMap(map, start, ends);

                for (int j = 0; j < ends.Length; j++)
                {
                    Point end = ends[j];
                    Point[] route = calculateShortestRouteThroughDistanceMap(distanceMap, start, end);

                    string filename = String.Format("[{0}.{1}].txt", i, i + j + 1);
                    Route.Save(filename, route);

                    int distance = distanceMap[end.Y][end.X];

                    routeMap[i][i + j + 1] = distance;
                    routeMap[i + j + 1][i] = distance;
                }
            }
            logger.Info("Finished.");
            return routeMap;
        }
        private static Point[] calculateShortedRouteFollowingPoints(int[][] map, Point[] requiredRoute)
        {
            logger.Info("Calculating shortest route for list of {0} points", requiredRoute.Length);
            Point[] totalRoute = new Point[0];
            for (int i = 1; i < requiredRoute.Length; i++)
            {
                Point start = requiredRoute[i - 1];
                Point end = requiredRoute[i];
                Point[] route = calculateShortestRouteBetweenTwoPoints(map, start, end);

                totalRoute = ArrayTools.Append(totalRoute, route);
            }
            logger.Info("Finished...");
            return totalRoute;
        }
        private static Point[] calculateShortestRouteBetweenTwoPoints(int[][] map, Point start, Point end)
        {
            logger.Info("Calculating distance between {0} and {1}", start.ToString(), end.ToString());
            int[][] distanceMap = calculateDistanceMap(map, start, end);
            Point[] route = calculateShortestRouteThroughDistanceMap(distanceMap, start, end);
            return route;
        }
        private static int[][] calculateDistanceMap(int[][] map, Point start, params Point[] ends)
        {
            logger.Info("Calculating a distance map...");
            int[][] distanceMap = ArrayTools.Create(Int32.MaxValue, map.Length, map[0].Length);

            List<Point> pointQueue = new List<Point>();
            pointQueue.Add(start);
            distanceMap[start.Y][start.X] = 0;

            List<Point> left = new List<Point>(ends);

            logger.Info("Processing points...");
            while (pointQueue.Count > 0)
            {
                int[] distances = (from p in pointQueue select distanceMap[p.Y][p.X]).ToArray();
                int minIndex = ArrayTools.MinIndex(distances);

                Point c = pointQueue[minIndex];

                for (int i = 0; i < left.Count; i++)
                {
                    if (c.Equals(left[i]))
                    {
                        left.Remove(left[i]);
                        break;
                    }
                }
                if (left.Count == 0) break;

                int currentDistance = distanceMap[c.Y][c.X];

                // Add connected points if needed        
                for (int i = 0; i < Directions.Length; i++)
                {
                    Point p = c + Directions[i];

                    // check point is inside map
                    bool isInside = Map.IsPointInsideMap(map, p);
                    if (!isInside) continue;

                    int effort = map[p.Y][p.X];
                    int newDistance = currentDistance + effort;

                    if (newDistance < distanceMap[p.Y][p.X])
                    {
                        distanceMap[p.Y][p.X] = newDistance;
                        pointQueue.Add(p);
                    }
                }
                pointQueue.Remove(c);
            }
            logger.Info("Created distance map.");
            return distanceMap;
        }
        private static Point[] getDirections()
        {
            Point[] directions = new Point[4];
            directions[0] = new Point( 1, 0 );
            directions[1] = new Point( 0, -1 );
            directions[2] = new Point( -1, 0 );
            directions[3] = new Point( 0, 1 );
            return directions;
        }
        private static Point[] calculateShortestRouteThroughDistanceMap(int[][] distanceMap, Point start, Point end)
        {
            logger.Info("Calculating route through distance map...");
            List<Point> route = new List<Point>();
            Point currentPoint = end;
            while (!ArrayTools.Equals(currentPoint, start))
            {
                Point bestPoint = calculateBestPoint(currentPoint, distanceMap);
                route.Add(bestPoint);
                currentPoint = bestPoint;
            }
            logger.Info("Calculated route.");
            return route.ToArray();
        }
        private static Point calculateBestPoint(Point currentPoint, int[][] distanceMap)
        {
            int[] distanceOptions = ArrayTools.Create(4, Int32.MaxValue);
            Point[] pointOptions = new Point[4];
            Point[] directions = getDirections();

            logger.Debug("Current Point: {0}", currentPoint.ToString());

            for (int i = 0; i < directions.Length; i++)
            {
                Point p = currentPoint + directions[i];

                // check point is inside map
                bool isInside = Map.IsPointInsideMap(distanceMap, p);
                if (!isInside) continue;

                distanceOptions[i] = distanceMap[p.Y][p.X];
                pointOptions[i] = p;

                logger.Debug("- {0} : {1}", p.ToString(), distanceMap[p.Y][p.X]);
            }

            int minIndex = ArrayTools.MinIndex(distanceOptions);
            Point bestPoint = pointOptions[minIndex];
            return bestPoint;
        }
    }
}
