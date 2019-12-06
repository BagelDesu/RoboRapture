//-----------------------------------------------------------------------
// <copyright file="UnitDyingListener.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Environment
{
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Units;
    using UnityEngine;

    public class UnitDyingListener : MonoBehaviour
    {
        private void OnEnable()
        {
            EnemyUnit.EnemyDied += EnemyDied;
        }

        private void OnDisable()
        {
            EnemyUnit.EnemyDied -= EnemyDied;
        }

        private void EnemyDied(Point point)
        {
            Point p = new Point(point.x, 0, point.z);
            bool removed = EnvironmentManager.EnemyCollection.Remove(p);
            Logcat.I($"Unregistering unit from environment manager {p}, successfully? {removed}");
        }
    }
}