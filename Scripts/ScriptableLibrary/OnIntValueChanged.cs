//-----------------------------------------------------------------------
// <copyright file="OnIntValueChanged.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.ScriptableLibrary
{ 
    using NaughtyAttributes;
    using UnityEngine;
    using UnityEngine.Events;

    public class OnIntValueChanged : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private RefInt refInt;

        [SerializeField]
        private UEvent_Int valueChanged;

        private void OnEnable()
        {
            this.refInt.Listeners += this.OnValueChanged;
            this.OnValueChanged();
        }

        private void OnDisable()
        {
            this.refInt.Listeners -= this.OnValueChanged;
        }

        private void OnValueChanged()
        {
            this.valueChanged.Invoke(this.refInt.Value);
        }

        [System.Serializable]
        private class UEvent_Int : UnityEvent<int>
        {
        }
    }
}
