//-----------------------------------------------------------------------
// <copyright file="PlayerUnit.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using System;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.UI;
    using Edu.Vfs.RoboRapture.Units.Actions;
    using UnityEngine;

    public class PlayerUnit : Unit
    {
        public static Action<Unit> LeveledUp;

        public static Action<Point, UnitType> PlayerUnitDied;

        [SerializeField]
        private SpawnSystem.UnitType playerUnitType;

        public new void Awake()
        {
            base.Awake();
            SkillDialog.ActionPurchased += LevelUp;
        }

        public void OnDisable()
        {
            SkillDialog.ActionPurchased -= LevelUp;
        }

        private int level = 1;

        public int Level { get => this.level; set => this.level = value; }

        public UnitType PlayerUnitType { get => playerUnitType; private set => playerUnitType = value; }

        private void LevelUp(Unit unit, int actionIndex)
        {
            if (unit.UnitName != this.UnitName)
            {
                return;
            }

            this.level++;
        }

        public void NotifyDyingEvent()
        {
            PlayerUnitDied?.Invoke(this.GetPosition(), playerUnitType);
        }
    }
}