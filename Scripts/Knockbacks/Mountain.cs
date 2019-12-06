//-----------------------------------------------------------------------
// <copyright file="Mountain.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Knockbacks
{
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class Mountain : MonoBehaviour, IKnockback
    {
        [SerializeField]
        private int damageOnCollision = 1;

        public bool Handle(Unit target)
        {
            Unit unit = this.GetComponent<Unit>();
            target?.Health.ReduceHealth(this.damageOnCollision);
            unit?.Health.DeadByOneHit();
            return true;
        }
    }
}