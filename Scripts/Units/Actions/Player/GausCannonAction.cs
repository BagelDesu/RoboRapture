//-----------------------------------------------------------------------
// <copyright file="GausCannonAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Player
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;
    using Edu.Vfs.RoboRapture.Patterns;
    using UnityEngine;

    public class GausCannonAction : SkillAction
    {
        [SerializeField]
        private GameObject lasserVFX;

        private float delay = 0.4f;

        public override bool ValidateAction(Point target)
        {
            return base.ValidateAction(target);
        }

        public override void Execute(Point target)
        {
            base.Execute(target);
            HighlightAttack(false, target);
        }

        public override void Execute()
        {
            CardinalDirections direction = Direction.GetCardinalDirection(this.Unit.GetPosition(), this.Target);
            Vector3 point = PointConverter.ToVector(Direction.GetDirection(direction));
            Destroy(Instantiate(lasserVFX, this.transform.position, RotationHelper.GetRotation(direction)), delay);
            StartCoroutine(Attack(direction, point));
        }

        private IEnumerator Attack(CardinalDirections direction, Vector3 point)
        {
            yield return new WaitForSeconds(delay);
            Logcat.I(this, $"Shooting with direction {direction} vector {point}");
            this.transform.localPosition += point;
            GetPointsInLine(direction)?.ForEach(p => UnitsMap.Get(p)?.Health?.ReduceHealth(this.DeltaHealth));
            this.transform.localPosition -= point;
            KnockbackHandler handler = new KnockbackHandler(this.UnitsMap);
            handler.Execute(this.BoardController, Target, this.Unit.GetPosition(), this.Knockback);
        }

        private List<Point> GetPointsInLine(CardinalDirections direction)
        {
            if (Board == null)
            {
                return new List<Point>();
            }

            Point unit = this.Unit.GetPosition();
            List<Point> points = new List<Point>();
            switch (direction)
            {
                case CardinalDirections.North:
                    points = Board.Where(point => point.x == unit.x && point.z > unit.z).ToList();
                    break;
                case CardinalDirections.South:
                    points = Board.Where(point => point.x == unit.x && point.z < unit.z).ToList();
                    break;
                case CardinalDirections.East:
                    points = Board.Where(point => point.z == unit.z && point.x > unit.x).ToList();
                    break;
                case CardinalDirections.West:
                    points = Board.Where(point => point.z == unit.z && point.x < unit.x).ToList();
                    break;
            }

            return points;
        }

        protected override void HighlightTileAttack(bool highlight, Point position)
        {
            base.HighlightTileAttack(highlight, position);
            HighlightAttack(highlight, position);
        }

        private void HighlightAttack(bool highlight, Point position)
        {
            CardinalDirections direction = Direction.GetCardinalDirection(this.Unit.GetPosition(), position);
            List<Point> pointsInLine = GetPointsInLine(direction);
            pointsInLine?.ForEach(p => SimulateAttack(highlight, p, this.DeltaHealth, this.Knockback, true));
        }
    }
}