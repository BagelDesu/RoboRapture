//-----------------------------------------------------------------------
// <copyright file="TileStringBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.StringBuilders
{
    using System.Text;

    public class TileStringBuilder : IStringBuilder
    {
        private Tile tile;

        public TileStringBuilder(Tile tile)
        {
            this.tile = tile;
        }

        public string GetString()
        {
            StringBuilder info = new StringBuilder();
            info.Append("<b><size=45>").Append(tile.TileName).Append("</size></b>\n\n")
                .Append("<size=30>").Append(tile.Description).Append("</size>");
            return info.ToString();
        }
    }
}