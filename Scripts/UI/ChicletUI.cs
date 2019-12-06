//-----------------------------------------------------------------------
// <copyright file="ChicletUI.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ChicletUI : MonoBehaviour
    {
        [SerializeField]
        private Sprite chicletOn;

        [SerializeField]
        private Sprite chicletOff;

        [SerializeField]
        private Sprite chicletNeutral;

        [SerializeField]
        private Material normalMaterial;

        [SerializeField]
        private Material pulsingMaterial;

        private Image image;

        private void OnEnable()
        {
            this.image = this.GetComponent<Image>();
        }

        public void SetOn()
        {
            if (this.image == null)
            {
                return;
            }

            this.image.sprite = chicletOn;
            this.image.material = normalMaterial;
        }

        public void SetOff()
        {
            if (this.image == null)
            {
                return;
            }

            this.image.sprite = chicletOff;
            this.image.material = normalMaterial;
        }

        public void SetNeutral()
        {
            if (this.image == null)
            {
                return;
            }

            this.image.sprite = chicletNeutral;
            this.image.material = pulsingMaterial;
        }
    }
}