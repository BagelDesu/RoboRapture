//-----------------------------------------------------------------------
// <copyright file="CrossPattern.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Patterns
{ 
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;

    public class CrossPattern : IPattern
    {
        private int range;

        private List<CardinalDirections> cardinalDirections;

        public CrossPattern(int range, List<CardinalDirections> cardinalDirections)
        {
            this.range = range;
            this.cardinalDirections = cardinalDirections;
        }

        public List<Point> GetPattern()
        {
            List<Point> points = new List<Point>();

            foreach (CardinalDirections cardinalDirection in this.cardinalDirections)
            {
                Point direction = Direction.GetDirection(cardinalDirection);
                points.AddRange(this.ProcessDirection(direction));
            }

            return points;
        }

        private List<Point> ProcessDirection(Point direction)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < this.range; i++)
            {
                Point point = new Point(direction.x + (direction.x * i), direction.y + (direction.y * i), direction.z + (direction.z * i));
                points.Add(point);
            }

            return points;
        }
    }
}