//-----------------------------------------------------------------------
// <copyright file="UndoMovementHelper.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Units;
    using Edu.Vfs.RoboRapture.Units.Actions;
    using Edu.Vfs.RoboRapture.Validators;
    using UnityEngine;
    using UnityEngine.UI;

    public class UndoMovementHelper : MonoBehaviour
    {
        [SerializeField]
        private RefPoint itemSelected;

        private Unit selectedUnit;

        private Point originalPosition;

        private Button undoButton;

        private bool wasMovementAction;





        public void OnEnable()
        {
            this.undoButton = GetComponentInChildren<Button>(true);
            PlayerController.UnitSelected += this.OnUnitSelected;
            PlayerController.PlayerUnitActionExecuted += this.OnActionExecuted;
            PlayerController.PlayerTurnEnded += this.OnPlayerTurnEnded;
        }

        public void OnDisable()
        {
            PlayerController.UnitSelected -= this.OnUnitSelected;
            PlayerController.PlayerUnitActionExecuted -= this.OnActionExecuted;
            PlayerController.PlayerTurnEnded -= this.OnPlayerTurnEnded;
        }

        public void UndoMovement()
        {
            if (this.selectedUnit == null)
            {
                return;
            }

            Logcat.I(this, $"Undo movement. Original position {this.originalPosition} final position {this.selectedUnit.GetPosition()}");
            PlacementHelper.Move(this.selectedUnit, this.originalPosition, new MoveUnitValidator(this.selectedUnit, this.originalPosition));
            this.undoButton.gameObject.SetActive(false);
            this.RevertStats();
            this.itemSelected.Value = default;
        }

        private void RevertStats()
        {
            if (this.selectedUnit == null)
            {
                return;
            }

            Logcat.I(this, $"RevertStats {this.selectedUnit.UnitName}");
            Action action = this.selectedUnit.ActionsHandler.GetActions()[0];
            action?.IsActive(false);
            action?.IsEnabled(true);
            this.selectedUnit = null;
        }

        private void OnUnitSelected(Unit unit)
        {
            if (unit == null)
            {
                return;
            }

            if (this.selectedUnit == null || this.selectedUnit != unit)
            {
                
                this.selectedUnit = unit;
                this.originalPosition = this.selectedUnit.GetPosition();
                this.undoButton.gameObject.SetActive(false);
                Logcat.I(this, $"OnActionSelected Selected Unit's position {this.originalPosition}");
            }
        }

        private void OnActionExecuted(Unit unit, Units.Actions.ActionType actionType)
        {
            if (this.selectedUnit != unit)
            {
                return;
            }

            this.wasMovementAction = actionType == ActionType.Movement;
            bool enableButton = this.wasMovementAction && selectedUnit != null;
            this.undoButton.gameObject.SetActive(enableButton);
        }

        private void OnPlayerTurnEnded()
        {
            this.selectedUnit = null;
            this.undoButton.gameObject.SetActive(false);
        }
    }
}