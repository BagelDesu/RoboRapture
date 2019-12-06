//-----------------------------------------------------------------------
// <copyright file="SkillButton.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Units;
    using Edu.Vfs.RoboRapture.Units.Actions;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillButton : MonoBehaviour
    {
        [SerializeField]
        private int actionIndex;

        [SerializeField]
        private RefInt experiencePoints;

        [SerializeField]
        private FXWrapper lockedActionFx;

        private Button button;

        private TextMeshProUGUI xp;

        private Unit unit;

        private SkillAction action;

        private int experiencePointsToUnlock;

        private void Awake()
        {
            button = GetComponent<Button>();
            xp = GetComponentInChildren<TextMeshProUGUI>();
            unit = GetComponentInParent<Unit>();
            action = (SkillAction) unit.ActionsHandler.GetActions()[actionIndex];
            experiencePointsToUnlock = action.ExperiencePointsToUnlocked;
        }

        private void OnEnable()
        {
            xp.text = experiencePointsToUnlock.ToString();
            HighlightButton();
        }

        private void HighlightButton()
        {
            bool availableToUnlock = SkillStoreController.ReadyToUnlock(action, experiencePoints.Value);
            HighlightingTypes.Options color = availableToUnlock ? HighlightingTypes.Options.New_Skill_Available : HighlightingTypes.Options.Unavailable;
            button.image.color = HighlightingTypes.GetColor(color);
        }

        public void OnClick()
        {
            lockedActionFx?.Play(this.transform.position);
        }
    }
}