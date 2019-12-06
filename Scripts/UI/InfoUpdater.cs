//-----------------------------------------------------------------------
// <copyright file="InfoUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using System;
    using Edu.Vfs.RoboRapture.StringBuilders;
    using TMPro;
    using UnityEngine;

    public class InfoUpdater : MonoBehaviour
    {
        [SerializeField]
        private string builderName;

        private TextMeshProUGUI text;

        private void Awake()
        {
            IStringBuilder builder = (IStringBuilder)Activator.CreateInstance(Type.GetType("Edu.Vfs.RoboRapture.StringBuilders." + this.builderName));

            this.text = this.GetComponent<TextMeshProUGUI>();
            this.text.SetText(builder.GetString());
        }
    }
}