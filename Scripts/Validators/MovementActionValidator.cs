//-----------------------------------------------------------------------
// <copyright file="MovementActionValidator.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Validators
{
    public class MovementActionValidator : IValidator
    {
        /// <summary>
        /// Validation made on selection stage, at this point, the player has selected a valid position
        /// </summary>
        /// <returns>true</returns>
        public bool IsValid()
        {
            return true;
        }
    }
}
