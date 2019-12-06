//-----------------------------------------------------------------------
// <copyright file="WhirlwindAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Player
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;

    public class WhirlwindAction : SkillAction
    {
        public override bool ValidateAction(Point target)
        {
            Logcat.I(this, $"WhirlwindAction validating action");
            SimulateAttack(true);
            return base.ValidateAction(target);
        }

        public override void Execute()
        {
            SimulateAttack(false);
            ValidPositions?.ForEach(p => AttackPositions(p));
            this.IsActive(false);
        }

        protected virtual void AttackPositions(Point point)
        {
            if (!UnitsMap.Contains(point))
            {
                return;
            }

            Unit targetUnit = this.UnitsMap.Get(point);
            KnockbackHandler handler = new KnockbackHandler(this.UnitsMap);
            handler.Execute(this.BoardController, this.Unit.GetPosition(), targetUnit.GetPosition(), this.Knockback);
            targetUnit?.Health.ReduceHealth(this.DeltaHealth);
        }

        protected override void HighlightTileAttack(bool highlight, Point position)
        {
            base.HighlightTileAttack(highlight, position);
            SimulateAttack(true);
        }

        private void SimulateAttack(bool highlight)
        {
            ValidPositions?.ForEach(p => SimulateAttack(highlight, p, this.DeltaHealth, this.Knockback, true));
        }
    }
}