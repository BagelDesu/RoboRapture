//-----------------------------------------------------------------------
// <copyright file="PointComparerByRow.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.DataTypes
{
    using System.Collections.Generic;

    public class PointComparerByRow : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            if (point1 == default || point2 == default)
            {
                return 0;
            }

            if (point1.x < point2.x)
            {
                return 1;
            }
            else if (point1.x > point2.x)
            {
                return -1;
            }
            else
            {
                if (point1.z < point2.z)
                {
                    return 1;
                }
                else if (point1.z > point2.z)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}