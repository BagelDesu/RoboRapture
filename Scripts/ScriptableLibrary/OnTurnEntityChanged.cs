//-----------------------------------------------------------------------
// <copyright file="OnTurnEntityChanged.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.ScriptableLibrary
{
    using Edu.Vfs.RoboRapture.TurnSystem;
    using NaughtyAttributes;
    using UnityEngine;
    using UnityEngine.Events;

    public class OnTurnEntityChanged : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private RefTurnEntity refTurnEntity;

        [SerializeField]
        private UEvent_TurnEntity valueChanged;

        /// <summary>
        /// Register the listeners to events.
        /// </summary>
        private void OnEnable()
        {
            this.refTurnEntity.Listeners += this.OnValueChanged;
            this.OnValueChanged();
        }

        /// <summary>
        /// Unregister the listeners from events.
        /// </summary>
        private void OnDisable()
        {
            this.refTurnEntity.Listeners -= this.OnValueChanged;
        }

        /// <summary>
        /// Event to invoke when a change occurs to the reference.
        /// </summary>
        private void OnValueChanged()
        {
            this.valueChanged.Invoke(this.refTurnEntity.Value);
        }

        [System.Serializable]
        private class UEvent_TurnEntity : UnityEvent<TurnEntities>
        {
        }
    }
}
