//-----------------------------------------------------------------------
// <copyright file="IPattern.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Patterns
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;

    public interface IPattern
    {
        List<Point> GetPattern();
    }
}
