//-----------------------------------------------------------------------
// <copyright file="OnPointValueChanged.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.ScriptableLibrary
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using NaughtyAttributes;
    using UnityEngine;
    using UnityEngine.Events;

    public class OnPointValueChanged : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private RefPoint refPoint;

        [SerializeField]
        private UEvent_Point valueChanged;

        /// <summary>
        /// Register the listeners to events.
        /// </summary>
        private void OnEnable()
        {
            this.refPoint.Listeners += this.OnValueChanged;
            this.OnValueChanged();
        }

        /// <summary>
        /// Unregister the listeners from events.
        /// </summary>
        private void OnDisable()
        {
            this.refPoint.Listeners -= this.OnValueChanged;
        }

        /// <summary>
        /// Event to invoke when a change occurs to the reference.
        /// </summary>
        private void OnValueChanged()
        {
            this.valueChanged.Invoke(this.refPoint.Value);
        }

        [System.Serializable]
        private class UEvent_Point : UnityEvent<Point>
        {
        }
    }
}
