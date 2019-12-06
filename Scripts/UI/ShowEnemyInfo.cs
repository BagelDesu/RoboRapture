//-----------------------------------------------------------------------
// <copyright file="ShowEnemyInfo.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class ShowEnemyInfo : MonoBehaviour
    {
        [SerializeField]
        private HealthBarDisplayer healthBarDisplayer;

        private Unit unit;

        private bool isPlayerUnitSelected;

        private void OnEnable()
        {
            SelectableHovered.UnitHoveredOn += ShowInfo;
            SelectableHovered.UnitHoveredOff += HideInfo;
            this.unit = GetComponent<Unit>();
        }

        private void OnDisable()
        {
            SelectableHovered.UnitHoveredOn -= ShowInfo;
            SelectableHovered.UnitHoveredOff -= HideInfo;
        }

        private void ShowInfo(Unit unit)
        {
            ShowInfo(unit, true);
        }

        private void HideInfo(Unit unit)
        {
            ShowInfo(unit, false);
        }

        private void ShowInfo(Unit unit, bool show)
        {
            if (this.unit != unit)
            {
                return;
            }

            if (this.isPlayerUnitSelected)
            {
                return;
            }

            healthBarDisplayer?.Show(show);
        }
    }
}