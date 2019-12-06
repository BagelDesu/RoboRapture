//-----------------------------------------------------------------------
// <copyright file="Debugger.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Debbugging
{
    using System.Linq;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.KillZoneSystem;
    using Edu.Vfs.RoboRapture.Scriptables;
    using NaughtyAttributes;
    using UnityEngine;

    public class Debugger : MonoBehaviour
    {
        [SerializeField]
        private BoardController boardController;

        [SerializeField]
        private UnitsMap map;

        [Button("Print All Unit Positions")]
        public void PrintAllUnitPositions()
        {
            Logcat.W(this, "========== Printing All Unit's Positions ==========");
            this.map.GetUnits().ForEach(u => Logcat.W(this, $"Unit {map.Get(u).GetUnitType()} {map.Get(u).name} at {u}, status {map.Get(u).gameObject.active}"));
        }

        [Button("Print All Valid Positions")]
        public void PrintAllValidPositions()
        {
            Logcat.W(this, "========== Printing All Board Valid Positions ==========");
            this.boardController.GetAllPointsWithNavigationTypeOf(TerrainSystem.TerrainNavigationType.BOTH).ForEach(t => Logcat.W(this, $"Tile {t}"));
        }

        [Button("Print All KillZones")]
        public void PrintAllKillZones()
        {
            Logcat.W(this, "========== Printing All KillZones ==========");
            KillZones.KillZoneCollection.ForEach(k => Logcat.W(this, $"Killzone {k}"));
        }

        [Button("Print Enemies On Environment Manager")]
        public void PrintEnemiesOnEnvironmentManager()
        {
            Logcat.W(this, "========== Printing All Enemies on Environment Manager ==========");
            EnvironmentManager.EnemyCollection.Keys.ToList().ForEach(e => Logcat.W($"Enemy {EnvironmentManager.EnemyCollection[e].SpawnedEnemyType} at {e}, status {EnvironmentManager.EnemyCollection[e].gameObject.active}"));
        }

        [Button("Print Environment Units On Environment Manager")]
        public void PrintEnvironmentUnitOnEnvironmentManager()
        {
            Logcat.W(this, "========== Printing All Enemies on Environment Manager ==========");
            EnvironmentManager.EnvironmentCollection.Keys.ToList().ForEach(e => Logcat.W($"Environment {EnvironmentManager.EnvironmentCollection[e]} at {e}"));
        }
    }
}