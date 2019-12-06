//-----------------------------------------------------------------------
// <copyright file="ActionsHandler.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using NaughtyAttributes;
    using UnityEngine;

    public class ActionsHandler : MonoBehaviour, IAction
    {
        [ReorderableList]
        [SerializeField]
        private List<Action> actions;

        [SerializeField]
        private RefInt actionsIndex;

        private bool wasActionExecutedInTheTurn;

        public RefInt ActionsIndex { get => this.actionsIndex; private set => this.actionsIndex = value; }

        public bool WasActionExecutedInTheTurn { get => wasActionExecutedInTheTurn; set => wasActionExecutedInTheTurn = value; }

        private int currentActionIndex;

        public void Awake()
        {
            this.actionsIndex.Value = -1;
        }

        public void OnDisable()
        {
            this.actionsIndex.Value = -1;
        }

        public int GetRange()
        {
            return this.actions[0].Range;
        }

        public void ActivateAction(int index)
        {
            if (this.actionsIndex.Value == index)
            {
                return;
            }

            this.actionsIndex.Value = index;
        }

        public bool IsActive()
        {
            if (this.actionsIndex.Value < 0)
            {
                return false;
            }

            return this.actions[this.actionsIndex.Value].IsActive();
        }

        public void IsActive(bool isActive)
        {
            this.actions[this.actionsIndex.Value].IsActive(isActive);
        }

        public List<Point> GetValidTargets(List<Point> board, Point point)
        {
            if (this.actionsIndex.Value < 0)
            {
                return null;
            }

            return this.actions[this.actionsIndex.Value].GetValidTargets(board, point);
        }

        public bool ValidateAction(Point target)
        {
            return this.actions[this.actionsIndex.Value].ValidateAction(target);
        }

        public void Execute(Point target)
        {
            this.currentActionIndex = this.actionsIndex.Value;
            this.actions[this.actionsIndex.Value].Execute(target);
            this.actions[this.actionsIndex.Value].ResetCoolDown();
        }

        public bool IsEnabled()
        {
            if (this.actionsIndex.Value < 0)
            {
                return false;
            }

            return this.actions[this.actionsIndex.Value].IsEnabled();
        }

        public void IsEnabled(bool isEnabled)
        {
            this.actions[this.actionsIndex.Value].IsEnabled(isEnabled);
        }

        public bool IsUnlocked()
        {
            return this.actions[this.actionsIndex.Value].IsUnlocked();
        }

        public void EnableActions()
        {
            foreach (var item in this.actions)
            {
                if (!item.IsUnlocked())
                {
                    return;
                }

                item.IsEnabled(true);
            }
        }

        public List<Action> GetActions()
        {
            return this.actions;
        }

        public bool AreAvailableActions()
        {
            foreach (var item in actions)
            {
                if (item.IsEnabled() && item.IsUnlocked() && item.IsReadyToUse())
                {
                    return true;
                }
            }
            return false;
        }

        public void RefreshReactivationTime()
        {
            foreach (var item in this.actions)
            {
                if (!item.IsUnlocked())
                {
                    return;
                }

                item.RefreshReactivationTime();
            }
        }

        public void Execute()
        {
            Logcat.I(this, $"Execute skill {this.currentActionIndex}");
            if (this.actions[this.currentActionIndex] is SkillAction)
            {
                ((SkillAction)this.actions[this.currentActionIndex])?.Execute();
                this.WasActionExecutedInTheTurn = true;
            }
        }
    }
}
