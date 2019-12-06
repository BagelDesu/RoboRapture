//-----------------------------------------------------------------------
// <copyright file="NonFlyingMovementAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.BFS;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Environment;
    using Edu.Vfs.RoboRapture.Helpers;
    using UnityEngine;

    public class NonFlyingMovementAction : Action
    {
        public static System.Action<List<Point>> MovementPath;

        protected Dictionary<Point, List<Point>> validPaths;

        [SerializeField]
        protected bool showPath = false;

        public void OnEnable()
        {
            TileHovered.TileHoveredOn += GeneratePath;
        }

        public void OnDisable()
        {
            TileHovered.TileHoveredOn -= GeneratePath;
        }

        public override List<Point> GetValidTargets(List<Point> board, Point point)
        {
            List<Point> boardPoints = base.GetValidTargets(board, point);
            List<Point> excludingEnemies = boardPoints.Where(currentPoint => !this.Unit.UnitsMap.Contains(currentPoint)).ToList();
            validPaths = BFS.GetPaths(excludingEnemies, point, this.Range);
            this.ValidPositions = validPaths.Keys.ToList();
            return this.ValidPositions;
        }

        public override void Execute(Point target)
        {
            this.IsActive(false);
            this.FaceTargetDirection(target);

            PlacementEffects placement = new PlacementEffects();
            this.StartCoroutine(placement.LerpMovementPath(this, this.Unit, new List<Point>(SetPathOrder(validPaths[target]))));
            this.validPaths = null;
        }

        private void GeneratePath(Tile tile)
        {
            if (validPaths == null || tile == null || !validPaths.ContainsKey(tile.GetPosition()) || !this.IsActive() || !showPath)
            {
                return;
            }

            List<Point> path = SetPathOrder(validPaths[tile.GetPosition()]);
            MovementPath?.Invoke(path);
        }

        protected List<Point> SetPathOrder(List<Point> path)
        {
            if (path[0] != this.Unit.GetPosition())
            {
                path.Reverse();
            }

            return path;
        }
    }
}
