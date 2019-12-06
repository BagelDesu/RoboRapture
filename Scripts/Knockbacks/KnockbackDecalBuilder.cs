//-----------------------------------------------------------------------
// <copyright file="KnockbackDecalBuilder.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Knockbacks
{
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Patterns;
    using System.Collections.Generic;
    using UnityEngine;

    public class KnockbackDecalBuilder
    {
        private GameObject decal;

        private List<GameObject> decalInstances;

        public KnockbackDecalBuilder(GameObject decal)
        {
            this.decal = decal;
            this.decalInstances = new List<GameObject>();
        }

        public void Instanciate(Point position, CardinalDirections direction)
        {
            if (direction == CardinalDirections.Center)
            {
                return;
            }

            GameObject instance = Instanciate(position);
            Vector3 vectorPosition = instance.transform.position + GetPositionOffset(direction, 1.05f);
            instance.transform.position = vectorPosition;
            instance.transform.rotation = RotationHelper.GetRotation(direction, 90f, 0f);
        }

        public void DestroyInstances()
        {
            this.decalInstances?.ForEach(i => MonoBehaviour.Destroy(i));
        }

        private GameObject Instanciate(Point position)
        {
            Vector3 vector = PointConverter.ToVector(position);
            GameObject instance = MonoBehaviour.Instantiate(this.decal, vector, this.decal.transform.rotation);
            this.decalInstances?.Add(instance);
            return instance;
        }

        private Vector3 GetPositionOffset(CardinalDirections direction, float yOffset)
        {
            float offset = 0.5f;

            switch (direction)
            {
                case CardinalDirections.North:
                    return new Vector3(0, yOffset, offset);
                case CardinalDirections.NorthEast:
                    return new Vector3(offset, yOffset, offset);
                case CardinalDirections.NorthWest:
                    return new Vector3(-offset, yOffset, offset);
                case CardinalDirections.East:
                    return new Vector3(offset, yOffset, 0);
                case CardinalDirections.SouthEast:
                    return new Vector3(offset, yOffset, -offset);
                case CardinalDirections.SouthWest:
                    return new Vector3(-offset, yOffset, -offset);
                case CardinalDirections.West:
                    return new Vector3(0, yOffset, -offset);
                case CardinalDirections.South:
                    return new Vector3(0, yOffset, -offset);
                default:
                    return new Vector3(0, -yOffset, 0);
            }
        }
    }
}
