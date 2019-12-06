//-----------------------------------------------------------------------
// <copyright file="PlayerUnitsSpawnManager.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Managers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.TerrainSystem;
    using Edu.Vfs.RoboRapture.UI;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class PlayerUnitsSpawnManager : MonoBehaviour
    {
        [SerializeField]
        private BoardController boardController;

        [SerializeField]
        private UnitsMap map;

        [SerializeField]
        private UnitsInfoUpdater unitsUIInfo;

        private Dictionary<UnitType, int> playerUnits;

        [SerializeField]
        private int turnsToRespawn = 3;

        [SerializeField]
        private float initialDelay = 2.5f;

        private System.Random random = new System.Random();

        private void OnEnable()
        {
            this.playerUnits = new Dictionary<UnitType, int>();
            PlayerUnit.PlayerUnitDied += this.PlayerUnitDefeated;
            PlayerController.PlayerTurnStarted += this.PlayerTurnStarted;
        }

        private void OnDisable()
        {
            PlayerUnit.PlayerUnitDied -= this.PlayerUnitDefeated;
            PlayerController.PlayerTurnStarted -= this.PlayerTurnStarted;
        }

        private void PlayerUnitDefeated(Point position, UnitType unitType)
        {
            playerUnits.Add(unitType, turnsToRespawn);
            unitsUIInfo?.UpdateTurnsToRespawn(unitType, turnsToRespawn);
            unitsUIInfo?.UpdateActionButtons(unitType, false);
        }

        private void PlayerTurnStarted()
        {
            StartCoroutine(PlaceUnits());
        }

        private IEnumerator PlaceUnits()
        {
            Logcat.I(this, "Player turn started");
            yield return new WaitForSeconds(initialDelay);

            foreach (var item in playerUnits.Keys.ToList())
            {
                playerUnits[item] = playerUnits[item] - 1;
                Logcat.I(this, $"{playerUnits[item]} turns to respawn {item}");
                unitsUIInfo?.UpdateTurnsToRespawn(item, playerUnits[item]);
                if (playerUnits[item] == 0)
                {
                    Logcat.I(this, $"Respawning unit {item}");
                    SpawnManager.Instance.SpawnUnit(item, GetValidPointToRespawn());
                    playerUnits.Remove(item);
                }
            }

            yield return null;
        }

        //// TODO use UI to place units
        private Point GetValidPointToRespawn()
        {
            List<Point> board = boardController.GetAllPointsWithNavigationTypeOf(TerrainNavigationType.BOTH);
            List<Point> excludingEnemies = board.Where(p => !map.Contains(p)).ToList();
            int index = random.Next(excludingEnemies.Count);
            return excludingEnemies[index];
        }
    }
}