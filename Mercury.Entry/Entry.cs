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

            int width = 5000;
            int height = 5000;
            //int numPoints = 20;

            Map.Save("map.txt", Map.Create(width, height));

            //Point[] points = Route.Create(numPoints, width, height);
            //Route.Save("route.txt", points, true);

            logger.Info("Mercury.Entry");
            int[][] map = Map.Load(args[0]);
            Point[] requiredRoute = Route.Load(args[1], true);

            // Task 1
            Point[] route1 = calculateShortestRouteBetweenTwoPoints(map, requiredRoute[0], requiredRoute[1]);
            Route.Save("task1.txt", route1);
            Map.Draw("task1.bmp", map, requiredRoute ,route1);
            //// Task 2
            //Point[] route2 = calcaulteShortedRouteFollowingPoints(map, requiredRoute);
            //Route.Save("task2.txt", route2);
            //Map.Draw("task3.bmp", map, bestRoute, null);
            //// Task 3
            //Point[] bestRoute = calculateShortestRouteToVisitAllPoints(map, points);
            //Route.Save("task3.txt", bestRoute);
        }
        
        private static Point[] calculateShortestRouteToVisitAllPoints(int[][] map, Point[] requiredPoints)
        {
            logger.Info("Calculating shortest route to visit all {0} points", requiredPoints.Length);

            int[][] routeMap = calculateRouteMap(map, requiredPoints);
            Point[] route = calculateOrderToVisitPoints(routeMap, requiredPoints);
            Point[] fullRoute = loadPoints(route);

            return fullRoute;
        }

        private static Point[] loadPoints(Point[] route)
        {
            Point[] fullRoute = new Point[0];
            for(int i=1;i<route.Length;i++)
            {
                Point from = route[i-1];
                Point to = route[i];

                string filenamea = String.Format("[{0}.{1}][{2}.{3}].txt", from.X, from.Y, to.X, to.Y);
                string filenameb = String.Format("[{0}.{1}][{2}.{3}].txt", to.X, to.Y, from.X, from.Y);
                Point[] subroute;
                if (File.Exists(filenamea)) subroute = Route.Load(filenamea, false);
                else if (File.Exists(filenameb)) subroute = Route.Load(filenameb, false);
                else throw new Exception("Route not find");

                fullRoute = ArrayTools.Append(fullRoute, subroute);
            }
            return fullRoute;
        }

        private static Point[] calculateOrderToVisitPoints(int[][] routeMap, Point[] requiredPoints)
        {
            logger.Info("Calculating order to visit points", requiredPoints.Length);
            return bruteForce(routeMap, requiredPoints);
        }
        private static Point[] bruteForce(int[][] routeMap, Point[] required)
        {
            int bestDistance = Int32.MaxValue;
            Point[] bestRoute = null;

            for (int i = 0; i < required.Length;i++)
            {
                Point start = required[i];
                logger.Info("Starting from {0}", start);

                // Setup points left list
                Point[] left = new Point[required.Length - 1];
                for (int j = 0; j < i; j++) left[j] = required[j];
                for (int j = i + 1; j < required.Length; j++) left[j-1] = required[j];

                Point[] route = bruteForce(routeMap, required, left, new Point[] { start }, 0);
                int routeDistance = calculateRouteDistance(required, route, routeMap);

                if (routeDistance < bestDistance)
                {
                    bestRoute = route;
                    bestDistance = routeDistance;
                }
            }
            return bestRoute;
        }
        private static Point[] bruteForce(int[][] routeMap, Point[] required, Point[] pointsLeft, Point[] pointsVisited, int distance)
        {
            Point start = pointsVisited[pointsVisited.Length-1];
            int startIndex = ArrayTools.IndexOf(required, start);

            int bestDistance = (pointsLeft.Length == 0) ? distance : Int32.MaxValue;
            Point[] bestRoute = (pointsLeft.Length == 0) ? pointsVisited : null;
            for (int i = 0; i < pointsLeft.Length; i++)
            {
                Point end = pointsLeft[i];

                // Setup points left list
                Point[] left = new Point[pointsLeft.Length - 1];
                for (int j = 0; j < i; j++) left[j] = pointsLeft[j];
                for (int j = i+1; j < pointsLeft.Length; j++) left[j-1] = pointsLeft[j];
                
                // Setup points visited list
                Point[] visited = new Point[pointsVisited.Length + 1];
                for (int j = 0; j < pointsVisited.Length; j++) visited[j] = pointsVisited[j];
                visited[visited.Length-1] = pointsLeft[i];

                int endIndex = ArrayTools.IndexOf(required, end);
                int additionalDist = routeMap[startIndex][endIndex];

                Point[] route = bruteForce(routeMap, required, left, visited, additionalDist + distance);
                int routeDistance = calculateRouteDistance(required, route, routeMap);

                if (routeDistance < bestDistance)
                {
                    bestRoute = route;
                    bestDistance = routeDistance;
                }
            }
            return bestRoute;
        }

        private static int calculateRouteDistance(Point[] required, Point[] route, int[][] routeMap)
        {
            int distance = 0;
            for (int j = 1; j < route.Length; j++)
            {
                int a = ArrayTools.IndexOf(required, route[j - 1]);
                int b = ArrayTools.IndexOf(required, route[j]);
                distance += routeMap[a][b];
            }
            return distance;
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

                    string filenamea = String.Format("[{0}.{1}][{2}.{3}].txt", start.X, start.Y, end.X, end.Y);
                    string filenameb = String.Format("[{0}.{1}][{2}.{3}].txt", end.X, end.Y, start.X, start.Y);
                    Route.Save(filenamea, route);
                    Route.Save(filenameb, route);

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

            List<Point> pointQueue = new List<Point>(2*map.Length);
            pointQueue.Add(start);
            distanceMap[start.Y][start.X] = 0;

            List<Point> left = new List<Point>(ends);

            logger.Info("Processing points...");
            int iteration = 0;
            DateTime startTime = DateTime.Now;
            while (pointQueue.Count > 0)
            {
                if (iteration % 10000 == 0)
                {
                    TimeSpan span = DateTime.Now - startTime;
                    string spanS = String.Format("{0}", span);
                    logger.Info("{0,-10} at {1,-10} Q {2,-10}", iteration, spanS, pointQueue.Count);
                }
                iteration++;

                //int[] distances = (from p in pointQueue select distanceMap[p.Y][p.X]).ToArray();
                //int minIndex = ArrayTools.MinIndex(distances);
                Point c = ArrayTools.MinFor<Point>(pointQueue, p => distanceMap[p.Y][p.X]);

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
