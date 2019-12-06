//-----------------------------------------------------------------------
// <copyright file="RotationHelper.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using Edu.Vfs.RoboRapture.Patterns;
    using UnityEngine;

    public class RotationHelper
    {
        public static Quaternion GetRotation(CardinalDirections direction)
        {
            switch (direction)
            {
                case CardinalDirections.North:
                    return Quaternion.Euler(0f, 0f, 0f);
                case CardinalDirections.East:
                    return Quaternion.Euler(0f, 90f, 0f);
                case CardinalDirections.West:
                    return Quaternion.Euler(0f, 270f, 0f);
                case CardinalDirections.South:
                default:
                    return Quaternion.Euler(0f, 180f, 0f);
            }
        }

        public static Quaternion GetRotation(CardinalDirections direction, float xRotation, float zRotation)
        {
            switch (direction)
            {
                case CardinalDirections.North:
                    return Quaternion.Euler(xRotation, 0f, zRotation);
                case CardinalDirections.NorthEast:
                    return Quaternion.Euler(xRotation, 45f, zRotation);
                case CardinalDirections.NorthWest:
                    return Quaternion.Euler(xRotation, 315f, zRotation);
                case CardinalDirections.East:
                    return Quaternion.Euler(xRotation, 90f, zRotation);
                case CardinalDirections.SouthEast:
                    return Quaternion.Euler(xRotation, 135f, zRotation);
                case CardinalDirections.SouthWest:
                    return Quaternion.Euler(xRotation, 225f, zRotation);
                case CardinalDirections.West:
                    return Quaternion.Euler(xRotation, 270f, zRotation);
                case CardinalDirections.South:
                default:
                    return Quaternion.Euler(xRotation, 180f, zRotation);
            }
        }
    }
}
