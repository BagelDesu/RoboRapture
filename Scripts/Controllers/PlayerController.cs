//-----------------------------------------------------------------------
// <copyright file="PlayerController.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Controllers
{
    using System;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.TurnSystem;
    using Edu.Vfs.RoboRapture.UI;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class PlayerController : UnitsController
    {
        public static Action<Unit> UnitSelected;

        public static Action PlayerTurnStarted;

        public static Action PlayerTurnEnded;

        public static Action<bool> AvailableActions;

        public static Action<List<Unit>> UnitsLoaded;

        public static Action<Units.Actions.ActionType> PlayerActionSelected;

        public static Action PlayerActionExecuted;

        public static Action<Unit, Units.Actions.ActionType> PlayerUnitActionExecuted;

        [SerializeField]
        private Unit unitPrefab;

        [SerializeField]
        private RefPoint itemSelected;

        [SerializeField]
        private EnemyController enemyController;

        [SerializeField]
        private RefInt playerTurn;

        public void Awake()
        {
            playerTurn.Value = 0;
            EntityTurnManager.RegisterEntity(this, TurnEntities.PLAYER);
        }

        private void OnEnable()
        {
            Selector.CancelledAction += OnCancelledAction;
            UnitInfo.SelectedUnitFromUI += SelectedUnitFromUI;
            this.Units = unitPrefab.UnitsMap.GetUnits(RoboRapture.Units.Type.Player);
            UnitsLoaded?.Invoke(this.Units);
        }

        private void OnDisable()
        {
            Selector.CancelledAction -= OnCancelledAction;
            UnitInfo.SelectedUnitFromUI -= SelectedUnitFromUI;
        }

        public override void Select(Point point)
        {
            this.LevelManager.BoardController?.ClearAllActiveBoardsDecorations();
            AvailableActions?.Invoke(AreMoreAvailableActions());

            if (this.SelectedUnit != null && this.SelectedUnit.ActionsHandler.IsActive())
            {
                this.Execute(point);
            }
            else if (this.unitPrefab.UnitsMap.Contains(point, Edu.Vfs.RoboRapture.Units.Type.Player) && !this.unitPrefab.UnitsMap.Get(point).Health.IsDead())
            {
                this.UpdatePreviousUnit();
                this.SelectedUnit = this.unitPrefab.UnitsMap.Get(point);
                this.SelectedUnit.ChangeState(StateType.Selected);
                this.SelectedUnit.VoFx?.Play(this.SelectedUnit.gameObject.transform.position);
                UnitSelected?.Invoke(SelectedUnit);
            }
            else if (this.SelectedUnit != null)
            {
                this.SelectedUnit.ChangeState(StateType.Normal);
                UnitSelected?.Invoke(null);
            }
        }

        public void ActivateSkill(int index)
        {
            if (this.SelectedUnit == null)
            {
                return;
            }

            if (!this.SelectedUnit.ActionsHandler.IsEnabled())
            {
                return;
            }

            this.SelectedUnit.ActionsHandler.ActivateAction(index); //// TODO check index
            PlayerActionSelected?.Invoke(this.SelectedUnit.ActionsHandler.GetActions()[index].ActionType);
            this.SelectedUnit.ActionsHandler.GetActions()[index].BoardController = this.LevelManager.BoardController;
            List<Point> points = this.SelectedUnit.ActionsHandler.GetValidTargets(this.LevelManager.GetBoard(TerrainSystem.TerrainNavigationType.BOTH), this.SelectedUnit.GetPosition());
            this.SelectedUnit.ChangeState(StateType.Selected);

            if (points == null)
            {
                return;
            }

            this.LevelManager.BoardController?.ClearAllActiveBoardsDecorations();
            TileAuxillary.TileStates state = (index == 0) ? TileAuxillary.TileStates.HIGHLIGHT : TileAuxillary.TileStates.ATTACK;
            this.LevelManager.BoardController?.SwitchTilesFromActiveBoards(new HashSet<Point>(points), state);
        }

        public bool IsPlayerUnitActive()
        {
            return this.SelectedUnit != null;
        }

        public void CleanSelection()
        {
            this.itemSelected.Value = new Point(-1, 0, -1);
            this.LevelManager.BoardController?.ClearAllActiveBoardsDecorations();
        }

        public override void StartTurn(EntityTurnManager turnManager)
        {
            this.Units = unitPrefab.UnitsMap.GetUnits(RoboRapture.Units.Type.Player);
            if (this.Units.Count == 0)
            {
                return;
            }

            this.CleanSelection();
            this.enemyController.CleanSelection();

            if (this.TurnManager == null)
            {
                this.TurnManager = turnManager;
            }

            this.EnableActions();
            this.RefreshReactivationTime();

            this.playerTurn.Value++;
           
            this.Units.ForEach(item => item.Turn = playerTurn.Value);

            PlayerTurnStarted?.Invoke();
        }

        public override void EndTurn()
        {
            if (this.TurnManager == null || this.TurnManager.ActiveTurnEntity != TurnEntities.PLAYER)
            {
                return;
            }

            this.CleanSelection();
            this.enemyController.CleanSelection();

            this.UnHighlightUnits();
            TurnManager.NextEntityTurn();
            PlayerTurnEnded?.Invoke();
        }

        protected override void Execute(Point point)
        {
            this.LevelManager.BoardController.ClearAllActiveBoardsDecorationsOfType(TileAuxillary.TileStates.HIGHLIGHT);
            bool isAvailable = this.SelectedUnit.ActionsHandler.ValidateAction(point);
            if (isAvailable)
            {
                int actionIndex = SelectedUnit.ActionsHandler.ActionsIndex.Value;
                this.SelectedUnit.GetComponentInChildren<AnimationStateUpdater>().PlayAttackAnimation(SelectedUnit.ActionsHandler.ActionsIndex.Value);
                this.SelectedUnit.ActionsHandler.Execute(point);
                this.SelectedUnit.ActionsHandler.IsActive(false);
                this.SelectedUnit.ActionsHandler.IsEnabled(false);
                AvailableActions?.Invoke(AreMoreAvailableActions());
                this.Select(this.SelectedUnit.GetPosition());
                PlayerActionExecuted?.Invoke();
                PlayerUnitActionExecuted?.Invoke(this.SelectedUnit, this.SelectedUnit.ActionsHandler.GetActions()[actionIndex].ActionType);
            }

            this.SelectedUnit.ChangeState(StateType.Normal);
            this.SelectedUnit = null;
            UnitSelected?.Invoke(null);
        }

        private bool AreMoreAvailableActions()
        {
            foreach (var item in Units)
            {
                if (!item.Health.IsDead() && item.ActionsHandler.AreAvailableActions())
                {
                    return true;
                }
            }
            return false;
        }

        private void EnableActions()
        {
            foreach (var item in this.Units)
            {
                if (item == null || item.Health.IsDead())
                {
                    continue;
                }

                item.ActionsHandler.EnableActions();
                item.ActionsHandler.WasActionExecutedInTheTurn = false;
            }
        }

        private void RefreshReactivationTime()
        {
            foreach (var item in this.Units)
            {
                if (item.Health.IsDead())
                {
                    continue;
                }

                item.ActionsHandler.RefreshReactivationTime();
            }
        }

        private void SelectedUnitFromUI(UnitType unitType)
        {
            List<Unit> units = unitPrefab.UnitsMap.GetUnits(RoboRapture.Units.Type.Player);
            foreach (var unit in units)
            {
                if (unit.GetSpawnedUnitType() == unitType)
                {
                    Select(unit.GetPosition());
                    return;
                }
            }
        }

        private void OnCancelledAction()
        {
            Select(new Point(-1, -1, -1));
            UnitSelected?.Invoke(null);
        }
    }
}
