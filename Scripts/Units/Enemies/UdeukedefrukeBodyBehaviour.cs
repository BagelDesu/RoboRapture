//-----------------------------------------------------------------------
// <copyright file="UdeukedefrukeBodyBehaviour.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Enemies
{
    using UnityEngine;

    public class UdeukedefrukeBodyBehaviour : MonoBehaviour
    {
        private EnemyUnit bodyUnit;

        public void SetUp(Health parentHealth)
        {
            bodyUnit = GetComponent<EnemyUnit>();
            this.bodyUnit.Health.SetMaxHealth(parentHealth.GetTotalHealth());
            this.bodyUnit.Health.SetCurrentHealth(parentHealth.GetCurrentHealth());
            this.bodyUnit.Health = parentHealth;
        }
    }
}
