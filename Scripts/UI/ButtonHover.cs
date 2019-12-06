//-----------------------------------------------------------------------
// <copyright file="ButtonHover.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using Edu.Vfs.RoboRapture.Helpers;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static Action<bool, int> ButtonHovered;

        [SerializeField]
        private int index;

        [SerializeField]
        private FXWrapper buttonHoveredFx;

        public void OnPointerEnter(PointerEventData eventData)
        {
            ButtonHovered?.Invoke(true, index);
            buttonHoveredFx?.Play(this.gameObject.transform.position);
            Logcat.I(this, $"Button hover on");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ButtonHovered?.Invoke(false, index);
            Logcat.I(this, $"Button hover off");
        }
    }
}