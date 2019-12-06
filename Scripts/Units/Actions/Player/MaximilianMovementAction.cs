//-----------------------------------------------------------------------
// <copyright file="MaximilianMovementAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Environment;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Scriptables;
    using UnityEngine;

    public class MaximilianMovementAction : Action
    {
        public static System.Action<List<Point>, Point> MovementPath;

        [SerializeField]
        private UnitsMap map;

        public void OnEnable()
        {
            TileHovered.TileHoveredOn += OnTileHovered;
        }

        public void OnDisable()
        {
            TileHovered.TileHoveredOn -= OnTileHovered;
        }

        public void OnTileHovered(Tile tile)
        {
            if (this.IsActive() && this.ValidPositions != null && this.ValidPositions.Contains(tile.GetPosition()))
            {
                //// Logcat.I(this, $"Maximilian movement {this.ValidPositions.Count}, target position {tile.GetPosition()}");
                MovementPath?.Invoke(this.ValidPositions, tile.GetPosition());
            }
        }

        public override List<Point> GetValidTargets(List<Point> board, Point point)
        {
            List<Point> boardPoints = base.GetValidTargets(board, point);
            this.ValidPositions = boardPoints.Where(currentPoint => !map.Contains(currentPoint)).ToList();
            return this.ValidPositions;
        }

        public override void Execute(Point target)
        {
            this.IsActive(false);
            FaceTargetDirection(target);

            PlacementEffects placement = new PlacementEffects();
            StartCoroutine(placement.JumpCoroutine(this.Unit, target));
        }
    }
}
