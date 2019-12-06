//-----------------------------------------------------------------------
// <copyright file="WhelpAttackAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;

    public class WhelpAttackAction : SkillAction
    {
        public override void Execute()
        {
            if (this.UnitsMap.Contains(this.Target))
            {
                Unit targetUnit = this.UnitsMap.Get(this.Target);
                KnockbackHandler handler = new KnockbackHandler(this.UnitsMap);
                handler.Execute(this.BoardController, this.Unit.GetPosition(), targetUnit.GetPosition(), this.Knockback);
                targetUnit.Health.ReduceHealth(this.DeltaHealth);
            }

            this.IsActive(false);

            Logcat.I(this, "Whelp attack action executed");
        }
    }
}