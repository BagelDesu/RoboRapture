//-----------------------------------------------------------------------
// <copyright file="UnitsInfoUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.SpawnSystem;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class UnitsInfoUpdater : MonoBehaviour
    {
        [SerializeField]
        private UnitsMap map;

        private UnitInfo[] unitsInfo;

        private void Awake()
        {
            this.unitsInfo = GetComponentsInChildren<UnitInfo>(true);
            PlayerController.UnitsLoaded += this.Initialize;
            PlayerController.UnitSelected += this.HighlightSelectedUnit;
            PlayerController.PlayerTurnEnded += OnActionCancelled;
            Selector.CancelledAction += OnActionCancelled;
            SkillStoreController.ActionAvailable += this.HighlightUnit;
            SkillDialog.ActionPurchased += this.UnhighlightUnit;
        }

        private void OnDisable()
        {
            PlayerController.UnitsLoaded -= this.Initialize;
            PlayerController.UnitSelected -= this.HighlightSelectedUnit;
            PlayerController.PlayerTurnEnded -= OnActionCancelled;
            Selector.CancelledAction -= OnActionCancelled;
            SkillStoreController.ActionAvailable -= this.HighlightUnit;
            SkillDialog.ActionPurchased -= this.UnhighlightUnit; 
        }

        public void Initialize(List<Unit> units)
        {
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                for (int j = 0; j < unitsInfo.Length; j++)
                {
                    if (unit.GetSpawnedUnitType() == unitsInfo[j].UnitType)
                    {
                        this.Init(j, unit.Health);
                    }
                }
            }
        }

        private void Init(int index, Health health)
        {
            this.unitsInfo[index].gameObject.SetActive(true);
            this.unitsInfo[index].Init(health);
        }

        public void HighlightSelectedUnit(Unit unit)
        {
            if (this.unitsInfo == null)
            {
                return;
            }

            if (unit == null)
            {
                OnActionCancelled();
                return;
            }

            Logcat.I(this, $"HighlightSelectedUnit {unit?.GetSpawnedUnitType()}");
            for (int i = 0; i < this.unitsInfo.Length; i++)
            {
                this.unitsInfo[i].Highlight(this.unitsInfo[i].UnitType == unit.GetSpawnedUnitType()
                    ? HighlightingTypes.Options.Selected : HighlightingTypes.Options.Normal);
            }
        }

        public void HighlightUnit(Unit unit, int index)
        {
            for (int i = 0; i < this.unitsInfo.Length; i++)
            {
                this.unitsInfo[i].Highlight(this.unitsInfo[i].UnitType == unit.GetSpawnedUnitType()
                    ? HighlightingTypes.Options.New_Skill_Available : HighlightingTypes.Options.Normal);
            }
        }

        public void UnhighlightUnit(Unit unit, int index)
        {
            for (int i = 0; i < this.unitsInfo.Length; i++)
            {
                if (this.unitsInfo[i].UnitType == unit.GetSpawnedUnitType())
                {
                    this.unitsInfo[i].Highlight(HighlightingTypes.Options.Normal);
                }
            }
        }

        public void ItemSelected(RefPoint point)
        {
            Unit unit = map.Get(point.Value);
            this.HighlightSelectedUnit(unit);
        }

        public void UpdateTurnsToRespawn(UnitType unit, int turnsToRespawn)
        {
            foreach (var item in this.unitsInfo)
            {
                if (item.UnitType == unit)
                {
                    item.UpdateTurnsToRespawn(turnsToRespawn);
                }
            }
        }

        public void UpdateActionButtons(UnitType unit, bool enable)
        {
            foreach (var item in this.unitsInfo)
            {
                if (item.UnitType == unit)
                {
                    item.EnableActionButtons(enable);
                }
            }
        }

        private void OnActionCancelled()
        {
            for (int i = 0; i < unitsInfo.Length; i++)
            {
                this.unitsInfo[i].Highlight(HighlightingTypes.Options.Normal);
            }
        }
    }
}
