//-----------------------------------------------------------------------
// <copyright file="UpgradeAvailableText.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.Units;
    using TMPro;
    using UnityEngine;

    public class UpgradeAvailableText : MonoBehaviour
    {
        [SerializeField]
        private UnitType unitType;

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private UnitsMap map;

        [SerializeField]
        private RefInt experiencePoints;
       
        public void OnEnable()
        {
            ExperiencePointsUpdater.ExperiencePointsUpdated += this.OnEnxperiencePointUpdater;
            SkillDialog.ActionPurchased += this.OnActionPurchased;
            this.text.gameObject.SetActive(false);
        }

        public void OnDisable()
        {
            ExperiencePointsUpdater.ExperiencePointsUpdated -= this.OnEnxperiencePointUpdater;
            SkillDialog.ActionPurchased -= this.OnActionPurchased;
        }

        public void OnEnxperiencePointUpdater(int value)
        {
            this.TotalExperienceUnlocked();
        }

        private void OnActionPurchased(Unit unit, int index)
        {
            this.TotalExperienceUnlocked();
        }

        private void TotalExperienceUnlocked()
        {
            Unit unit = this.map.GetUnits(Type.Player).Find(u => u?.GetSpawnedUnitType() == this.unitType);
            if (unit == null)
            {
                return;
            }

            foreach (var action in unit.ActionsHandler.GetActions())
            {
                if (SkillStoreController.ReadyToUnlock(action, this.experiencePoints.Value))
                {
                    this.text.gameObject.SetActive(true);
                    return;
                }
            }

            this.text.gameObject.SetActive(false);
        }
    }
}