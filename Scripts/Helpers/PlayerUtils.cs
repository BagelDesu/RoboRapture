//-----------------------------------------------------------------------
// <copyright file="PlayerUtils.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using Edu.Vfs.RoboRapture.Patterns;

    public class PlayerUtils
    {
        public static CardinalDirections[] HoloBlastSplitDirections(CardinalDirections direction)
        {
            CardinalDirections[] directions = new CardinalDirections[2];
            switch (direction)
            {
                case CardinalDirections.East:
                case CardinalDirections.West:
                    directions[0] = CardinalDirections.North;
                    directions[1] = CardinalDirections.South;
                    break;
                case CardinalDirections.North:
                case CardinalDirections.South:
                default:
                    directions[1] = CardinalDirections.East;
                    directions[0] = CardinalDirections.West;
                    break;
            }

            return directions;
        }
    }
}