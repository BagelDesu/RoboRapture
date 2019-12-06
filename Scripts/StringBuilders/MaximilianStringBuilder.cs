//-----------------------------------------------------------------------
// <copyright file="MaximilianStringBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.StringBuilders
{
    using System.Text;

    public class MaximilianStringBuilder : IStringBuilder
    {
        public string GetString()
        {
            return new StringBuilder()
                .Append("<b>").Append("<color=#00ffffff>MAXIMILIAN</color>").Append("</b>").Append("\n")
                .Append("Support Unit").Append("\n")
                .Append("<color=green>HP:</color> 6").ToString();
        }
    }
}