//-----------------------------------------------------------------------
// <copyright file="IKnockback.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
using Edu.Vfs.RoboRapture.Units;

namespace Edu.Vfs.RoboRapture.Knockbacks
{
    public interface IKnockback
    {
        /// <summary>
        /// Returns true if the target unit can takes this place.
        /// </summary>
        /// <returns>True if the target unit can takes this place, false otherwise.</returns>
        bool Handle(Unit target);
    }
}
