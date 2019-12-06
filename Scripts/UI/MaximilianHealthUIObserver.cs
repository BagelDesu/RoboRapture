//-----------------------------------------------------------------------
// <copyright file="MaximilianHealthUIObserver.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class MaximilianHealthUIObserver : MonoBehaviour
    {
        [SerializeField]
        private ChicletsUI chiclets;

        public void SetUp(Health health)
        {
            chiclets.SetUp(health);
        }

        public void UpdateHealth(Health health)
        {
            chiclets?.UpdateHealth(health);
        }
    }
}
