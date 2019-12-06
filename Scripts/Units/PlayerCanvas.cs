//-----------------------------------------------------------------------
// <copyright file="PlayerCanvas.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.Helpers;
    using UnityEngine;

    public class PlayerCanvas : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        private Unit unit;

        private void OnEnable()
        {
            this.unit = GetComponent<Unit>();
            SelectableHovered.UnitHoveredOn += UnitSelected;
            SelectableHovered.UnitHoveredOff += UnitDeselected;
            Selector.CancelledAction += CancelAction;
        }

        private void OnDisable()
        {
            SelectableHovered.UnitHoveredOn -= UnitSelected;
            SelectableHovered.UnitHoveredOff -= UnitDeselected;
            Selector.CancelledAction -= CancelAction;
        }

        private void UnitSelected(Unit unit)
        {
            Logcat.I(this, $"Unit hovered {unit.UnitName}");
            canvas.gameObject.SetActive(this.unit == unit);
        }

        private void UnitDeselected(Unit unit)
        {
            Logcat.I(this, $"Unit unhovered {unit.UnitName}");
            canvas.gameObject.SetActive(false);
        }

        private void CancelAction()
        {
            canvas.gameObject.SetActive(false);
        }
    }
}