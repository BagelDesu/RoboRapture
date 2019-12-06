//-----------------------------------------------------------------------
// <copyright file="UnitsController.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Controllers
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Managers;
    using Edu.Vfs.RoboRapture.TurnSystem;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public abstract class UnitsController : MonoBehaviour, IController, ITurnEntity
    {
        private List<Unit> units = new List<Unit>();

        [SerializeField]
        private EntityTurnManager turnManager;

        [SerializeField]
        private LevelManager levelManager;

        private Unit previousUnit;

        private Unit selectedUnit;

        protected LevelManager LevelManager { get => this.levelManager; set => this.levelManager = value; }

        protected Unit SelectedUnit { get => this.selectedUnit; set => this.selectedUnit = value; }

        protected Unit PreviousUnit { get => this.previousUnit; set => this.previousUnit = value; }

        protected EntityTurnManager TurnManager { get => this.turnManager; set => this.turnManager = value; }

        protected List<Unit> Units { get => this.units; set => this.units = value; }

        public abstract void Select(Point point);

        public abstract void StartTurn(EntityTurnManager turnManager);

        public abstract void EndTurn();

        protected abstract void Execute(Point point);

        protected void UpdatePreviousUnit()
        {
            if (this.SelectedUnit == null)
            {
                return;
            }

            this.PreviousUnit = this.SelectedUnit;
            this.PreviousUnit.ChangeState(StateType.Normal);
        }

        protected void UnHighlightUnits()
        {
            units?.ForEach(item => item.ChangeState(StateType.Normal));
        }
    }
}