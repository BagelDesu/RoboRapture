//-----------------------------------------------------------------------
// <copyright file="ISelectable.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Base 
{
    using Edu.Vfs.RoboRapture.DataTypes;
    
    /// <summary>
    /// Labels the object as selectable.
    /// </summary>
    public interface ISelectable
    {
        Point GetPosition();
    }
}