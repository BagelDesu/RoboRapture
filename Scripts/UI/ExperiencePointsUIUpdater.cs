//-----------------------------------------------------------------------
// <copyright file="ExperiencePointsUIUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Helpers;
    using TMPro;
    using UnityEngine;

    public class ExperiencePointsUIUpdater : MonoBehaviour
    {
        private TextMeshProUGUI text;

        [SerializeField]
        private int delay = 3;

        [SerializeField]
        private FXWrapper audioFx;

        private int xp;

        public void Start()
        {
            this.text = this.GetComponent<TextMeshProUGUI>();
            ExperiencePointsUpdater.ExperiencePointsUpdated += this.UpdateUI;
        }

        public void OnDisable()
        {
            ExperiencePointsUpdater.ExperiencePointsUpdated -= this.UpdateUI;
        }

        private void UpdateUI(int xp)
        {
            this.xp = xp;
            Invoke("UpdatePoints", this.delay);
        }

        private void UpdatePoints()
        {
            this.text.text = xp.ToString();
            audioFx?.Play(this.transform.position);
        }

    }
}