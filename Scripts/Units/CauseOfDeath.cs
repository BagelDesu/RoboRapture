//-----------------------------------------------------------------------
// <copyright file="CauseOfDeath.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{ 
    public enum CauseOfDeath
    {
        /// <summary>
        /// Dead by combat.
        /// </summary>
        HealthReachesZero,
        /// <summary>
        /// Applicable for environment elements.
        /// </summary>
        OneHit,
        /// <summary>
        /// Unit is reached by the Rapture.
        /// </summary>
        Rapture,

        /// <summary>
        /// Not applicable.
        /// </summary>
        Roboenza,
        Dehidration,
        Hypotermia,
        BurnToDeath,
        Malaria
    }
}