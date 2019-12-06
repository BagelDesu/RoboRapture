//-----------------------------------------------------------------------
// <copyright file="PlayerInfoStringBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.StringBuilders
{
    using System.Text;
    using Edu.Vfs.RoboRapture.Units;

    public class PlayerInfoStringBuilder : IStringBuilder
    {
        private PlayerUnit playerUnit;

        public PlayerInfoStringBuilder(PlayerUnit playerUnit)
        {
            this.playerUnit = playerUnit;
        }

        public string GetString()
        {
            StringBuilder info = new StringBuilder();
            info.Append("<b>").Append(playerUnit.UnitName).Append("</b>\n")
                .Append("Level: ").Append(playerUnit.Level).Append("\n")
                .Append("HP: ").Append(playerUnit.Health.GetCurrentHealth()).Append("/").Append(playerUnit.Health.GetTotalHealth());

            return info.ToString();
        }
    }
}