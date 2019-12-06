//-----------------------------------------------------------------------
// <copyright file="PlacementUnitUI.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class PlacementUnitUI : MonoBehaviour
    {
        [SerializeField]
        private Panel textPanel;

        [SerializeField]
        private Image button;

        [SerializeField]
        private Sprite normal;

        [SerializeField]
        private Sprite selected;

        public void SetActive()
        {
            this.textPanel.gameObject.SetActive(true);
            button.sprite = selected;
        }

        public void SetNormal()
        {
            this.textPanel.gameObject.SetActive(false);
            button.sprite = normal;
        }

        public void SetPlaced()
        {
            this.textPanel.gameObject.SetActive(false);
            this.button.enabled = false;
        }
    }
}
