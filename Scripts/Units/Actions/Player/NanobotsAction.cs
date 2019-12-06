//-----------------------------------------------------------------------
// <copyright file="NanobotsAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Player
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;

    public class NanobotsAction : SkillAction
    {
        public override bool ValidateAction(Point target)
        {
            bool isValid = base.ValidateAction(target) && this.UnitsMap.Contains(target, Type.Player);

            if (isValid)
            {
                this.SkillFxPosition = this.UnitsMap.Get(target).gameObject.transform.position;
            }

            Logcat.I(this, $"Nanobots validating action {isValid}");
            return isValid;
        }

        public override void Execute()
        {
            Logcat.I(this, $"Nanobots called");
           
            Unit targetUnit = this.UnitsMap.Get(this.Target);
            SimulateAttack(false, this.Target, this.DeltaHealth, this.Knockback, false);
            KnockbackHandler handler = new KnockbackHandler(this.UnitsMap);
            handler.Execute(this.BoardController, this.Unit.GetPosition(), targetUnit.GetPosition(), this.Knockback);
            targetUnit.Health.IncreaseHealth(this.DeltaHealth);
            Logcat.I(this, $"Nanobots unit healed");

            this.IsActive(false);
        }

        protected override void HighlightTileAttack(bool highlight, Point position)
        {
            base.HighlightTileAttack(highlight, position);
            SimulateAttack(highlight, position, this.DeltaHealth, this.Knockback, false);
        }
    }
}

