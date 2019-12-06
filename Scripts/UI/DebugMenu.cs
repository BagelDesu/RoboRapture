//-----------------------------------------------------------------------
// <copyright file="DebugMenu.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{ 
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.Scriptables;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class DebugMenu : MonoBehaviour
    {
        [SerializeField]
        private UnitsMap unitsMap;

        [SerializeField]
        private Panel panel;

        [SerializeField]
        private ScriptobjectCacheCleaner cleaner;

        private string escapeKey = "escape";

        public void Update()
        {
            if (Input.GetKeyDown(this.escapeKey) && !this.IsDebugMenuVisible())
            {
                this.ShowDebugMenu(true);
            }
            else if (Input.GetKeyDown(this.escapeKey) && this.IsDebugMenuVisible())
            {
                this.ShowDebugMenu(false);
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(1);
            this.cleaner.CleanUp();
        }

        public void HealthPlayerUnits()
        {
            this.HealthUnits(Type.Player);
            this.ShowDebugMenu(false);
        }

        public void HealthEnemyUnits()
        {
            this.HealthUnits(Type.Enemy);
            this.ShowDebugMenu(false);
        }

        private bool IsDebugMenuVisible()
        {
            return this.panel.gameObject.activeInHierarchy;
        }

        private void ShowDebugMenu(bool active)
        {
            this.panel.gameObject.SetActive(active);
        }

        private void HealthUnits(Type type)
        {
            List<Unit> units = this.unitsMap.GetUnits(type);
            foreach (Unit item in units)
            {
                item.Health.Restore();
            }
        }
    }
}