//-----------------------------------------------------------------------
// <copyright file="NeoSatanLegAnimationStateUpdater.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    public class NeoSatanLegAnimationStateUpdater : AnimationStateUpdater
    {
        private string rising = "Rising";

        private string falling = "Falling";

        public void Fall()
        {
            this.animator.SetTrigger(this.falling);
        }

        public void Rise()
        {
            this.animator.SetTrigger(this.rising);
        }
    }
}