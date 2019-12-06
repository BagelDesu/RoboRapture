//-----------------------------------------------------------------------
// <copyright file="TestamentMovementAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using UnityEngine;

    public class TestamentMovementAction : WhelpMovementAction
    {
        [SerializeField]
        FXWrapper idleFx;

        protected override Point GetTarget()
        {
            idleFx?.Play(this.transform.position);
            return AIUtils.GetWeakestTarget(this.Unit.UnitsMap);
        }
    }
}