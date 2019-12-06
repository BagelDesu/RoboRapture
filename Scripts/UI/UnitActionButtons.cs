//-----------------------------------------------------------------------
// <copyright file="UnitActionButtons.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Helpers;
    using UnityEngine;

    public class UnitActionButtons : MonoBehaviour
    {
        [SerializeField]
        private ChicletUI movementButton;

        [SerializeField]
        private ChicletUI actionButton;

        public void ActivateButton(Units.Actions.ActionType actionType, bool enable)
        {
            switch (actionType) 
            {
                case Units.Actions.ActionType.Movement:
                    SwitchState(movementButton, enable);
                    break;
                case Units.Actions.ActionType.Action:
                    SwitchState(actionButton, enable);
                    break;
                default:
                    break;
            }
        }

        private void SwitchState(ChicletUI button, bool enable)
        {
            Logcat.I(this, $"SwitchState {enable}");
            if (enable)
            {
                button.SetOn();
            }
            else
            {
                button.SetOff();
            }
        }
    }
}