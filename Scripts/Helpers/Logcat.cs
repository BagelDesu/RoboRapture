//-----------------------------------------------------------------------
// <copyright file="Logcat.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Logcat
    {
        public static bool Debugging = true;

        private static Dictionary<Level, Color> colors;

        static Logcat()
        {
            colors = new Dictionary<Level, Color>()
            {
                { Level.Error, Color.red },
                { Level.Warning, Color.yellow },
                { Level.Info, Color.white },
                { Level.Debug, Color.cyan },
                { Level.Verboise, Color.green },
            };
        }

        private enum Level
        {
            Error,
            Warning,
            Info,
            Debug,
            Verboise
        }

        public static void E(Object tag, string message)
        {
            ShowLog(Level.Error, tag, message);
        }

        public static void E(string message)
        {
            ShowLog(Level.Error, null, message);
        }

        public static void W(Object tag, string message)
        {
            ShowLog(Level.Warning, tag, message);
        }

        public static void W(string message)
        {
            ShowLog(Level.Warning, null, message);
        }

        public static void I(Object tag, string message)
        {
            ShowLog(Level.Info, tag, message);
        }

        public static void I(string message)
        {
            ShowLog(Level.Info, null, message);
        }

        public static void D(Object tag, string message)
        {
            ShowLog(Level.Debug, tag, message);
        }

        public static void D(string message)
        {
            ShowLog(Level.Debug, null, message);
        }

        public static void V(Object tag, string message)
        {
            ShowLog(Level.Verboise, tag, message);
        }

        public static void V(string message)
        {
            ShowLog(Level.Verboise, null, message);
        }

        private static void ShowLog(Level level, Object tag, string message)
        {
            if (!Debugging)
            {
                return;
            }

            if (level == Level.Info || level == Level.Debug)
            {
                return;
            }

            Debug.Log(SetFormat(level, tag, message));
        }

        private static string SetFormat(Level level, Object tag, string message)
        {
            Color color = colors[level];
            return string.Format(
                "<color=#{0:X2}{1:X2}{2:X2}>{3} {4} - {5}</color>", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), level, tag == null ? string.Empty : tag.ToString(), message);
        }
    }
}