//-----------------------------------------------------------------------
// <copyright file="DecalBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using UnityEngine;

    public class DecalBuilder
    {
        private GameObject decal;

        private List<GameObject> decalInstances;

        public DecalBuilder(GameObject decal)
        {
            this.decal = decal;
            this.decalInstances = new List<GameObject>();
        }

        public void Instanciate(List<Point> positions)
        {
            positions?.ForEach(p => this.Instanciate(p));
        }

        public void DestroyInstances()
        {
            this.decalInstances?.ForEach(i => MonoBehaviour.Destroy(i));
        }

        private void Instanciate(Point position)
        {
            Vector3 vector = PointConverter.ToVector(position);
            vector.y = 1.02f;
            GameObject instance = MonoBehaviour.Instantiate(this.decal, vector, this.decal.transform.rotation);
            this.decalInstances?.Add(instance);
        }
    }
}