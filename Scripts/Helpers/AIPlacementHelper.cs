//-----------------------------------------------------------------------
// <copyright file="AIPlacementHelper.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class AIPlacementHelper
    {
        public static void MoveToClosestTarget(MonoBehaviour caller, Dictionary<Point, List<Point>> allPaths, Unit unit, int range, Point targetUnit)
        {
            Dictionary<Point, int> adjacentTargetPoints = AIUtils.AdjacentPointsToTarget(allPaths, unit.GetPosition(), targetUnit);
            foreach (var item in adjacentTargetPoints)
            {
                //// Logcat.I($"Moving {this.Unit.GetPosition()} to closest target by adjacent tiles {item.Key} within distance {item.Value}");
                if (item.Value <= range)
                {
                    List<Point> path = allPaths[item.Key];
                    SetPathOrder(path, unit);

                    List<Point> pathToTarget = path.Count > range + 1 ? path.GetRange(0, range + 1) : path;
                    PlacementEffects placement = new PlacementEffects();
                    caller.StartCoroutine(placement.LerpMovementPath(caller, unit, pathToTarget));
                    return;
                }
            }
        }

        public static void MoveToTarget(MonoBehaviour caller, Dictionary<Point, List<Point>> allValidPaths, Unit unit, int range, Point targetUnit)
        {
            Dictionary<Point, int> adjacentTargetPoints = AIUtils.AdjacentPointsToTarget(allValidPaths, unit.GetPosition(), targetUnit);
            if (adjacentTargetPoints.Count == 0)
            {
                return;
            }

            Point pointTarget = adjacentTargetPoints.First().Key;
            List<Point> path = allValidPaths[pointTarget];
            SetPathOrder(path, unit);
            //// Logcat.I($"path.Count {path.Count()} range {this.Range} distance {adjacentTargetPoints.First().Value}");
            if (path.Count() > range)
            {
                PlacementEffects placement = new PlacementEffects();
                caller.StartCoroutine(placement.LerpMovementPath(caller, unit, path.GetRange(0, range+1)));
                //// PlacementHelper.Move(unit, path[range], new MovementActionValidator());
            }
        }

        public static Unit AddUnit(Transform parent, Point point, Unit prefab)
        {
            Unit instance = MonoBehaviour.Instantiate(prefab, new Vector3((float)point.x, point.y + prefab.Height, (float)point.z), prefab.transform.rotation);
            instance.SetPosition(point);
            instance.transform.parent = parent;
            instance.UnitsMap.Add(point, instance);
            return instance;
        }

        public static GameObject AddEffect(GameObject prefab, Transform parent, Point point, float height)
        {
            GameObject instance = MonoBehaviour.Instantiate(prefab, new Vector3((float)point.x, (float)(point.y + height), (float)point.z), prefab.transform.rotation);
            instance.transform.parent = parent;
            return instance;
        }

        public static void RemoveEffect(GameObject prefab)
        {
            MonoBehaviour.Destroy(prefab);
        }

        private static List<Point> SetPathOrder(List<Point> path, Unit unit)
        {
            if (path[0] != unit.GetPosition())
            {
                path.Reverse();
            }

            return path;
        }
    }
}