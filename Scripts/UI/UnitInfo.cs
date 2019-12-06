//-----------------------------------------------------------------------
// <copyright file="UnitInfo.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.Units;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using static Edu.Vfs.RoboRapture.UI.HighlightingTypes;

    public class UnitInfo : MonoBehaviour, IPointerClickHandler
    {
        public static Action<UnitType> SelectedUnitFromUI;

        [SerializeField]
        private SpawnSystem.UnitType unitType;

        [SerializeField]
        private TextMeshProUGUI unitName;

        [SerializeField]
        private ChicletsUI chiclets;

        [SerializeField]
        private UnitActionButtons actionButtons;

        [SerializeField]
        private TextMeshProUGUI turnsToRespawn;

        [SerializeField]
        private Image background;

        [SerializeField]
        private Sprite normal;

        [SerializeField]
        private Sprite selected;

        public UnitType UnitType { get => unitType; private set => unitType = value; }

        public void Init(Health health)
        {
            this.Highlight(Options.Normal);
            this.chiclets.SetUp(health);
        }

        public void EnableActionButtons(bool enable)
        {
            actionButtons.ActivateButton(Units.Actions.ActionType.Movement, enable);
            actionButtons.ActivateButton(Units.Actions.ActionType.Action, enable);
        }

        private void OnEnable()
        {
            PlayerController.PlayerTurnStarted += OnPlayerTurnStarted;
            PlayerController.PlayerUnitActionExecuted += OnUnitActionExecuted;
        }

        private void OnDisable()
        {
            PlayerController.PlayerTurnStarted -= OnPlayerTurnStarted;
            PlayerController.PlayerUnitActionExecuted -= OnUnitActionExecuted;
        }

        private void OnPlayerTurnStarted()
        {
            EnableActionButtons(string.IsNullOrEmpty(turnsToRespawn.text));
        }

        private void OnUnitActionExecuted(Unit unit, Units.Actions.ActionType type)
        {
            if (unitType != unit.GetSpawnedUnitType())
            {
                return;
            }

            actionButtons.ActivateButton(type, false);
        }

        public void Highlight(Options highlighting)
        {
            switch (highlighting)   
            {
                case Options.Normal:
                    background.sprite = normal;
                    break;
                case Options.New_Skill_Available:
                    background.sprite = normal;
                    break;
                case Options.Selected:
                    background.sprite = selected;
                    break;
                case Options.Unavailable:
                    background.sprite = normal;
                    break;
                default:
                    break;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SelectedUnitFromUI?.Invoke(unitType);
        }

        public void UpdateTurnsToRespawn(int turns)
        {
            string text = turns == 0 ? string.Empty : turns.ToString();
            turnsToRespawn.text = text;
        }
    }
}