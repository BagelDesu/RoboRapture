//-----------------------------------------------------------------------
// <copyright file="ExperiencePointsUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using System;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using UnityEngine;

    public class ExperiencePointsUpdater
    {
        private RefInt pointsEarned;

        public static Action<int> ExperiencePointsUpdated;

        public ExperiencePointsUpdater(RefInt pointsEarned)
        {
            this.pointsEarned = pointsEarned;
        }

        public void IncreaseEarnedPoints(int delta)
        {
            this.pointsEarned.Value += Mathf.Abs(delta);
            ExperiencePointsUpdated?.Invoke(this.pointsEarned.Value);
        }

        public void ReducePoints(int delta)
        {
            this.pointsEarned.Value -= Mathf.Abs(delta);
            if (this.pointsEarned.Value < 0)
            {
                this.pointsEarned.Value = 0;
            }

            ExperiencePointsUpdated?.Invoke(this.pointsEarned.Value);
        }

        public bool HaveEnoughPoints(int delta)
        {
            return this.pointsEarned.Value - Mathf.Abs(delta) >= 0;
        }

        public void Reset()
        {
            this.pointsEarned.Value = 0;
        }

        public int GetExperiencePoints()
        {
            return this.pointsEarned.Value;
        }
    }
}