//-----------------------------------------------------------------------
// <copyright file="NeoSatanHeadMovementAction.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units.Actions.Enemy
{
    using System.Collections.Generic;
    using System.Linq;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.ScriptableLibrary;
    using Edu.Vfs.RoboRapture.Units.Enemies;
    using Edu.Vfs.RoboRapture.Validators;
    using UnityEngine;

    public class NeoSatanHeadMovementAction : Action
    {
        [SerializeField]
        private RefInt raptureRow;

        private int offset = 0;

        private System.Random random = new System.Random();

        private NeoSatanBehaviour neoSatanBehaviour;

        private void OnEnable()
        {
            neoSatanBehaviour = GetComponentInParent<NeoSatanBehaviour>();
        }

        public override List<Point> GetValidTargets(List<Point> board, Point point)
        {
            List<Point> boardPoints = base.GetValidTargets(board, point);
            List<Point> excludingLatestRows = boardPoints?.Where(p => p.x > this.raptureRow.Value + this.offset).ToList();
            List<Point> excludingLegs = excludingLatestRows?.Where(p => !neoSatanBehaviour.LegsPositions.Contains(p)).ToList();
            this.ValidPositions = excludingLegs.Where(currentPoint => !this.Unit.UnitsMap.Contains(currentPoint)).ToList();
            return this.ValidPositions;
        }

        public override void Execute(Point target)
        {
            if (this.ValidPositions == null || this.ValidPositions.Count == 0)
            {
                return;
            }

            int randomIndex = this.random.Next(this.ValidPositions.Count);
            Point position = this.ValidPositions[randomIndex]; 
            PlacementHelper.Move(this.Unit, position, new MovementActionValidator());
        }
    }
}
