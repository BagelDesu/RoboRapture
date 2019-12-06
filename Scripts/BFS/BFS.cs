//-----------------------------------------------------------------------
// <copyright file="BFS.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.BFS
{
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.DataTypes;

    public class BFS
    {
        public static Dictionary<Point, List<Point>> GetPaths(List<Point> validPositions, Point origin, int maxPathLengthEdges)
        {
            HashSet<Point> visitedPoints = new HashSet<Point>();
            Dictionary<Point, Point> backtrackMap = new Dictionary<Point, Point>();
            List<Point> pendingPoints = new List<Point>();
            pendingPoints.Add(origin);
            while (!(pendingPoints.Count == 0))
            {
                Point currentPoint = pendingPoints[0];
                pendingPoints.RemoveAt(0);
                visitedPoints.Add(currentPoint);
                List<Point> unvisitedNeighbors = GetUnvisitedNeighbors(currentPoint, validPositions, pendingPoints, visitedPoints);
                AddToBacktrackMap(currentPoint, unvisitedNeighbors, backtrackMap);
                pendingPoints.AddRange(unvisitedNeighbors);
            }

            Dictionary<Point, List<Point>> paths = new Dictionary<Point, List<Point>>();
            foreach (Point p in backtrackMap.Keys)
            {
                List<Point> path = GetPathTo(p, backtrackMap, maxPathLengthEdges + 1);
                if (path != null)
                {
                    paths.Add(p, path);
                }
            }

            return paths;
        }

        private static List<Point> GetPathTo(Point p, Dictionary<Point, Point> backtrackMap, int maxPathLengthPoints)
        {
            List<Point> path = new List<Point>();
            Point current = p;
            path.Add(p);
            while (backtrackMap.ContainsKey(current) && path.Count <= maxPathLengthPoints)
            {
                path.Add(backtrackMap[current]);
                current = backtrackMap[current];
            }

            return path.Count <= maxPathLengthPoints ? path : null;
        }

        private static void AddToBacktrackMap(Point ancestor, List<Point> points, Dictionary<Point, Point> backtrackMap)
        {
            foreach (Point point in points)
            {
                backtrackMap.Add(point, ancestor);
            }
        }

        private static List<Point> GetUnvisitedNeighbors(Point currentPoint, List<Point> validPositions, List<Point> pendingPoints, HashSet<Point> visitedPoints)
        {
            List<Point> possibleNeighbors = new List<Point>();
            possibleNeighbors.Add(new Point(currentPoint.x, 0, currentPoint.z + 1));
            possibleNeighbors.Add(new Point(currentPoint.x, 0, currentPoint.z - 1));
            possibleNeighbors.Add(new Point(currentPoint.x - 1, 0, currentPoint.z));
            possibleNeighbors.Add(new Point(currentPoint.x + 1, 0, currentPoint.z));

            return possibleNeighbors.Where(point => validPositions.Contains(point))
                                    .Where(point => !pendingPoints.Contains(point))
                                    .Where(point => !visitedPoints.Contains(point))
                                    .ToList();
        }
    }
}