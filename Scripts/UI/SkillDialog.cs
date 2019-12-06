//-----------------------------------------------------------------------
// <copyright file="SkillDialog.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.StringBuilders;
    using Edu.Vfs.RoboRapture.Units;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillDialog : MonoBehaviour
    {
        public static Action<Unit, int> ActionPurchased;

        private Panel panel;

        [SerializeField]
        private Button purchaseButton;

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private RefInt experiencePoints;

        [SerializeField]
        private FXWrapper purchaseFx;

        private Unit currentUnit;

        private Units.Actions.Action currentAction;

        private int actionIndex;

        [SerializeField]
        private FXWrapper levelUpFx;

        private void Awake()
        {
            panel = GetComponentInChildren<Panel>();
            ShowPanel(false);
            purchaseButton.interactable = false;
            Selector.CancelledAction += HidePanel;
            PlayerActionButton.EnterButton += DisplayActionInfo;
            PlayerController.PlayerActionExecuted += this.HidePanel;
        }

        private void OnDisable()
        {
            Selector.CancelledAction -= HidePanel;
            PlayerActionButton.EnterButton -= DisplayActionInfo; 
            PlayerController.PlayerActionExecuted -= this.HidePanel;
        }
        
        public void PurchaseAction()
        {
            Logcat.I(this, $"Action purchased {this.currentUnit} index {this.actionIndex}");
            levelUpFx?.Play(this.currentUnit.transform.position);
            purchaseFx?.Play(this.transform.position);
            ActionPurchased?.Invoke(this.currentUnit, this.actionIndex);
            HidePanel();
        }

        private void DisplayActionInfo(Unit unit, Units.Actions.Action action, int index)
        {
            this.currentUnit = unit;
            this.currentAction = action;
            this.actionIndex = index;
            if (this.currentAction == null || this.currentUnit == null)
            {
                return;
            }

            IStringBuilder stringBuilder = new ActionInfoStringBuilder(action);
            text.text = stringBuilder.GetString();

            ShowPanel(true);
            EnableButton();
            purchaseButton?.gameObject.SetActive(!currentAction.IsUnlocked());
        }

        public void EnableButton()
        {
            if (currentAction == null || currentUnit == null)
            {
                return;
            }

            currentAction.AvailableToUnlock = SkillStoreController.ReadyToUnlock(this.currentAction, experiencePoints.Value);
            Logcat.I(this, $"Is the action available to unlock? {currentAction.AvailableToUnlock}");
            purchaseButton.interactable = currentAction.AvailableToUnlock;
        }

        private void ShowPanel()
        {
            this.ShowPanel(true);
        }

        public void HidePanel()
        {
            this.ShowPanel(false);
        }

        private void ShowPanel(bool show)
        {
            this.panel?.gameObject.SetActive(show);
        }
    }
}