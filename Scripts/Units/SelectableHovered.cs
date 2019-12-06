//-----------------------------------------------------------------------
// <copyright file="SelectableHovered.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using System;
    using UnityEngine;

    public class SelectableHovered : MonoBehaviour
    {
        public static Action<Unit> UnitHoveredOn;

        public static Action<Unit> UnitHoveredOff;

        private Unit unit;

        private void Awake()
        {
            this.unit = this.GetComponent<Unit>();
        }

        private void OnMouseEnter()
        {
            UnitHoveredOn?.Invoke(this.unit);
        }

        private void OnMouseExit()
        {
            UnitHoveredOff?.Invoke(this.unit);
        }
    }
}