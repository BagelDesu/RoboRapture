//-----------------------------------------------------------------------
// <copyright file="Particle.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using System.Collections.Generic;
    using UnityEngine;

    public class Particle : MonoBehaviour
    {
        private Point point;

        public void SetPoint(Point point)
        {
            this.point = point;
        }

        private void Awake()
        {
            Board.OnRowSink += RaptureMoving;
        }

        private void OnDisable()
        {
            Board.OnRowSink -= RaptureMoving;
        }

        private void RaptureMoving(List<Point> row)
        {
            if (row == null || row.Count == 0 || point.x > row[0].x)
            {
                return;
            }

            Destroy(this.gameObject);
        }
    }
}