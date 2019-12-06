//-----------------------------------------------------------------------
// <copyright file="TileHovered.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Environment
{
    using System;
    using UnityEngine;

    public class TileHovered : MonoBehaviour
    {
        public static Action<Tile> TileHoveredOn;

        public static Action<Tile> TileHoveredOff;

        [SerializeField]
        private Tile tile;

        private void OnMouseEnter()
        {
            TileHoveredOn?.Invoke(this.tile);
        }

        private void OnMouseExit()
        {
            TileHoveredOff?.Invoke(this.tile);
        }
    }
}