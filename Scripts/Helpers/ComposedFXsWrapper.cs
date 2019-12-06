//-----------------------------------------------------------------------
// <copyright file="ComposedFXsWrapper.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using System.Linq;
    using UnityEngine;

    public class ComposedFXsWrapper : FXWrapper
    {
        [SerializeField]
        private FXWrapper[] fxWrappers;

        public override void Play(Vector3 position)
        {
            this.fxWrappers?.ToList().ForEach(fx => fx?.Play(position));
        }

        public new bool ShowAttributes()
        {
            return false;
        }
    }
}