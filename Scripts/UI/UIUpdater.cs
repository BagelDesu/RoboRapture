//-----------------------------------------------------------------------
// <copyright file="UIUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.Units;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class UIUpdater : MonoBehaviour
    {
        [SerializeField]
        private UnitsMap unitsMap;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Panel panel;

        [SerializeField]
        private TextMeshProUGUI info;

        protected UnitsMap UnitsMap { get => this.unitsMap; private set => this.unitsMap = value; }

        protected Image Image { get => this.image; private set => this.image = value; }

        protected Panel Panel { get => this.panel; private set => this.panel = value; }

        protected TextMeshProUGUI Info { get => this.info; private set => this.info = value; }

        public void UpdateUI(Point point, Type type)
        {
            bool isValidUnit = this.unitsMap.Contains(point, type);
            this.EnableUI(isValidUnit);
            if (isValidUnit)
            {
                this.UpdateInfo(this.unitsMap.Get(point));
            }
        }

        protected abstract void UpdateInfo(Unit unit);

        protected void EnableUI(bool enable)
        {
            this.image.enabled = enable;
            this.panel.gameObject.SetActive(enable);
            this.info.enabled = enable;
        }
    }
}
