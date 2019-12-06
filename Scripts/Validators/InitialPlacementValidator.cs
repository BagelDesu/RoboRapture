//-----------------------------------------------------------------------
// <copyright file="InitialPlacementValidator.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.Helpers;
using Edu.Vfs.RoboRapture.Scriptables;

namespace Edu.Vfs.RoboRapture.Validators
{
    public class InitialPlacementValidator : IValidator
    {
        private UnitsMap map;

        private Point point;

        public InitialPlacementValidator(UnitsMap map, Point point)
        {
            this.map = map;
            this.point = point;
        }

        public bool IsValid()
        {
            return point.x > 1 && point.x < 4 && point.z >= 0 && !map.Contains(point);
        }
    }
}