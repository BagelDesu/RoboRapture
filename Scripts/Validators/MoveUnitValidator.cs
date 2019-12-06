//-----------------------------------------------------------------------
// <copyright file="MoveUnitValidator.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Validators
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Units;

    public class MoveUnitValidator : IValidator
    {
        private Unit unit;

        private Point newPosition;

        public MoveUnitValidator(Unit unit, Point newPosition)
        {
            this.unit = unit;
            this.newPosition = newPosition;
        }

        public bool IsValid()
        {
            return this.unit != null && !this.unit.UnitsMap.Contains(this.newPosition);
        }
    }
}
