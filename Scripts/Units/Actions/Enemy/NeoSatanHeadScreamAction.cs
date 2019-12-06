//-----------------------------------------------------------------------
// <copyright file="NeoSatanHeadScreamAction.cs" company="VFS">
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
    using UnityEngine;

    public class NeoSatanHeadScreamAction : SkillAction
    {
        [SerializeField]
        private int mountainsToSpawn = 7;

        [SerializeField]
        private RefInt raptureRow;

        private int offset = 0;

        private System.Random random = new System.Random();

        private NeoSatanBehaviour neoSatanBehaviour;

        public override List<Point> GetValidTargets(List<Point> board, Point position)
        {
            Logcat.I(this, $"NeoSatanHeadScreamAction rapture {raptureRow.Value}");
            neoSatanBehaviour = GetComponentInParent<NeoSatanBehaviour>();
            this.ValidPositions = base.GetValidTargets(board, position);
            List<Point> excludingUnits = this.ValidPositions.Where(currentPoint => !this.UnitsMap.Contains(currentPoint)).ToList();
            List<Point> excludingLegs = excludingUnits?.Where(p => !neoSatanBehaviour.LegsPositions.Contains(p)).ToList();
            List<Point> excludingLatestRows = excludingLegs?.Where(p => p.x > this.raptureRow.Value + this.offset).ToList();
            this.ValidPositions = excludingLatestRows;
            return this.ValidPositions;
        }

        public new void Execute(Point point)
        {
            SkillActionFX?.Play(this.transform.position);
        }

        public override void Execute()
        {
            this.SpawnMountains();
        }

        private void SpawnMountains()
        {
            for (int i = 0; i < this.mountainsToSpawn; i++)
            {
                Point position = this.GetRandomPosition(this.ValidPositions);
                if (position == default)
                {
                    return;
                }

                Point point = position;
                this.Execute(point);
                BoardController.EnvManager.SpawnMountain(point);
            }
        }

        private Point GetRandomPosition(List<Point> positionsList)
        {
            if (positionsList == null || positionsList.Count == 0)
            {
                return default;
            }

            int randomIndex = this.random.Next(positionsList.Count);
            Point position = positionsList[randomIndex];
            positionsList.Remove(position);
            return position;
        }
    }
}