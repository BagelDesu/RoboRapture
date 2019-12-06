//-----------------------------------------------------------------------
// <copyright file="EnemyInfoStringBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
using Edu.Vfs.RoboRapture.Units;
using Edu.Vfs.RoboRapture.Units.Actions;
using System.Text;

namespace Edu.Vfs.RoboRapture.StringBuilders
{
    public class EnemyInfoStringBuilder : IStringBuilder
    {
        private EnemyUnit enemyUnit;

        public EnemyInfoStringBuilder(EnemyUnit enemyUnit)
        {
            this.enemyUnit = enemyUnit;
        }

        public string GetString()
        {
            float limitToShowHealth = 100;
            StringBuilder info = new StringBuilder();
            info.Append("<b><size=45><color=#ff0000ff>").Append(enemyUnit.UnitName).Append("</color></size></b>\n")
            .Append(enemyUnit.Description).Append("\n")
            .Append("<size=30>");
            if (enemyUnit.Health != null && enemyUnit.Health.GetTotalHealth() < limitToShowHealth)
            {
                info.Append("\n")
                .Append("HP: ").Append(enemyUnit.Health.GetCurrentHealth()).Append("/").Append(enemyUnit.Health.GetTotalHealth()).Append("\n");
            }
            else
            {
                info.Append("\n");
            }

            info.Append("Movement Range: ").Append(enemyUnit.Range()).Append("\n");

            float deltaDamage = GetDeltaDamage(enemyUnit);
            string damage = deltaDamage == 0 ? "?" : deltaDamage.ToString();
            info.Append("Attack Damage: ").Append(damage).Append("\n");
            bool knockback = AfflictsKnockback(enemyUnit);
            info.Append("Afflicts Knockback: ").Append(knockback ? "Yes" : "No")
                .Append("</size>");
                
            return info.ToString();
        }

        private float GetDeltaDamage(Unit unit)
        {
            float deltaDamage = 0;
            if (unit.ActionsHandler == null || unit.ActionsHandler.GetActions() == null)
            {
                return deltaDamage;
            }
       
            int attackIndex = unit.ActionsHandler.GetActions().Count - 1;
            if (unit.ActionsHandler.GetActions()[attackIndex] is SkillAction)
            {
                deltaDamage = ((SkillAction)unit.ActionsHandler.GetActions()[attackIndex]).DeltaHealth;
            }

            return deltaDamage;
        }

        private bool AfflictsKnockback(Unit unit)
        {
            bool afflictsKnockback = false;
            if (unit.ActionsHandler == null || unit.ActionsHandler.GetActions() == null)
            {
                return afflictsKnockback;
            }

            int attackIndex = unit.ActionsHandler.GetActions().Count - 1;
            if (unit.ActionsHandler.GetActions()[attackIndex] is SkillAction)
            {
                afflictsKnockback = ((SkillAction)unit.ActionsHandler.GetActions()[attackIndex]).Knockback != 0 ? true : false;
            }

            return afflictsKnockback;
        }
    }
}