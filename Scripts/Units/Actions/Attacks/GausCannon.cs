//-----------------------------------------------------------------------
// <copyright file="GausCannon.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Attacks
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Scriptables;
    using UnityEngine;

    public class GausCannon : MonoBehaviour
    {
        public void SetUp(UnitsMap unitsMap, BoardController boardController, Point attackerPosition, int knockback, int damage)
        {
            DamageOnCollision bullet = this.GetComponent<DamageOnCollision>();
            bullet.Damage = damage;
            bullet.UnitsMap = unitsMap;
            bullet.BoardController = boardController;
            bullet.AttackerPoint = attackerPosition;
            bullet.Knockback = knockback;
            bullet.DestroyComponent = true;
        }
    }
}
