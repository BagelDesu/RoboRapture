//-----------------------------------------------------------------------
// <copyright file="ExperiencePointsListener.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using UnityEngine;

    public class ExperiencePointsListener : MonoBehaviour
    {
        [SerializeField]
        private Health health;

        [SerializeField]
        private int points;

        [SerializeField]
        private RefInt experiencePoints;

        [SerializeField]
        private int turn;

        private ExperiencePointsUpdater experiencePointsUpdater;

        private bool updated = false;

        public void Awake()
        {
            this.experiencePointsUpdater = new ExperiencePointsUpdater(this.experiencePoints);
        }

        public void UpdateExperiencePointsEarned()
        {
            if (!this.health.IsDead())
            {
                return;
            }

            if (!updated)
            {
                Logcat.I(this, $"UpdateExperiencePointsEarned called");
                this.experiencePointsUpdater?.IncreaseEarnedPoints(this.points);
                updated = true;
            }
        }
    }
}
