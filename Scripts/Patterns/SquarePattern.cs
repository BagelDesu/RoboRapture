//-----------------------------------------------------------------------
// <copyright file="SquarePattern.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Patterns
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;

    public class SquarePattern : IPattern
    {
        private int range;

        public SquarePattern(int range)
        {
            this.range = range;
        }

        public List<Point> GetPattern()
        {
            List<Point> points = new List<Point>();
            if (this.range <= 0)
            {
                return points;
            }

            for (int x = -this.range; x <= this.range; x++)
            {
                for (int y = -this.range; y <= this.range; y++)
                {
                    points.Add(new Point(x, 0, y));
                }
            }

            return points;
        }
    }
}
