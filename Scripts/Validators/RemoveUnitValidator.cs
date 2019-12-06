//-----------------------------------------------------------------------
// <copyright file="RemoveUnitValidator.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Validators
{
    using Edu.Vfs.RoboRapture.Units;

    public class RemoveUnitValidator : IValidator
    {
        private Unit unit;

        public RemoveUnitValidator(Unit unit)
        {
            this.unit = unit;
        }

        public bool IsValid()
        {
            return this.unit != null;
        }
    }
}