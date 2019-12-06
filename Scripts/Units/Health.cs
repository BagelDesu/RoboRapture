//-----------------------------------------------------------------------
// <copyright file="Health.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using UnityEngine;

    /// <summary>
    /// Unit's life condition.
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// Encapsulates health data.
        /// </summary>
        private RefFloat value;

        /// <summary>
        /// Health's max value.
        /// </summary>
        [SerializeField]
        private float maxValue = 100;

        /// <summary>
        /// Health's min value.
        /// </summary>
        [SerializeField]
        private float minValue = 0;

        [SerializeField]
        private FXWrapper hitFx;

        [SerializeField]
        private FXWrapper deadFx;

        private CauseOfDeath causeOfDeath;

        private bool hasPlayed = false;

        private bool hasIncreasedHealth;

        public RefFloat Data { get => this.value; set => this.value = value; }

        public CauseOfDeath CauseOfDeath { get => causeOfDeath; private set => causeOfDeath = value; }

        public bool HasIncreasedHealth { get => hasIncreasedHealth; private set => hasIncreasedHealth = value; }

        /// <summary>
        /// Called when the object becomes enabled and active.
        /// </summary>
        public void OnEnable()
        {
            this.value = ScriptableObject.CreateInstance<RefFloat>();
            this.value.MaxValue = this.maxValue;
            this.value.MinValue = this.minValue;
            this.value.Value = this.maxValue; 
        }

        public void SetMaxHealth(float health)
        {
            this.value.MaxValue = health;
            this.value.Value = health;
        }

        /// <summary>
        /// Returns the total health of the unit.
        /// </summary>
        /// <returns>Total health of the unit.</returns>
        public float GetTotalHealth()
        {
            return this.value.MaxValue;
        }

        /// <summary>
        /// Returns the actual health of the unit.
        /// </summary>
        /// <returns>Actual health of the unit.</returns>
        public float GetCurrentHealth()
        {
            return this.value.Value;
        }

        public void SetCurrentHealth(float health)
        {
            this.value.Value = health;
        }

        /// <summary>
        /// Updates the unit's health with delta.
        /// </summary>
        /// <param name="delta">Value to reduce from the unit's health.</param>
        public void ReduceHealth(float delta)
        {
            this.hasIncreasedHealth = false;
            this.value.Value -= Mathf.Abs(delta);
            if (this.value.Value <= this.value.MinValue)
            {
                this.causeOfDeath = CauseOfDeath.HealthReachesZero;

                if (!this.hasPlayed)
                {
                    this.deadFx?.Play(this.transform.position);
                    this.hasPlayed = true;
                }

                if (this.value.Value < 0)
                {
                    this.value.Value = 0;
                }

                return;
            }

            this.hitFx?.Play(this.transform.position);            
        }

        public void DeadByRapture()
        {
            this.ReduceHealth(this.value.Value);
            this.causeOfDeath = CauseOfDeath.Rapture;
        }

        public void DeadByOneHit()
        {
            this.ReduceHealth(this.value.Value);
            this.causeOfDeath = CauseOfDeath.OneHit;
        }

        public void IncreaseHealth(float delta)
        {
            this.hasIncreasedHealth = true;
            this.value.Value += Mathf.Abs(delta);
            if (this.value.Value > this.value.MaxValue)
            {
                this.value.Value = this.value.MaxValue;
            }
        }

        public void Restore()
        {
            this.value.Value = this.value.MaxValue;
        }

        /// <summary>
        /// Returns true if the unit's health is less than MinValue.
        /// </summary>
        /// <returns>True if the unit's health is less than MinValue.</returns>
        public bool IsDead()
        {
            return this.value.Value <= this.value.MinValue;
        }
    }
}
