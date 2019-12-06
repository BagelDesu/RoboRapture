//-----------------------------------------------------------------------
// <copyright file="AllHailTheMightyRobotAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Player
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Knockbacks;

    public class AllHailTheMightyRobotAction : SkillAction
    {
        public override bool ValidateAction(Point target)
        {
            SimulateAttack(true);
            Logcat.I(this, $"All Hail The Mighty Robot Action validating action");
            return true;
        }

        public override void Execute(Point target)
        {
            if (target == default)
            {
                return;
            }

            this.Target = target;
            FaceTargetDirection(target);

            Logcat.I(this, $"AllHailTheMightyRobotAction Action executed {Unit.UnitName} - {SkillName}, target {target}");
            SimulateAttack(false);
            SkillActionExecuted?.Invoke(this, target);
        }

        public override void Execute()
        {
            Logcat.I(this, $"All Hail The Mighty Robot Action called");
            
            this.HealAllies();
            this.PullAll();
        }

        private void HealAllies()
        {
            List<Unit> playerUnits = this.UnitsMap.GetUnits(Type.Player);
            playerUnits.ForEach(unit => unit.Health.IncreaseHealth(this.DeltaHealth));
            playerUnits.ForEach(unit => SkillActionFX.Play(unit.gameObject.transform.position));
        }

        private void PullAll()
        {
            KnockbackHandler handler = new KnockbackHandler(this.Unit.UnitsMap);
            List<Unit> enemyUnits = UnitsMap.GetUnits(Type.Enemy);
            enemyUnits.AddRange(UnitsMap.GetUnits(Type.Player));
            enemyUnits.ForEach(enemyUnit => this.PullUnit(handler, enemyUnit));
        }

        private void PullUnit(KnockbackHandler handler, Unit enemyUnit)
        {
            handler.InverseKnockback(this.BoardController, this.Unit.GetPosition(), enemyUnit.GetPosition(), this.Knockback);
        }

        protected override void HighlightTileAttack(bool highlight, Point position)
        {
            base.HighlightTileAttack(highlight, position);
            SimulateAttack(true);
        }

        private void SimulateAttack(bool highlight)
        {
            List<Unit> playerUnits = this.UnitsMap.GetUnits(Type.Player);
            List<Unit> enemyUnits = UnitsMap.GetUnits(Type.Enemy);

            enemyUnits.ForEach(unit => SimulateAttack(highlight, unit.GetPosition(), 0, -this.Knockback, false));
            playerUnits.ForEach(unit => SimulateAttack(highlight, unit.GetPosition(), this.DeltaHealth, -this.Knockback, false));
        }
    }
}