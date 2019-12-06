//-----------------------------------------------------------------------
// <copyright file="BrutusStringBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.StringBuilders
{
    using System.Text;

    public class BrutusStringBuilder : IStringBuilder
    {
        public string GetString()
        {
            return new StringBuilder()
                .Append("<b>").Append("<color=#00ffffff>BRUTUS</color>").Append("</b>").Append("\n")
                .Append("Melee Unit").Append("\n")
                .Append("<color=green>HP:</color> 7").ToString();
        }
    }
}