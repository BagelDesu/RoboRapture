//-----------------------------------------------------------------------
// <copyright file="MaximilianHealthAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.UI;
    using UnityEngine;

    public class MaximilianHealthAction : MonoBehaviour
    {
        private MaximilianHealthUIObserver observer;

        private Health health;

        public void OnEnable()
        {
            this.health = this.GetComponent<Health>();
            observer = (MaximilianHealthUIObserver) Resources.FindObjectsOfTypeAll(typeof(MaximilianHealthUIObserver))[0];
            OnHealthChanged();
        }

        public void OnHealthChanged()
        {
            observer?.UpdateHealth(this.health);
        }
    }
}