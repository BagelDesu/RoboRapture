//-----------------------------------------------------------------------
// <copyright file="EnemyRangeHighlighter.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.TileAuxillary;
    using UnityEngine;

    public class EnemyRangeHighlighter : MonoBehaviour
    {
        private Unit unit;

        private BoardController boardController;

        private bool isPlayerUnitSelected;

        public void OnEnable()
        {
            this.unit = this.GetComponent<Unit>();
            this.boardController = MonoBehaviour.FindObjectOfType<BoardController>();
            SelectableHovered.UnitHoveredOn += this.HighlightActionRange;
            SelectableHovered.UnitHoveredOff += this.UnhighlightActionRange;
            PlayerController.UnitSelected += this.OnUnitSelected;
            EnemyController.UnitSelected += this.OnUnitSelected;
        }

        public void OnDisable()
        {
            SelectableHovered.UnitHoveredOn -= this.HighlightActionRange;
            SelectableHovered.UnitHoveredOff -= this.UnhighlightActionRange;
            PlayerController.UnitSelected -= this.OnUnitSelected;
            EnemyController.UnitSelected -= this.OnUnitSelected;
        }

        public void HighlightActionRange(Unit unit)
        {
            if (this.boardController == null || this.unit != unit)
            {
                return;
            }

            if (unit.GetSpawnedUnitType() == UnitType.INCARNATE || unit.GetSpawnedUnitType() == UnitType.UDEUKE || unit.GetSpawnedUnitType() == UnitType.ZIGGURAT || unit.GetSpawnedUnitType() == UnitType.NEOSATAN_HEAD)
            {
                return;
            }

            if (this.isPlayerUnitSelected)
            {
                this.UnhighlightActionRange(this.unit);
                return;
            }

            List<Point> board = this.boardController.GetAllPointsWithNavigationTypeOf(TerrainSystem.TerrainNavigationType.BOTH);
            int index = this.unit.ActionsHandler.GetActions().Count - 1;
            List<Point> actionRange = this.unit?.ActionsHandler.GetActions()[index]?.GetActionRange(board);
            this.boardController?.SwitchTilesFromActiveBoards(new HashSet<Point>(actionRange), TileStates.ActiveAttack);
        }

        public void UnhighlightActionRange(Unit unit)
        {
            if (this.unit != unit)
            {
                return;
            }

            this.boardController?.ClearAllActiveBoardsDecorationsOfType(TileStates.ActiveAttack);
        }

        private void OnUnitSelected(Unit playerUnit)
        {
            this.isPlayerUnitSelected = playerUnit != null;
            Logcat.I(this, $"Is player unit selected {this.isPlayerUnitSelected}");
        }
    }
}