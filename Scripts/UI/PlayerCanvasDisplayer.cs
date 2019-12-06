//-----------------------------------------------------------------------
// <copyright file="PlayerCanvasDisplayer.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.GameMachine.States;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class PlayerCanvasDisplayer : MonoBehaviour
    {
        [SerializeField]
        private Canvas actionCanvas;

        [SerializeField]
        private Canvas healthCanvas;

        private Unit unit;

        private bool isPlayersTurn = true;

        private void OnEnable()
        {
            this.unit = GetComponent<Unit>();
            PlayerController.PlayerActionSelected += OnActionSelected;
            PlayerController.PlayerActionExecuted += OnActionExecuted;
            PlayerController.UnitSelected += OnUnitSelected;
            PlayerController.PlayerTurnStarted += this.OnPlayerTurnStarted;
            PlayerController.PlayerTurnEnded += this.OnPlayerTurnEnded;
            SkillDialog.ActionPurchased += OnActionPurchased;
            Selector.CancelledAction += OnActionExecuted;
        }

        private void OnDisable()
        {
            PlayerController.PlayerActionSelected -= OnActionSelected;
            PlayerController.PlayerActionExecuted -= OnActionExecuted;
            PlayerController.PlayerTurnStarted -= this.OnPlayerTurnStarted;
            PlayerController.PlayerTurnEnded -= this.OnPlayerTurnEnded;
            PlayerController.UnitSelected -= OnUnitSelected;
            SkillDialog.ActionPurchased -= OnActionPurchased;
            Selector.CancelledAction -= OnActionExecuted;
        }

        private void OnPlayerTurnStarted()
        {
            isPlayersTurn = true;
        }

        private void OnPlayerTurnEnded()
        {
            isPlayersTurn = false;
        }

        private void OnActionSelected(Units.Actions.ActionType actionType)
        {
            ShowCanvas(false);
        }

        private void OnActionPurchased(Unit unit, int index)
        {
            ShowCanvas(false);
        }

        private void OnActionExecuted()
        {
            ShowCanvas(false);
        }

        private void OnUnitSelected(Unit unit)
        {
            ShowCanvas(this.unit == unit);
        }

        private void ShowCanvas(bool show)
        {
            if (!isPlayersTurn || PlacementState.isInPlacementPhase)
            {
                show = false;
            }

            actionCanvas.gameObject.SetActive(show);
            healthCanvas.gameObject.SetActive(show);
        }

        public void OnItemSelected(RefPoint point)
        {
            ShowCanvas(point.Value == this.unit.GetPosition());
        }
    }
}