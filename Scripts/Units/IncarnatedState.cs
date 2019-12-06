//-----------------------------------------------------------------------
// <copyright file="IncarnatedState.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using UnityEngine;

    public class IncarnatedState : MonoBehaviour
    {
        [SerializeField]
        private Material selected;

        [SerializeField]
        private Material normal;

        private StateRenderer stateRenderer;

        public void OnEnable()
        {
            this.stateRenderer = this.GetComponent<StateRenderer>();
        }

        public void SwitchMaterials()
        {
            this.stateRenderer.Normal = this.normal;
            this.stateRenderer.Target = this.normal;
            this.stateRenderer.Disabled = this.normal;
            this.stateRenderer.Selected = this.selected;
        }
    }
}