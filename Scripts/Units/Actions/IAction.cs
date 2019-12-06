//-----------------------------------------------------------------------
// <copyright file="IAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;

    public interface IAction
    {
        bool IsActive();

        void IsActive(bool isActive);

        bool IsEnabled();

        void IsEnabled(bool isEnabled);

        bool IsUnlocked();
        
        List<Point> GetValidTargets(List<Point> board, Point point);

        bool ValidateAction(Point target);

        void Execute(Point target);
    }
}
