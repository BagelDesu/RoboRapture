//-----------------------------------------------------------------------
// <copyright file="IncarnateRebirthAttackAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using UnityEngine;

    public class IncarnateRebirthAttackAction : SkillAction
    {
        [SerializeField]
        private Unit mopelessPrefab;

        [SerializeField]
        private Unit udeukedefrukePrefab;

        [SerializeField]
        private Unit testamentPrefab;

        private UnitType[] enemiesExcluded = { UnitType.MAXIMILION, UnitType.ALESSA, UnitType.BRUTUS, UnitType.WHELP, UnitType.PHANTASM, UnitType.HELLBLOOM, UnitType.INCARNATE, UnitType.ZIGGURAT, UnitType.NEOSATAN_HEAD};

        private SpawnSystem.UnitType lastEnemyKilled;

        private new void OnEnable()
        {
            base.OnEnable();
            EnemyUnit.EnemyWithTypeDied += EnemyDied;    
        }

        private new void OnDisable()
        {
            base.OnDisable();
            EnemyUnit.EnemyWithTypeDied -= EnemyDied;    
        }

        public bool IsAnExcludedEnemy()
        {
            return (new List<UnitType>(enemiesExcluded)).Contains(lastEnemyKilled);
        }

        public override void Execute()
        {
            Logcat.I(this, $"Incarnate IncarnateRebirthAttackAction execute");
            base.Execute();
            if (IsAnExcludedEnemy())
            {
                return;
            }

            UnitsMap.Remove(this.Unit.GetPosition());
            StartCoroutine(Rebirth());  
        }

        private IEnumerator Rebirth()
        {
            Unit prefab = GetUnit(lastEnemyKilled);
            if (prefab == null)
            {
                yield return null;
            }

            Unit unit = AIPlacementHelper.AddUnit(null, this.Unit.GetPosition(), prefab);
            unit?.Health.SetMaxHealth(this.Unit.Health.GetTotalHealth());
            //// TODO CHECK unit?.GetComponent<IncarnatedState>()?.SwitchMaterials();
            this.transform.parent.gameObject.SetActive(false);
        }

        private void EnemyDied(Point point, SpawnSystem.UnitType enemy)
        {
            this.lastEnemyKilled = enemy;
        }

        private Unit GetUnit(UnitType type)
        {
            Unit unit = null;
            switch (type)
            {
                case UnitType.MOPELESS:
                    unit = this.mopelessPrefab;
                    break;
                case UnitType.UDEUKE:
                    unit = this.udeukedefrukePrefab;
                    break;
          
                case UnitType.TESTAMENT:
                    unit = this.testamentPrefab;
                    break;
            }

            return unit;
        }
    }
}