//-----------------------------------------------------------------------
// <copyright file="IncarnateAttackAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using System.Linq;
    using Edu.Vfs.RoboRapture.Helpers;
    using System.Collections.Generic;
    using UnityEngine;

    public class IncarnateAttackAction : SkillAction
    {
        [SerializeField]
        private FXWrapper healingFX;

        public override void Execute()
        {
            Logcat.I(this, $"Incarnate IncarnateAttackAction execute");
            base.Execute();
            Afterbirth();
        }

        private void Afterbirth()
        {
            List<Unit> enemies = this.UnitsMap.GetUnits(Type.Enemy);
            List<Unit> activeObject = enemies.Where(unit => unit.gameObject.activeSelf).ToList();
            List<Unit> activeEnemies = activeObject.Where(unit => unit.StructureType != StructureType.BODY_PART).ToList();
            List<Unit> sortedEnemies = activeEnemies.OrderBy(unit => unit.Health.GetCurrentHealth()).ToList();
            List<Unit> enemiesWithLowestHealth = sortedEnemies.Where(unit => unit.Health.GetCurrentHealth() == sortedEnemies.First().Health.GetCurrentHealth()).ToList();
            enemiesWithLowestHealth.ForEach(unit => unit.Health.IncreaseHealth(this.DeltaHealth));
            enemiesWithLowestHealth.ForEach(unit => healingFX?.Play(unit.transform.position));
        }
    }
}