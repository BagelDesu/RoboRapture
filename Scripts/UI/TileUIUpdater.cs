//-----------------------------------------------------------------------
// <copyright file="TileUIUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Environment;
    using Edu.Vfs.RoboRapture.StringBuilders;
    using Edu.Vfs.RoboRapture.Units;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TileUIUpdater : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        
        [SerializeField]
        private Panel panel;

        [SerializeField]
        private TextMeshProUGUI info;

        private void Awake()
        {
            TileHovered.TileHoveredOn += ShowInfo;
            TileHovered.TileHoveredOff += HideInfo;

            SelectableHovered.UnitHoveredOn += ShowInfo;
            SelectableHovered.UnitHoveredOff += HideInfo;
        }

        private void OnDisable()
        {
            TileHovered.TileHoveredOn -= ShowInfo;
            TileHovered.TileHoveredOff -= HideInfo;

            SelectableHovered.UnitHoveredOn -= ShowInfo;
            SelectableHovered.UnitHoveredOff -= HideInfo;
        }

        private void ShowInfo(Unit unit)
        {
            if (unit.GetUnitType() != Type.Envrionment)
            {
                return;
            }

            image.gameObject.SetActive(unit.Picture != null);
            panel.gameObject.SetActive(true);

            DestroyableBlockersStringBuilder builder = new DestroyableBlockersStringBuilder((DestroyableBlockers) unit);
            image.sprite = unit.Picture;
            info.text = builder.GetString();
        }

        private void HideInfo(Unit unit)
        {
            if (unit.GetUnitType() != Type.Envrionment)
            {
                return;
            }

            image.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        }

        private void ShowInfo(Tile tile)
        {
            image.gameObject.SetActive(tile.Image != null);
            panel.gameObject.SetActive(true);

            IStringBuilder builder = new TileStringBuilder(tile);
            image.sprite = tile.Image;
            info.text = builder.GetString();
        }

        private void HideInfo(Tile tile)
        {
            image.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        }
    }
}