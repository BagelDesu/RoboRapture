//-----------------------------------------------------------------------
// <copyright file="DestroyableBlockersStringBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.StringBuilders
{
    using System.Text;

    public class DestroyableBlockersStringBuilder : IStringBuilder
    {
        private DestroyableBlockers blocker;

        public DestroyableBlockersStringBuilder(DestroyableBlockers blocker)
        {
            this.blocker = blocker;
        }

        public string GetString()
        {
            StringBuilder info = new StringBuilder();
            info.Append("<b><size=45>").Append(blocker.UnitName).Append("</color></size></b>\n\n")
            .Append("<size=30>").Append(blocker.Description).Append("\n");

            return info.ToString();
        }
    }
}