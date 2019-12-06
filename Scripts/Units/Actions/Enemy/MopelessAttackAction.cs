//-----------------------------------------------------------------------
// <copyright file="MopelessAttackAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.Units.Actions.Attacks;
    using UnityEngine;

    public class MopelessAttackAction : SkillAction
    {
        [SerializeField]
        private int damage = 3;

        [SerializeField]
        private GausCannon bullet;

        public override bool ValidateAction(Point target)
        {
            return base.ValidateAction(target);
        }

        public override void Execute()
        {
            CardinalDirections direction = Direction.GetCardinalDirection(this.Unit.GetPosition(), this.Target);
            Vector3 point = PointConverter.ToVector(Direction.GetDirection(direction));
            //// Logcat.I(this, $"Shooting with direction {direction} vector {point}");
            this.transform.localPosition += point;
            GausCannon gausCannon = MonoBehaviour.Instantiate(this.bullet, this.transform.position, RotationHelper.GetRotation(direction));
            gausCannon.SetUp(this.UnitsMap, this.BoardController, this.Unit.GetPosition(), this.Knockback, this.damage);
            this.transform.localPosition -= point;
        }
    }
}
