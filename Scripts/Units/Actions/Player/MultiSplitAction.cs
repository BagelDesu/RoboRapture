//-----------------------------------------------------------------------
// <copyright file="MultiSplitAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Player
{
    using Edu.Vfs.RoboRapture.Units.Actions.Attacks;

    public class MultiSplitAction : HoloBlastAction
    {
        public override void Execute()
        {
            MultiSplit.InstantiatedSplits = 0;
            base.Execute();
        }
    }
}