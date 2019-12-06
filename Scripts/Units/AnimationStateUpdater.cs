//-----------------------------------------------------------------------
// <copyright file="AnimationStateUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Units.Actions;
    using UnityEngine;

    public class AnimationStateUpdater : MonoBehaviour
    {
        protected string idle = "Idle";

        protected string dead = "Dead";

        protected string hit = "Hit";

        protected string attack = "Attack";

        private string special = "Special";

        [SerializeField]
        private bool hideWhenDead = false;

        [SerializeField]
        protected Animator animator;

        [SerializeField]
        private ActionsHandler attackAction;

        public virtual void Attack(Health health)
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
                this.animator.SetTrigger(this.idle);
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }

        public void PlayAttackAnimation(int action)
        {
            if (action == 1)
            {
                this.animator.SetTrigger(this.attack);
            }
            else if (action == 2 || action == 3)
            {
                this.animator.SetTrigger(this.special);
            }
        }

        /// <summary>
        /// Method called from an event when death animation ends.
        /// </summary>
        public void DeathEnded()
        {
            Health health = GetComponentInParent<Health>();
            this.RemoveFromUnitsMap();
            this.transform.parent.gameObject.SetActive(false);
            if (this.hideWhenDead || health.CauseOfDeath == CauseOfDeath.Rapture)
            {
                GetComponentInChildren<SpriteRenderer>().color = Color.clear;
            }
        }

        public void RemoveFromUnitsMap()
        {
            Unit unit = this.GetComponentInParent<Unit>();
            if (unit == null)
            {
                return;
            }

            Logcat.I(this, $"AnimationStateUpdater RemoveFromUnitsMap {unit.UnitName}");
            unit.UnitsMap.Remove(unit.GetPosition());
            GetComponentInParent<EnemyUnit>()?.NotifyDyingEvent();
            GetComponentInParent<PlayerUnit>()?.NotifyDyingEvent();
        }

        /// <summary>
        /// Method called from an event when attack animation ends.
        /// </summary>
        public void AttackEnded()
        {
            this.attackAction.Execute();
        }
    }
}