//-----------------------------------------------------------------------
// <copyright file="CityRuins.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Knockbacks
{
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class CityRuins : MonoBehaviour, IKnockback
    {
        public bool Handle(Unit target)
        {
            return false;
        }
    }
}