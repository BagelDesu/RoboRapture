//-----------------------------------------------------------------------
// <copyright file="AlessaAnimationStateUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using UnityEngine;

    public class AlessaAnimationStateUpdater : AnimationStateUpdater
    {
        private string idleHealthy = "IdleHealthy";

        private string idleNearDeath = "IdleNearDeath";

        private string alessaIdlePerfect = "Alessa_IdlePerfect";

        [SerializeField]
        private Health health;

        public override void Attack(Health health)
        {
            if (!health.HasIncreasedHealth)
            {
                this.animator.SetTrigger(this.hit);
            }

            if (health.IsDead())
            {
                this.animator.SetTrigger(this.dead);
            }
            else
            {
                this.SetIdleAnimation(health);
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }

        private void SetIdleAnimation(Health health)
        {
            if (health.Data.Value == health.Data.MaxValue)
            {
                this.animator.SetTrigger(this.idle);
            }
            else if (health.Data.Value >= health.Data.MaxValue / 2)
            {
                this.animator.SetTrigger(this.idleHealthy);
            }
            else
            {
                this.animator.SetTrigger(this.idleNearDeath);
            }
        }

        private void Update()
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName(this.alessaIdlePerfect))
            {
                this.SetIdleAnimation(this.health);
            }
        }
    }
}
