//-----------------------------------------------------------------------
// <copyright file="SkillStoreController.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Controllers
{
    using System;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;
    using Edu.Vfs.RoboRapture.UI;

    public class SkillStoreController : MonoBehaviour
    {
        public static Action<Unit, int> ActionAvailable;

        public static Action PurchaseCompleted;

        [SerializeField]
        private RefInt experiencePoints;

        private ExperiencePointsUpdater experiencePointsUpdater;

        public void OnEnable()
        {
            this.experiencePointsUpdater = new ExperiencePointsUpdater(this.experiencePoints);
            this.experiencePointsUpdater.Reset();

            SkillDialog.ActionPurchased += OnActionPurchased;
        }

        public void OnDisable()
        {
            SkillDialog.ActionPurchased -= OnActionPurchased;
        }

        public void OnActionPurchased(Unit unit, int actionIndex)
        {
            Logcat.I(this, $"Action Purchased {unit.UnitName} action {actionIndex}");
            int points = unit.ActionsHandler.GetActions()[actionIndex].ExperiencePointsToUnlocked;
            UnlockAction(unit, actionIndex);
            experiencePointsUpdater.ReducePoints(points);
            PurchaseCompleted?.Invoke();
        }

        private void UnlockAction(Unit unit, int actionIndex)
        {
            Units.Actions.Action unitAction = unit.ActionsHandler.GetActions()[actionIndex];
            if (!unitAction.AvailableToUnlock || !experiencePointsUpdater.HaveEnoughPoints(unitAction.ExperiencePointsToUnlocked))
            {
                return;
            }
            
            unitAction.CurrentExperiencePoints = unitAction.ExperiencePointsToUnlocked;
            unitAction.AvailableToUnlock = false;
        }

        public static bool ReadyToUnlock(Units.Actions.Action action, int experiencePoints)
        {
            return !action.IsUnlocked() && action.ExperiencePointsToUnlocked <= experiencePoints;
        }
    }
}