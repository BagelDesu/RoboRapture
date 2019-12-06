//-----------------------------------------------------------------------
// <copyright file="ChicletsUI.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class ChicletsUI : MonoBehaviour
    {
        private ChicletUI[] chiclets;
        
        private void Awake()
        {
            chiclets = GetComponentsInChildren<ChicletUI>();
        }

        public void SetUp(Health health)
        {
            SetMaxChiclets(health);
        }

        private void SetMaxChiclets(Health health)
        {
            Logcat.I(this, $"Set max chiclets {health?.GetTotalHealth()}");
            for (int i = 0; i < chiclets.Length; i++)
            {
                chiclets[i].gameObject.SetActive(i < health.GetTotalHealth());
            }
        }

        public void UpdateHealth(Health health)
        {
            if (chiclets == null)
            {
                return;
            }

            Logcat.I(this, $"Updating health {health.GetCurrentHealth()}");
            SetMaxChiclets(health);
            for (int i = 0; i < chiclets.Length; i++)
            {
                if (i < health.GetCurrentHealth())
                {
                    chiclets[i].SetOn();
                }
                else
                {
                    chiclets[i].SetOff();
                }
            }
        }

        public void SimulateAttack(Health health, int Delta, bool isSustractive)
        {
            if (chiclets == null)
            {
                return;
            }

            //// Logcat.W(this, $"Simulating attack with delta {Delta}, is sustractive? {isSustractive}");
            UpdateHealth(health);
            int j = isSustractive ? (int) health.GetCurrentHealth() - 1 : (int) health.GetCurrentHealth();
            for (int i = 0; i < Delta; i++)
            {
                if (isSustractive)
                {
                    //// Logcat.W(this, $"Sustraction, updating chicklet {j - i}, of {chiclets.Length}. Chiclet selected {j}");
                    if ((j - i) >= 0)
                    {
                        chiclets[j - i].SetNeutral();
                    }
                }
                else
                {
                    //// Logcat.W(this, $"Addition, updating chicklet {j + i}, of {chiclets.Length}. Chiclet selected {j}");
                    if ((j + i) < chiclets.Length)
                    {
                        chiclets[j + i].SetNeutral();
                    }
                }
            }
        }
    }
}