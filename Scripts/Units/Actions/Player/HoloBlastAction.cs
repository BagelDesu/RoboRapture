//-----------------------------------------------------------------------
// <copyright file="HoloBlastAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Player
{
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.Units.Actions.Attacks;
    using UnityEngine;

    public class HoloBlastAction : SkillAction
    {
        [SerializeField]
        private int firstAttackDamage = 2;

        [SerializeField]
        private int secondAttackDamage = 1;

        [SerializeField]
        private HoloBlast bullet;

        public override bool ValidateAction(Point target)
        {
            return base.ValidateAction(target);
        }

        public override void Execute(Point target)
        {
            base.Execute(target);
            Unhighlight();
        }

        public override void Execute()
        {
            CardinalDirections direction = Direction.GetCardinalDirection(this.Unit.GetPosition(), this.Target);
            Vector3 point = PointConverter.ToVector(Direction.GetDirection(direction));

            this.transform.localPosition += point;
            HoloBlast blast = MonoBehaviour.Instantiate(this.bullet, this.gameObject.transform.position, RotationHelper.GetRotation(direction));
            blast.SetUp(this.UnitsMap, this.BoardController, this.Unit.GetPosition(), PlayerUtils.HoloBlastSplitDirections(direction), this.firstAttackDamage, this.secondAttackDamage, this.Knockback);
            this.transform.localPosition -= point;
        }

        protected override void HighlightTileAttack(bool highlight, Point position)
        {
            base.HighlightTileAttack(highlight, position);
            if (highlight)
            {
                Highlight(position);
            }
            else
            {
                Unhighlight();
            }
        }

        private void Highlight(Point position)
        {
            Point closestTarget = AIUtils.GetClosestUnit(this.UnitsMap, this.Unit.GetPosition(), position);
            if (closestTarget != default)
            {
                SimulateAttack(true, closestTarget, this.DeltaHealth, this.Knockback, true);
            }
        }

        private void Unhighlight()
        {
            Point closestTarget = AIUtils.GetClosestUnit(this.UnitsMap, this.Unit.GetPosition(), new Point(this.Unit.GetPosition().x - 1, 0, this.Unit.GetPosition().z));
            if (closestTarget != default)
            {
                SimulateAttack(false, closestTarget, this.DeltaHealth, this.Knockback, true);
            }

            closestTarget = AIUtils.GetClosestUnit(this.UnitsMap, this.Unit.GetPosition(), new Point(this.Unit.GetPosition().x + 1, 0, this.Unit.GetPosition().z));
            if (closestTarget != default)
            {
                SimulateAttack(false, closestTarget, this.DeltaHealth, this.Knockback, true);
            }

            closestTarget = AIUtils.GetClosestUnit(this.UnitsMap, this.Unit.GetPosition(), new Point(this.Unit.GetPosition().x, 0, this.Unit.GetPosition().z - 1));
            if (closestTarget != default)
            {
                SimulateAttack(false, closestTarget, this.DeltaHealth, this.Knockback, true);
            }

            closestTarget = AIUtils.GetClosestUnit(this.UnitsMap, this.Unit.GetPosition(), new Point(this.Unit.GetPosition().x, 0, this.Unit.GetPosition().z + 1));
            if (closestTarget != default)
            {
                SimulateAttack(false, closestTarget, this.DeltaHealth, this.Knockback, true);
            }
        }
    }
}