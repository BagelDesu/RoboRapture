//-----------------------------------------------------------------------
// <copyright file="PhantasmUI.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using UnityEngine;

    public class PhantasmUI : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;

        [SerializeField]
        private RectTransform phantasmImage;

        [SerializeField]
        private Vector3 offset;

        private void Update()
        {
            MoveImage();
        }

        private void MoveImage()
        {
            Vector3 position = Input.mousePosition + offset;
            phantasmImage.position = cam.ScreenToWorldPoint(position);
        }
    }
}