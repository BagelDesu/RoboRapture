//-----------------------------------------------------------------------
// <copyright file="HoloBlastSplit.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Attacks
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Scriptables;
    using UnityEngine;

    public class HoloBlastSplit : MonoBehaviour
    {
        public void SetUp(BoardController boardController, UnitsMap unitsMap, int damage, int knockback, Point origin)
        {
            DamageOnCollision onCollision = this.GetComponent<DamageOnCollision>();
            Logcat.I($"HoloblastSplit damage {damage}, unitsMap {unitsMap}, knockback {knockback} attacker position {origin}");
            onCollision.BoardController = boardController;
            onCollision.UnitsMap = unitsMap;
            onCollision.Damage = damage;
            onCollision.Knockback = knockback;
            onCollision.AttackerPoint = origin;
        }
    }
}