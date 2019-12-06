//-----------------------------------------------------------------------
// <copyright file="KilledByRapture.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Units
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using System.Collections.Generic;
    using UnityEngine;

    public class KilledByRapture : MonoBehaviour
    {
        private Unit unit;

        private void OnEnable()
        {
            unit = GetComponent<Unit>();
            Board.OnRowSink += RaptureMoving;
        }

        private void OnDisable()
        {
            Board.OnRowSink -= RaptureMoving;
        }

        private void RaptureMoving(List<Point> row)
        {
            Logcat.I(this, $"Unit {unit.GetPosition()} Rapture moving {row[0].x}");
            if (row == null || row.Count == 0 || unit == null || unit.GetPosition().x > row[0].x)
            {
                return;
            }

            this.unit.Health.DeadByRapture();
        }
    }
}