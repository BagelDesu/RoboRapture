//-----------------------------------------------------------------------
// <copyright file="OnHealthValueChanged.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.ScriptableLibrary
{
    using Edu.Vfs.RoboRapture.Units;
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Notify to the listeners when <see cref="HealthValue"/> is updated.
    /// </summary>
    public class OnHealthValueChanged : MonoBehaviour
    {
        [SerializeField]
        private Health health;

        [SerializeField]
        public UEvent_Float _OnValueChanged;

        private void OnEnable()
        {
            if (this.health != null && this.health.Data != null)
            {
                this.health.Data.Listeners += this.OnValueChanged;
                this.OnValueChanged();
            }
        }

        private void OnDisable()
        {
            if (this.health != null)
            {
                this.health.Data.Listeners -= this.OnValueChanged;
            }
        }

        public void Attach(Action action)
        {
            this.health.Data.Listeners += action;
        }

        public void Dettach(Action action)
        {
            this.health.Data.Listeners -= action;
        }

        private void OnValueChanged()
        {
            this._OnValueChanged.Invoke(this.health.Data.Value / this.health.Data.MaxValue);
        }

        [System.Serializable]
        public class UEvent_Float : UnityEvent<float>
        {
        }
    }
}
