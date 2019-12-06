//-----------------------------------------------------------------------
// <copyright file="Direction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Patterns
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using UnityEngine;

    public class Direction
    {
        private static Dictionary<CardinalDirections, Point> directions;

        static Direction()
        {
            directions = new Dictionary<CardinalDirections, Point>()
            {
                { CardinalDirections.North, new Point(0, 0, 1) },
                { CardinalDirections.South, new Point(0, 0, -1) },
                { CardinalDirections.East, new Point(1, 0, 0) },
                { CardinalDirections.West, new Point(-1, 0, 0) },
                { CardinalDirections.Center, new Point(0, 0, 0) },
                { CardinalDirections.NorthEast, new Point(1, 0, 1) },
                { CardinalDirections.NorthWest, new Point(-1, 0, 1) },
                { CardinalDirections.SouthEast, new Point(1, 0, -1) },
                { CardinalDirections.SouthWest, new Point(-1, 0, -1) }
            };
        }

        public static Point GetDirection(CardinalDirections cardinalDirection)
        {
            return directions[cardinalDirection];
        }

        public static CardinalDirections GetCardinalDirection(Point initial, Point final)
        {
            Point point = GetDirection(initial, final);

            foreach (var item in directions)
            {
                if (item.Value == point)
                {
                    return item.Key;
                }
            }

            return CardinalDirections.Center;
        }

        private static Point GetDirection(Point initial, Point final)
        {
            Point result = final - initial;
            result.x = result.x == 0 ? 0 : result.x / Mathf.Abs(result.x);
            result.y = result.y == 0 ? 0 : result.y / Mathf.Abs(result.y);
            result.z = result.z == 0 ? 0 : result.z / Mathf.Abs(result.z);

            return result;
        }
    }
}