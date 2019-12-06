//-----------------------------------------------------------------------
// <copyright file="MopelessMovementAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using System.Collections.Generic;

    public class MopelessMovementAction : NonFlyingMovementAction
    {
        public override void Execute(Point enemyToAvoid)
        {
            if (enemyToAvoid == default)
            {
                MoveToAlignWithPlayer();
                return;
            }

            ScapeFromTarget(enemyToAvoid);
        }

        private void MoveToAlignWithPlayer()
        {
            Point possibleMovement = AIUtils.AlignToPlayerUnit(this.Unit.UnitsMap, this.Unit, this.validPaths);
            if (possibleMovement == default)
            {
                return;
            }

            List<Point> path = this.validPaths[possibleMovement];
            PlacementEffects placement = new PlacementEffects();
            this.StartCoroutine(placement.LerpMovementPath(this, this.Unit, SetPathOrder(path)));
            //// PlacementHelper.Move(this.Unit, possibleMovement, new MoveUnitValidator(this.Unit, possibleMovement));
        }

        private void MoveToWeakestTarget()
        {
            //// Logcat.I("There is not enemy to avoid, moving to weakest target");
            Point unit = AIUtils.GetWeakestTarget(this.Unit.UnitsMap);
            if (unit != default)
            {
                Dictionary<Point, List<Point>> allPaths = AIUtils.GetAllPaths(this.Board, this.Unit.UnitsMap, this.Unit.GetPosition(), true);
                AIPlacementHelper.MoveToTarget(this, allPaths, this.Unit, this.Range, unit);
            }
        }

        private void ScapeFromTarget(Point enemyToAvoid)
        {
            Point scapePosition = AIUtils.GetPositionToScape(enemyToAvoid, this.Unit.GetPosition(), this.Range, this.validPaths);
            if (scapePosition == default)
            {
                //// Logcat.I("Invalid position to scape");
                return;
            }

            //// Logcat.I(this, $"Scaping to {scapePosition}");
            List<Point> path = this.validPaths[scapePosition];
            PlacementEffects placement = new PlacementEffects();
            this.StartCoroutine(placement.LerpMovementPath(this, this.Unit, SetPathOrder(path)));
            //// PlacementHelper.Move(this.Unit, scapePosition, new MoveUnitValidator(this.Unit, scapePosition));
        }
    }
}
