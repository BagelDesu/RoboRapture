//-----------------------------------------------------------------------
// <copyright file="HighlightingTypes.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    public class HighlightingTypes
    {
        private static Dictionary<Options, Color> highlightingTypes;

        static HighlightingTypes()
        {
            highlightingTypes = new Dictionary<Options, Color>()
            {
                { Options.Normal, default },
                { Options.New_Skill_Available, Color.yellow },
                { Options.Selected, Color.blue },
                { Options.Unavailable, Color.gray }
            };
        }

        public enum Options
        {
            Normal,
            New_Skill_Available,
            Selected,
            Unavailable
        }

        public static Color GetColor(Options option)
        {
            return highlightingTypes[option];
        }
    }
}