//-----------------------------------------------------------------------
// <copyright file="PatternProcessor.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Patterns
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using UnityEngine;

    public class PatternProcessor
    {
        public static List<Point> Process(List<Point> board, IPattern pattern, Point initialPoint, int distance)
        {
            if (pattern == null || board == null || initialPoint == null)
            {
                return null;
            }

            List<Point> tempPoints = new List<Point>();
            foreach (Point point in pattern.GetPattern())
            {
                int x = ProcessPoint(point.x, distance);
                int y = ProcessPoint(point.y, distance);
                int z = ProcessPoint(point.z, distance);
                Point pointWithDistance = new Point(x, y, z);
                tempPoints.Add(initialPoint + pointWithDistance);
            }

            return Filter(board, tempPoints);
        }

        /// <summary>
        /// Depending on point value, we ignore, add or remove the distance.
        /// </summary>
        /// <param name="point">x, y, z values.</param>
        /// <param name="distance">distance to add.</param>
        /// <returns>If point is zero, not value added, if point is bigger than zero, we add the distance, otherwise, we reduce the distance.</returns>
        private static int ProcessPoint(int point, int distance)
        {
            return point != 0 ? (point > 0 ? point + distance : point - distance) : 0;
        }

        private static List<Point> Filter(List<Point> board, List<Point> points)
        {
            List<Point> temp = new List<Point>(points);
            foreach (Point point in points)
            {
                if (!board.Contains(point))
                {
                    temp.Remove(point);
                }
            }

            return temp;
        }
    }
}