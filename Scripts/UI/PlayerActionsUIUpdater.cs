//-----------------------------------------------------------------------
// <copyright file="PlayerActionsUIUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.Helpers;
    using UnityEngine;

    public class PlayerActionsUIUpdater : MonoBehaviour
    {
        [SerializeField]
        private PlayerActionButton[] actionButtons;

        [SerializeField]
        private PlayerActionButtonDisplayer actionButton;

        [SerializeField]
        private FXWrapper subActionButtonPressed;

        public void OnEnable()
        {
            PlayerController.PlayerActionExecuted += OnActionExecuted;
        }

        public void OnDisable()
        {
            PlayerController.PlayerActionExecuted -= OnActionExecuted;
        }

        public void SelectAction(int index)
        {
            Logcat.I(this, $"Selecting action {index}");
            this.actionButtons[index].SelectAction();
            subActionButtonPressed?.Play(this.transform.position);
        }

        private void OnActionExecuted()
        {
            this.actionButton.ShowButtons(false);
        }
    }
}