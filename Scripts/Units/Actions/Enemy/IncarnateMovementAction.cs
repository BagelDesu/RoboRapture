//-----------------------------------------------------------------------
// <copyright file="IncarnateMovementAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;

    public class IncarnateMovementAction : NonFlyingMovementAction
    {
        private System.Random random = new System.Random();

        public override List<Point> GetValidTargets(List<Point> board, Point point)
        {
            List<Point> boardPoints = base.GetValidTargets(board, point);
            this.ValidPositions = boardPoints?.Where(currentPoint => !this.Unit.UnitsMap.Contains(currentPoint)).ToList();
            return this.ValidPositions;
        }

        public override void Execute(Point target)
        {
            if (this.ValidPositions == null || this.ValidPositions.Count == 0)
            {
                return;
            }

            int randomIndex = this.random.Next(this.ValidPositions.Count);
            Point position = this.ValidPositions[randomIndex];

            PlacementEffects placement = new PlacementEffects();
            this.StartCoroutine(placement.LerpMovementPath(this, this.Unit, new List<Point>(SetPathOrder(validPaths[position]))));
        }
    }
}