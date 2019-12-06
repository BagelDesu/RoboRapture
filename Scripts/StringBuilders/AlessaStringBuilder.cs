//-----------------------------------------------------------------------
// <copyright file="AlessaStringBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.StringBuilders
{
    using System.Text;

    public class AlessaStringBuilder : IStringBuilder
    {
        public string GetString()
        {
            return new StringBuilder()
                .Append("<b>").Append("<color=#00ffffff>ALESSA</color>").Append("</b>").Append("\n")
                .Append("Ranged Unit").Append("\n")
                .Append("<color=green>HP:</color> 5").ToString();
        }
    }
}
