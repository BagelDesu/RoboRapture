//-----------------------------------------------------------------------
// <copyright file="UnitKnockback.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Knockbacks
{
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class UnitKnockback : MonoBehaviour, IKnockback
    {
        [SerializeField]
        private int damageOnCollision = 1;

        public bool Handle(Unit target)
        {
            Unit unit = this.GetComponent<Unit>();
            unit?.Health.ReduceHealth(this.damageOnCollision);
            target?.Health.ReduceHealth(this.damageOnCollision);
            return false;
        }
    }
}