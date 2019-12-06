//-----------------------------------------------------------------------
// <copyright file="BrutusThirdAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Player
{
    using Edu.Vfs.RoboRapture.DataTypes;

    public class BrutusThirdAction : WhirlwindAction
    {
        protected override void AttackPositions(Point point)
        {
            base.AttackPositions(point);
            if (!UnitsMap.Contains(point))
            {
                return;
            }

            SkillActionFX?.Play(UnitsMap.Get(point).gameObject.transform.position);
        }
    }
}