//-----------------------------------------------------------------------
// <copyright file="AIUtils.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.BFS;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;
    using static Patterns.CardinalDirections;

    public class AIUtils : MonoBehaviour
    {
        public static Dictionary<Point, List<Point>> GetAllPaths(List<Point> board, UnitsMap unitsMap, Point unitPosition, bool excludePlayerUnits)
        {
            if (board == null || unitsMap == null)
            {
                return new Dictionary<Point, List<Point>>();
            }

            int distance = 0;
            int patternSize = 7;
            int stepsLimit = 17;
            List<Point> boardPoints = PatternProcessor.Process(board, new SquarePattern(patternSize), unitPosition, distance);
            List<Point> processedList = boardPoints.Where(currentPoint => !unitsMap.Contains(currentPoint)).ToList();

            if (!excludePlayerUnits)
            {
                unitsMap.GetUnits(Type.Player).ForEach(unit => processedList.Add(unit.GetPosition()));
            }

            return BFS.GetPaths(processedList, unitPosition, stepsLimit);
        }

        public static Dictionary<Point, List<Point>> GetAllPaths(List<Point> board, UnitsMap unitsMap, Point unitPosition)
        {
            if (board == null || unitsMap == null)
            {
                return new Dictionary<Point, List<Point>>();
            }

            int distance = 0;
            int patternSize = 7;
            int stepsLimit = 17;
            List<Point> boardPoints = PatternProcessor.Process(board, new SquarePattern(patternSize), unitPosition, distance);
            return BFS.GetPaths(boardPoints, unitPosition, stepsLimit);
        }

        /// <summary>
        /// Returns the adjacent points to the target point passed as argument.
        /// </summary>
        /// <param name="allPaths">Enemy's all path to move over the board</param>
        /// <param name="unitPosition">Enemy's position</param>
        /// <param name="targetPosition">Player unit position</param>
        /// <returns>Adjacent points to target in order to place on that position</returns>
        public static Dictionary<Point, int> AdjacentPointsToTarget(Dictionary<Point, List<Point>> allPaths, Point unitPosition, Point targetPosition)
        {
            Dictionary<Point, int> points = new Dictionary<Point, int>();
            Point targetNorth = targetPosition + Direction.GetDirection(North);
            Point targetSouth = targetPosition + Direction.GetDirection(South);
            Point targetWest = targetPosition + Direction.GetDirection(West);
            Point targetEast = targetPosition + Direction.GetDirection(East);

            if (allPaths.ContainsKey(targetNorth))
            {
                points.Add(targetNorth, PointUtils.GetDistance(unitPosition, targetNorth));
            }
            else if (allPaths.ContainsKey(targetEast))
            {
                points.Add(targetEast, PointUtils.GetDistance(unitPosition, targetEast));
            }
            else if (allPaths.ContainsKey(targetSouth))
            {
                points.Add(targetSouth, PointUtils.GetDistance(unitPosition, targetSouth));
            }
            else if (allPaths.ContainsKey(targetWest))
            {
                points.Add(targetWest, PointUtils.GetDistance(unitPosition, targetWest));
            }

            points.OrderBy(value => value.Value);
            return points;
        }

        public static KeyValuePair<Point, int> GetClosestTarget(UnitsMap unitsMap, Point unitPosition, int range)
        {
            Dictionary<Point, int> playerUnits = new Dictionary<Point, int>();
            foreach (var playerUnit in unitsMap.GetUnits(Type.Player))
            {
                int distance = PointUtils.GetDistance(unitPosition, playerUnit.GetPosition());
                playerUnits.Add(playerUnit.GetPosition(), distance);
            }

            var orderer = playerUnits.OrderByDescending(unit => unit.Value);
            if (orderer == null || orderer.Count() == 0)
            {
                return default;
            }

            if (orderer.Last().Value >= range)
            {
                return default;
            }

            //// Logcat.I($"First {orderer.First().Key} distance {orderer.First().Value}. Returned Last {orderer.Last().Key} distance {orderer.Last().Value}");
            return orderer.Last();
        }

        public static Point GetStrongestTarget(UnitsMap unitsMap)
        {
            List<Unit> units = unitsMap.GetUnits(Type.Player);
            if (units == null || units.Count == 0)
            {
                return default;
            }

            var ordered = units.OrderByDescending(unit => unit.Health.GetCurrentHealth());
            return ordered.ToList()[0].GetPosition();
        }

        public static Point GetWeakestTarget(UnitsMap unitsMap)
        {
            List<Unit> units = unitsMap.GetUnits(Type.Player);
            if (units == null || units.Count == 0)
            {
                return default;
            }

            units.RemoveAll(unit => unit.Health.IsDead());
            var ordered = units.OrderByDescending(unit => unit.Health.GetCurrentHealth());
            ordered.ToList().ForEach(unit => Logcat.I($"Units {unit.Health.GetCurrentHealth()}"));
            Point weakestUnit = ordered.ToList()[ordered.ToList().Count - 1].GetPosition();
            //// Logcat.I($"Weakest unit {weakestUnit} with {unitsMap.Get(weakestUnit).Health.GetCurrentHealth()} health");
            return weakestUnit;
        }

        public static Unit GetUnitInLine(UnitsMap unitsMap, Point unitPosition, Dictionary<Point, List<Point>> allPaths)
        {
            if (unitsMap == null || allPaths == null)
            {
                return null;
            }

            List<Unit> units = unitsMap.GetUnits(Type.Player);
            foreach (var item in units)
            {
                if (!allPaths.ContainsKey(item.GetPosition()))
                {
                    continue;
                }

                Point targetPosition = item.GetPosition();
                int distance = PointUtils.GetDistance(unitPosition, targetPosition);
                int steps = allPaths[targetPosition].Count - 1;
                //// Logcat.I($"Positions unit position {unitPosition}, target position {targetPosition} distance {distance} steps {steps}");
                if ((targetPosition.x == unitPosition.x || targetPosition.z == unitPosition.z) & distance == steps)
                {
                    //// Logcat.I($"Unit in range {targetPosition}");
                    return item;
                }
            }

            //// Logcat.I($"There is not a unit in line");
            return null;
        }

        public static Point GetPositionToScape(Point pointToAvoid, Point unitPosition, int range, Dictionary<Point, List<Point>> validPaths)
        {
            if (validPaths == null)
            {
                return default;
            }

            CardinalDirections direction = Direction.GetCardinalDirection(pointToAvoid, unitPosition);
            //// Logcat.I($"Direction to scape {direction}");
            Point vector = Direction.GetDirection(direction);

            for (int i = range; i >= 0; i--)
            {
                Point targetPoint = unitPosition + PointUtils.ScaleVector(vector, i);
                //// Logcat.I($"Posible point to scape {targetPoint}");
                if (validPaths.ContainsKey(targetPoint))
                {
                    //// Logcat.I($"Target position where to move {targetPoint}");
                    return targetPoint;
                }
            }

            return default;
        }

        public static Point AlignToPlayerUnit(UnitsMap unitsMap, Unit unit, Dictionary<Point, List<Point>> validPaths)
        {
            if (unitsMap == null || validPaths == null)
            {
                return default;
            }

            List<Unit> units = unitsMap.GetUnits(Type.Player);
            units.RemoveAll(pUnit => pUnit.Health.IsDead());
            if (units == null || units.Count == 0)
            {
                return default;
            }

            foreach (var path in validPaths)
            {
                foreach (var playerUnit in units)
                {
                    //// Logcat.I($"Trying to align to player unit {playerUnit.GetPosition()}, considering the valid position to move {path.Key}");
                    if (playerUnit.GetPosition().x == path.Key.x || playerUnit.GetPosition().z == path.Key.z)
                    {
                        //// Logcat.I($"Match found! {path.Key}");
                        return path.Key;
                    }
                }
            }

            return default;
        }

        public static Point GetClosestUnit(UnitsMap map, Point attacker, Point direction)
        {
            int limit = 8;
            CardinalDirections cardinalDirection = Direction.GetCardinalDirection(attacker, direction);
            switch (cardinalDirection)  
            {
                case CardinalDirections.North:
                    for (int i = attacker.z + 1; i < attacker.z + limit; i++)
                    {
                        Point position = new Point(attacker.x, 0, i);
                        if (map.Contains(position))
                        {
                            return position;
                        }
                    }
                    break;
                case CardinalDirections.South:
                    for (int i = attacker.z - 1; i > attacker.z - limit; i--)
                    {
                        Point position = new Point(attacker.x, 0, i);
                        if (map.Contains(position))
                        {
                            return position;
                        }
                    }
                    break;
                case CardinalDirections.East:
                    for (int i = attacker.x + 1; i < attacker.x + limit; i++)
                    {
                        Point position = new Point(i, 0, attacker.z);
                        if (map.Contains(position))
                        {
                            return position;
                        }
                    }
                    break;
                case CardinalDirections.West:
                    for (int i = attacker.x - 1; i > attacker.x - limit; i--)
                    {
                        Point position = new Point(i, 0, attacker.z);
                        if (map.Contains(position))
                        {
                            return position;
                        }
                    }
                    break;
                default:
                    break;
            }
            return default;
        }
    }
}