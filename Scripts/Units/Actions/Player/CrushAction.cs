//-----------------------------------------------------------------------
// <copyright file="CrushAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Player
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;

    public class CrushAction : SkillAction
    {
        public override bool ValidateAction(Point target)
        {
            Logcat.I(this, $"DefensiveShot validating action");
            return base.ValidateAction(target) && this.UnitsMap.Get(target) != this.Unit;
        }

        public override void Execute()
        {
            if (!this.ValidateAction(this.Target))
            {
                return;
            }

            Unit targetUnit = this.UnitsMap.Get(this.Target);
            if (targetUnit != null)
            {
                SimulateAttack(false, this.Target, this.DeltaHealth, this.Knockback, true);
                KnockbackHandler handler = new KnockbackHandler(this.UnitsMap);
                handler.Execute(this.BoardController, this.Unit.GetPosition(), targetUnit.GetPosition(), this.Knockback);
                targetUnit?.Health.ReduceHealth(this.DeltaHealth);
            }

            this.IsActive(false);
            Logcat.I(this, "Crush Action executed");
        }

        protected override void HighlightTileAttack(bool highlight, Point position)
        {
            base.HighlightTileAttack(highlight, position);
            SimulateAttack(highlight, position, this.DeltaHealth, this.Knockback, true);
        }
    }
}