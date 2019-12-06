//-----------------------------------------------------------------------
// <copyright file="OnBoolValueChanged.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.ScriptableLibrary
{
    using NaughtyAttributes;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Sets a relationship between a reference and an unity event.
    /// </summary>
    public class OnBoolValueChanged : MonoBehaviour
    {
        /// <summary>
        /// Reference to track.
        /// </summary>
        [Required]
        [SerializeField]
        private RefBool refBool;

        /// <summary>
        /// Event to be dispatched when a change occurs to refFloat.
        /// </summary>
        [SerializeField]
        private UEvent_Bool valueChanged;

        /// <summary>
        /// Register the listeners to events.
        /// </summary>
        private void OnEnable()
        {
            this.refBool.Listeners += this.OnValueChanged;
            this.OnValueChanged();
        }

        /// <summary>
        /// Unregister the listeners from events.
        /// </summary>
        private void OnDisable()
        {
            this.refBool.Listeners -= this.OnValueChanged;
        }

        /// <summary>
        /// Event to invoke when a change occurs to the reference.
        /// </summary>
        private void OnValueChanged()
        {
            this.valueChanged.Invoke(this.refBool.Value);
        }

        /// <summary>
        /// UnityEvent to be invoked when the value changes.
        /// </summary>
        [System.Serializable]
        private class UEvent_Bool : UnityEvent<bool>
        {
        }
    }
}