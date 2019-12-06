//-----------------------------------------------------------------------
// <copyright file="HealthBarDisplayer.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Controllers;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class HealthBarDisplayer : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private GameObject shadow;

        [SerializeField]
        private float delay = 0f;

        private bool show;

        private Unit unit;

        public void OnEnable()
        {
            unit = GetComponentInParent<Unit>();
            Health health = unit?.Health;
            GetComponent<ChicletsUI>()?.SetUp(health);
            PlayerController.PlayerActionExecuted += OnPlayerActionExecuted;
        }

        private void OnDisable()
        {
            PlayerController.PlayerActionExecuted -= OnPlayerActionExecuted;
        }

        public void Show(Health health)
        {
            show = !health.IsDead();
            Invoke("Delay", delay);
        }

        public void Show(bool show)
        {
            this.canvas.enabled = show;
        }

        public void OnItemSelected(RefPoint point)
        {
            if (canvas != null)
            {
                this.canvas.enabled = this.unit.GetPosition() == point.Value;
            }
        }

        private void OnPlayerActionExecuted()
        {
            Logcat.I(this, "Hidding bar when player action executed");
            if (canvas != null)
            {
                this.canvas.enabled = false;
            }
        }

        private void Delay()
        {
            if (canvas != null)
            {
                this.canvas.enabled = show;
            }

            if (shadow != null)
            {
                this.shadow.SetActive(show);
            }
        }
    }
}
