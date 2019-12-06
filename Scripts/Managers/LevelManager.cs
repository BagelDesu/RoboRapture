//-----------------------------------------------------------------------
// <copyright file="LevelManager.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Managers 
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.TerrainSystem;
    using NaughtyAttributes;
    using UnityEngine;

    public class LevelManager : MonoBehaviour
    {
       [SerializeField]
        private UnitsMap unitsMap;

        [Required]
        [SerializeField]
        private BoardController boardMap;

        public UnitsMap UnitsMap
        {
            get => this.unitsMap;
            private set => this.unitsMap = value;
        }

        public BoardController BoardController { get => this.boardMap; set => this.boardMap = value; }

        public List<Point> GetBoard(TerrainNavigationType navigationType)
        {
            return this.boardMap.GetAllPointsWithNavigationTypeOf(navigationType);
        }
    }
}