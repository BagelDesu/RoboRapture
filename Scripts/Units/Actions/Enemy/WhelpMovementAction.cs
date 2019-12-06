//-----------------------------------------------------------------------
// <copyright file="WhelpMovementAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;

    public class WhelpMovementAction : NonFlyingMovementAction
    {
        private Dictionary<Point, List<Point>> allPaths;

        public override void Execute(Point target)
        {
            this.allPaths = AIUtils.GetAllPaths(this.Board, this.Unit.UnitsMap, this.Unit.GetPosition(), true);
            KeyValuePair<Point, int> targetUnit = AIUtils.GetClosestTarget(this.Unit.UnitsMap, this.Unit.GetPosition(), this.Range);
            
            if (!targetUnit.Equals(default(KeyValuePair<Point, int>)))
            {
                AIPlacementHelper.MoveToClosestTarget(this, this.validPaths, this.Unit, this.Range, targetUnit.Key);
                ((EnemyUnit)this.Unit).Target = PointUtils.GetDistance(this.Unit.GetPosition(), targetUnit.Key) <= 1 ? this.Unit.UnitsMap.Get(targetUnit.Key) : null;
                return;
            }

            Point unitPosition = GetTarget();
            AIPlacementHelper.MoveToTarget(this, this.allPaths, this.Unit, this.Range, unitPosition);
            ((EnemyUnit)this.Unit).Target = PointUtils.GetDistance(this.Unit.GetPosition(), unitPosition) <= 1 ? this.Unit.UnitsMap.Get(unitPosition) : null;
        }

        protected virtual Point GetTarget()
        {
            return AIUtils.GetStrongestTarget(this.Unit.UnitsMap);
        }
    }
}