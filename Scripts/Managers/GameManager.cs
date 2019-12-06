//-----------------------------------------------------------------------
// <copyright file="GameManager.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Managers
{
    using System;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.AudioSystem;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;
    using Type = Units.Type;

    public class GameManager : MonoBehaviour
    {
        public static Action<bool> GameEnded;

        [SerializeField]
        private UnitsMap unitsMap;

        private void Awake()
        {
            PlayerUnit.PlayerUnitDied += PlayerUnitDefeated;
            EnemyUnit.EnemyWithTypeDied += EnemyUnitDefeated;
        }

        private void OnDisable()
        {
            PlayerUnit.PlayerUnitDied -= PlayerUnitDefeated;
            EnemyUnit.EnemyWithTypeDied -= EnemyUnitDefeated;
        }

        private void PlayerUnitDefeated(Point position, UnitType unitType)
        {
            List<Unit> playerUnits = unitsMap.GetUnits(Type.Player);
            if (playerUnits.Count == 0)
            {
                MusicController.Instance.PlayManual(MusicTypes.LOSE);
                GameEnded?.Invoke(false);
            }
        }

        private void EnemyUnitDefeated(Point position, UnitType unitType)
        {
            if (unitType == UnitType.NEOSATAN_HEAD)
            {
                // GameEnded.Invoke(true);
            }
        }
    }
}