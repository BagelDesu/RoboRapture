//-----------------------------------------------------------------------
// <copyright file="NeoSatanLegBehaviour.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;

    public class NeoSatanLegBehaviour : Behaviour
    {
        private Unit unit;

        public void Awake()
        {
            this.unit = this.GetComponent<Unit>();
        }

        public override IEnumerator Execute(BoardController boardController, List<Point> board)
        {
            yield return null;
        }

        public void DestroyLeg()
        {
            this.unit?.Health.DeadByOneHit();
            this.GetComponentInChildren<AnimationStateUpdater>()?.RemoveFromUnitsMap();
            this.transform.gameObject.SetActive(false);
        }
    }
}