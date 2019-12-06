//-----------------------------------------------------------------------
// <copyright file="PhantasmMovementAction.cs" company="VFS">
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
    using Edu.Vfs.RoboRapture.Validators;
    using UnityEngine;

    public class PhantasmMovementAction : Action
    {
        [SerializeField]
        private RefInt raptureRow;

        [SerializeField]
        private FXWrapper phantasmSummon;

        private int offset = 3;

        private System.Random random = new System.Random();

        public override List<Point> GetValidTargets(List<Point> board, Point point)
        {
            List<Point> boardPoints = base.GetValidTargets(board, point);
            List<Point> excludingLatestRows = boardPoints?.Where(p => p.x > this.raptureRow.Value + this.offset).ToList();
            this.ValidPositions = excludingLatestRows?.Where(currentPoint => !this.Unit.UnitsMap.Contains(currentPoint)).ToList();
            phantasmSummon?.Play(this.transform.position);
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

            PlacementEffects placement = new PlacementEffects();
            StartCoroutine(placement.JumpCoroutine(this.Unit, position));
        }
    }
}