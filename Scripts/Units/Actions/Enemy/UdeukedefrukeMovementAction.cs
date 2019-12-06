//-----------------------------------------------------------------------
// <copyright file="UdeukedefrukeMovementAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Units.Enemies;
    using System.Collections.Generic;
    using System.Linq;

    public class UdeukedefrukeMovementAction : NonFlyingMovementAction
    {
        private Point stretchTo;

        private Point moveTo;

        public override void Execute(Point target)
        {
            GetBestStretching();
            if (moveTo == default)
            {
                return;
            }

            Logcat.I(this, $"UdeukedefrukeMovementAction moveTo {moveTo} stretchTo {stretchTo}");
            PlacementEffects placement = new PlacementEffects();
            this.StartCoroutine(placement.LerpMovementPath(this, this.Unit, SetPathOrder(validPaths[stretchTo])));
            //// PlacementHelper.Move(this.Unit, moveTo, new MovementActionValidator());
            this.GetComponentInParent<UdeukedefrukeBehaviour>().HeadPoint = moveTo;
        }

        private void GetBestStretching()
        {
            SortedDictionary<Point, Point> landingPositions = new SortedDictionary<Point, Point>(new PointComparerByRow());
            validPaths.Keys.ToList().ForEach(p => landingPositions.Add(p, GetMaxStretchingPointToLand(p)));

            int distance = 0;
            foreach (var item in landingPositions)
            {
                int tempDistance = PointUtils.GetDistance(item.Key, item.Value);
                if (tempDistance > distance)
                {
                    distance = tempDistance;
                    stretchTo = item.Key;
                }
            }
            if (distance < 3)
            {
                moveTo = default;
                stretchTo = default;
                return;
            }

            this.moveTo = landingPositions[stretchTo];
        }

        private Point GetMaxStretchingPointToLand(Point point)
        {
            Point remotestPoint = default;
            List<Point> unitsInRow = this.Unit.UnitsMap.GetUnits().Where(unit => (unit.x == point.x)).ToList();
            unitsInRow.Remove(this.Unit.GetPosition());
            unitsInRow.Add(new Point(point.x, point.y, -1));
            unitsInRow.Add(new Point(point.x, point.y, 8));

            List<Point> unitsAbove = unitsInRow.Where(p => p.z > point.z).OrderByDescending(p => p.z).ToList();
            List<Point> unitsBelow = unitsInRow.Where(p => p.z < point.z).OrderByDescending(p => p.z).ToList();

            Point superiorLimit = unitsAbove.Last();
            Point inferiorLimit = unitsBelow.First();

            if (PointUtils.GetDistance(point, superiorLimit) > PointUtils.GetDistance(point, inferiorLimit))
            {
                remotestPoint = new Point(superiorLimit.x, superiorLimit.y, superiorLimit.z -1);
            }
            else {
                remotestPoint = new Point(inferiorLimit.x, inferiorLimit.y, inferiorLimit.z + 1);
            }

            return remotestPoint;
        }
    }
}