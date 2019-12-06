//-----------------------------------------------------------------------
// <copyright file="PointUtils.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using System.Collections.Generic;
    using UnityEngine;

    public class PointUtils
    {
        public static int GetDistance(Point a, Point b)
        {
            return Mathf.Abs(b.x - a.x) + Mathf.Abs(b.z - a.z);
        }

        public static Point ScaleVector(Point vector, int scale)
        {
            return new Point(vector.x * scale, vector.y * scale, vector.z * scale);
        }

        public static List<Point> GetVerticalMiddlePoints(Point initialPoint, Point finalPoint)
        {
            List<Point> middlePoints = new List<Point>();
            if (initialPoint == default || finalPoint == default)
            {
                return middlePoints;
            }

            int distance = GetDistance(initialPoint, finalPoint);
            for (int i = 1; i < distance; i++)
            {
                Point middlePoint = new Point(initialPoint.x, initialPoint.y, initialPoint.z < finalPoint.z ? initialPoint.z + i : finalPoint.z + i);
                middlePoints.Add(middlePoint);
            }

            return middlePoints;
        }
    }
}