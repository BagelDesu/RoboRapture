//-----------------------------------------------------------------------
// <copyright file="PlayerActionButtonDisplayer.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Units;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerActionButtonDisplayer : MonoBehaviour
    {
        private PlayerActionButton[] buttons;

        private Button button;

        [SerializeField]
        private FXWrapper actionButtonPressed;

        private bool showButtons;

        public void Awake()
        {
            buttons = transform.parent.transform.parent.GetComponentsInChildren<PlayerActionButton>(true);
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.interactable = true;
            PlayerController.UnitSelected += OnUnitSelected;
        }

        private void OnDisable()
        {
            PlayerController.UnitSelected -= OnUnitSelected;
        }

        public void OnUnitSelected(Unit unit)
        {
            ShowButtons(true);
        }

        public void ShowButtons()
        {
            showButtons = !showButtons;
            ShowButtons(showButtons);
        }

        public void ShowButtons(bool show)
        {
            showButtons = show;
            Logcat.I(this, $"Showing action buttons {showButtons}, button's size {buttons?.Length}");
            buttons?.ToList().ForEach(b => b.gameObject.SetActive(!showButtons));
            actionButtonPressed?.Play(this.transform.position);
        }
    }
}