//-----------------------------------------------------------------------
// <copyright file="PlacementManager.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Environment;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Patterns;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.UI;
    using Edu.Vfs.RoboRapture.Units;
    using Edu.Vfs.RoboRapture.Validators;
    using UnityEngine;
    using static TileAuxillary.TileStates;

    public class PlacementManager : MonoBehaviour
    {
        public static Action PlacementCompleted;

        public static Action<List<int>> UnitsPlaced;

        [SerializeField]
        private UnitsMap map;

        [SerializeField]
        private RefPoint position;

        [SerializeField]
        private Unit[] playerPrefab;

        [SerializeField]
        private LevelManager levelManager;

        [SerializeField]
        private Transform unitsContainer;

        [SerializeField]
        private FXWrapper placementFx;

        private HashSet<Point> validPlacementPositions;

        private List<int> unitsPlacedList = new List<int>();

        private int index = - 1;

        [SerializeField]
        private float delay = 0.5f;

        private bool highlightTiles = false;

        public void Awake()
        {
            TileHovered.TileHoveredOn += TileHoveredOn;
            TileHovered.TileHoveredOff += TileHoveredOff;
            PlacementUI.PlacementSelectedUnit += OnPlacementSelectedUnit;

            UnitsPlaced?.Invoke(unitsPlacedList);
            Invoke("HighlightValidPositions", delay);
        }

        private void OnDisable()
        {
            TileHovered.TileHoveredOn -= TileHoveredOn;
            TileHovered.TileHoveredOff -= TileHoveredOff;
            PlacementUI.PlacementSelectedUnit -= OnPlacementSelectedUnit;
        }

        public void SetUnitPosition()
        {
            if (index < 0 || unitsPlacedList.Contains(index))
            {
                return;
            }

            Logcat.I(this, $"Setting player's unit at position {position.Value}");
            if (this.Add(unitsContainer.transform, position.Value, playerPrefab[index]))
            {
                placementFx?.Play(default);
                unitsPlacedList.Add(index);
                UnitsPlaced?.Invoke(unitsPlacedList);
                ClearHighlightedPositions();
                this.HighlightValidPositions();
            }

            if (AreAllUnitsPlaced())
            {
                position.Value = new Point(-1, -1, -1);
                ClearHighlightedPositions();
                PlacementCompleted?.Invoke();
            }
        }

        private void OnPlacementSelectedUnit(int index)
        {
            this.index = index;
        }

        private Unit Add(Transform parent, Point point, Unit unit)
        {
            InitialPlacementValidator placementValidator = new InitialPlacementValidator(map, point);
            return PlacementHelper.Add(parent, unit, point, placementValidator);
        }

        private void HighlightValidPositions()
        {
            if (this.levelManager == null || this.map == null)
            {
                return;
            }

            List<Point> boardPoints = PatternProcessor.Process(this.levelManager.GetBoard(TerrainSystem.TerrainNavigationType.BOTH), new SquarePattern(8), new Point(0, 0, 0), 0);
            List<Point> removingRows = boardPoints?.Where(currentPoint => (currentPoint.x > 1 && currentPoint.x < 4)).ToList();
            List<Point> removingUnits = removingRows?.Where(currentPoint => !map.Contains(currentPoint)).ToList();
            this.validPlacementPositions = new HashSet<Point>(removingUnits == null ? new List<Point>() : removingUnits);
            this.levelManager.BoardController?.SwitchTilesFromActiveBoards(this.validPlacementPositions, ActiveHighlight);
            highlightTiles = true; 
        }

        private void ClearHighlightedPositions()
        {
            highlightTiles = false;
            this.levelManager?.BoardController?.ClearAllActiveBoardsDecorations();
        }

        private bool AreAllUnitsPlaced()
        {
            List<Unit> playerUnits = map.GetUnits(Units.Type.Player);
            return playerUnits != null && playerUnits.Count() == playerPrefab.Length;
        }

        private void TileHoveredOn(Tile tile)
        {
            if (index < 0 || unitsPlacedList.Contains(index))
            {
                return;
            }

            this.HighlightTile(tile, true);
        }

        private void TileHoveredOff(Tile tile)
        {
            this.HighlightTile(tile, false);
        }

        private void HighlightTile(Tile tile, bool highlight)
        {
            if (this.validPlacementPositions == null || !this.validPlacementPositions.Contains(tile.GetPosition()) || !highlightTiles)
            {
                return;
            }

            HashSet<Point> points = new HashSet<Point>();
            points.Add(tile.GetPosition());
            this.levelManager.BoardController?.SwitchTilesFromActiveBoards(points, highlight? SelectionHighlight : ActiveHighlight);
        }
    }
}
