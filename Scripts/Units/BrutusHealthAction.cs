//-----------------------------------------------------------------------
// <copyright file="BrutusHealthAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.UI;
    using UnityEngine;

    public class BrutusHealthAction : MonoBehaviour
    {
        private BrutusHealthUIObserver observer;

        private Health health;

        public void OnEnable()
        {
            this.health = this.GetComponent<Health>();
            observer = (BrutusHealthUIObserver) Resources.FindObjectsOfTypeAll(typeof(BrutusHealthUIObserver))[0];
            OnHealthChanged();
        }

        public void OnHealthChanged()
        {
            observer?.UpdateHealth(this.health);
        }
    }
}