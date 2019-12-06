//-----------------------------------------------------------------------
// <copyright file="MessageUI.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using TMPro;
    using UnityEngine;

    public class MessageUI : MonoBehaviour
    {
        [SerializeField]
        private Panel blocker;

        [SerializeField]
        private TextMeshProUGUI message;

        private string placementPhaseText = "PLACE YOUR UNITS";

        private string gameplayPhaseText = "THE RAPTURE IS COMING";

        public enum PhaseText
        {
            Placement,
            Gameplay
        }

        public void Activate(bool show)
        {
            this.blocker.gameObject.SetActive(show);
        }

        public void SetText(PhaseText phaseText)
        {
            this.blocker.gameObject.SetActive(true);
            this.message.text = phaseText == PhaseText.Placement ? this.placementPhaseText : this.gameplayPhaseText;
        }
    }
}