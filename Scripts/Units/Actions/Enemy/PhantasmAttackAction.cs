//-----------------------------------------------------------------------
// <copyright file="PhantasmAttackAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.SpawnSystem;

    public class PhantasmAttackAction : SkillAction
    {
        public override void Execute()
        {
            Execute(PointConverter.ToPoint(this.transform.position));
            SpawnManager.Instance?.PrepareTestament(this.Unit.GetPosition());
        }
    }
}