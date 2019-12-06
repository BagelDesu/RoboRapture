//-----------------------------------------------------------------------
// <copyright file="PointConverter.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Converters
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using UnityEngine;

    public class PointConverter
    {
        public static Point ToPoint(Vector3 vector)
        {
            Point result = new Point(0, 0, 0);

            if (vector == null)
            {
                return result;
            }

            result.x = (int) vector.x;
            result.y = (int) vector.y;
            result.z = (int) vector.z;
            return result;
        }

        public static Vector3 ToVector(Point point)
        {
            return new Vector3(point.x, point.y, point.z);
        }
    }
}
