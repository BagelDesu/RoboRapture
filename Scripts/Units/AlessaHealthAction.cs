//-----------------------------------------------------------------------
// <copyright file="AlessaHealthAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.UI;
    using UnityEngine;

    public class AlessaHealthAction : MonoBehaviour
    {
        private AlessaHealthUIObserver observer;

        private Health health;

        public void OnEnable()
        {
            this.health = this.GetComponent<Health>();
            observer = (AlessaHealthUIObserver) Resources.FindObjectsOfTypeAll(typeof(AlessaHealthUIObserver))[0];
            OnHealthChanged();
        }

        public void OnHealthChanged()
        {
            observer?.UpdateHealth(this.health);
        }
    }
}
