//-----------------------------------------------------------------------
// <copyright file="PlayerActionButton.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Units;
    using Edu.Vfs.RoboRapture.Units.Actions;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerActionButton : MonoBehaviour
    {
        public static System.Action<Unit, Units.Actions.Action, int> EnterButton;
        
        [SerializeField]
        private int index;

        [SerializeField]
        private Button skillButton;

        [SerializeField]
        private Button button;

        [SerializeField]
        private TMPro.TextMeshProUGUI coolDown;

        [SerializeField]
        private RefInt actionSelected;

        private Unit unit;

        private Units.Actions.Action action;

        public Button Button { get => this.button; private set => this.button = value; }

        public Action Action { get => action; set => action = value; }

        public void Awake()
        {
            this.unit = GetComponentInParent<Unit>();
            this.action = unit.ActionsHandler.GetActions()[index];
        }

        private void OnEnable()
        {
            ButtonHover.ButtonHovered += OnButtonHovered;
            bool isUnlocked = action.IsUnlocked();
            skillButton?.gameObject.SetActive(!isUnlocked);
            button.gameObject.SetActive(isUnlocked);
            button.interactable = action.IsEnabled() && action.IsUnlocked() && action.IsReadyToUse();

            if (index != 0 && button.interactable)
            {
                button.interactable = !this.unit.ActionsHandler.WasActionExecutedInTheTurn;
            }

            this.RefreshCoolDown(action.TurnsToReactivate());
        }

        private void OnDisable()
        {
            ButtonHover.ButtonHovered -= OnButtonHovered;
        }

        public void SelectAction()
        {
            this.actionSelected.Value = index;
            EnterButton?.Invoke(unit, action, index);
        }

        public void RefreshCoolDown(int coolDown)
        {
            this.coolDown.text = coolDown <= 0 ? string.Empty : coolDown.ToString();
        }

        public void DisplaySkillDialog()
        {
            EnterButton?.Invoke(unit, action, index);
        }

        private void OnButtonHovered(bool hovered, int index)
        {
            if (hovered && index == this.index)
            {
                EnterButton?.Invoke(unit, action, index);
            }
        }
    }
}