//-----------------------------------------------------------------------
// <copyright file="EnemyUnitUIUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.StringBuilders;
    using Edu.Vfs.RoboRapture.Units;

    public class EnemyUnitUIUpdater : UIUpdater
    {
        private bool isPlayerUnitSelected;

        public void OnEnable()
        {
            SelectableHovered.UnitHoveredOn += UpdateUI;
            SelectableHovered.UnitHoveredOff += HideInfo;
            PlayerController.UnitSelected += this.OnUnitSelected;
            EnableUI(false);
        }

        public void OnDisable()
        {
            SelectableHovered.UnitHoveredOn -= UpdateUI;
            SelectableHovered.UnitHoveredOff -= HideInfo;
            PlayerController.UnitSelected -= this.OnUnitSelected;
        }

        private void UpdateUI(Unit unit)
        {
            if (this.isPlayerUnitSelected)
            {
                HideInfo(unit);
                return;
            }

            base.UpdateUI(unit.GetPosition(), Type.Enemy);
        }

        protected override void UpdateInfo(Unit unit)
        {
            EnemyUnit enemyUnit = (EnemyUnit)unit;
            this.Image.preserveAspect = true;
            this.Image.sprite = enemyUnit.Picture;
            this.Info.text = this.BuildInfo(enemyUnit);
        }

        private void HideInfo(Unit unit)
        {
            this.EnableUI(false);
        }

        private string BuildInfo(EnemyUnit enemyUnit)
        {
            IStringBuilder enemyInfo = new EnemyInfoStringBuilder(enemyUnit);
            return enemyInfo.GetString();
        }

        private void OnUnitSelected(Unit playerUnit)
        {
            this.isPlayerUnitSelected = playerUnit != null;
            Logcat.I(this, $"Is player unit selected {this.isPlayerUnitSelected}");
        }
    }
}