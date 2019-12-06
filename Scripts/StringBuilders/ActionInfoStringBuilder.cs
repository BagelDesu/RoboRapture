//-----------------------------------------------------------------------
// <copyright file="ActionInfoStringBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.StringBuilders
{
    using Edu.Vfs.RoboRapture.Units.Actions;
    using System.Text;

    public class ActionInfoStringBuilder : IStringBuilder
    {
        private Action action;

        public ActionInfoStringBuilder(Action action)
        {
            this.action = action;
        }

        public string GetString()
        {
            if (action == null)
            {
                return string.Empty;
            }

            StringBuilder info = new StringBuilder();
            info.Append("<b><size=45>").Append(action.SkillName).Append("</size></b>").Append("\n");
            info.Append(action.Description).Append("\n");
            info.Append("<size=30>Range: ").Append(action.Range).Append(" tile(s)\n");

            if (action is SkillAction)
            {
                SkillAction skillAction = (SkillAction)action;
                info.Append(skillAction.DamageDescription).Append("\n");
                info.Append("Knockback: ").Append(skillAction.KnockbackDescription).Append("\n");
                info.Append("Cooldown: ").Append(skillAction.CoolDown).Append(" turn(s) \n");
            }

            if (!action.IsUnlocked())
            {
                info.Append("COST: ").Append(action.ExperiencePointsToUnlocked).Append(" Kills");
            }
            info.Append("</size>");

            return info.ToString();
        }
    }
}
