//-----------------------------------------------------------------------
// <copyright file="UdeukedefrukeAnimationStateUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Units.Enemies;
    using UnityEngine;

    public class UdeukedefrukeAnimationStateUpdater : AnimationStateUpdater
    {
        private string idleHead = "Idle_Head";

        private string hitHead = "Hit_Head";

        [SerializeField]
        private UdeukedefrukeBehaviour behaviour;

        public void GetHit(Health health)
        {
            Logcat.I(this, $"IsWithHeadAndMask? {behaviour.IsWithHeadAndMask}");
            if (!health.HasIncreasedHealth)
            {
                this.animator.SetTrigger(behaviour.IsWithHeadAndMask ? this.hit : this.hitHead);
            }

            if (health.IsDead())
            {
                this.animator.SetTrigger(this.dead);
            }
            else
            {
                this.animator.SetTrigger(behaviour.IsWithHeadAndMask ? this.idle : this.idleHead);
            }
        }

        public void Extrude()
        {
            this.animator.SetBool(this.attack, true);
        }

        public void SuckIn()
        {
            this.animator.SetBool(this.attack, false);
        }
    }
}