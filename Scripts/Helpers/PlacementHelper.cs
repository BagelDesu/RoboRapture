//-----------------------------------------------------------------------
// <copyright file="PlacementHelper.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.Helpers
{ 
    using Edu.Vfs.RoboRapture.Converters;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Units;
    using Edu.Vfs.RoboRapture.Validators;
    using System.Collections;
    using UnityEngine;

    public class PlacementHelper
    {
        public static Unit Add(Transform parent, Unit unit, Point point, IValidator validator)
        {
            if (unit == null || !validator.IsValid())
            {
                Logcat.I($"Invalid position {point}");
                return null;
            }

            Unit instance = MonoBehaviour.Instantiate(unit, new Vector3((float)point.x, point.y + unit.Height, (float)point.z), unit.transform.rotation);
            instance.SetPosition(point);
            instance.transform.parent = parent;
            instance.UnitsMap.Add(point, instance);
            return instance;
        }

        public static bool Move(Unit unit, Point point, IValidator validator)
        {
            if (unit == null || validator == null || !validator.IsValid())
            {
                return false;
            }

            unit.transform.position = new Vector3(point.x, point.y + unit.Height, point.z);
            unit.UnitsMap.Move(unit, point);
            unit.SetPosition(point);

            return true;
        }

        public static IEnumerator FallingMovement(Unit unit, Vector3 initialPosition, Vector3 finalPosition, float time)
        {
            if (unit == null)
            {
                yield return null;
            }

            float elapsedTime = 0;
            unit.transform.position = initialPosition;

            while (elapsedTime < time)
            {
                float yPosition = Mathf.Lerp(unit.transform.position.y, finalPosition.y, elapsedTime / time);
                if (yPosition < unit.Height)
                {
                    yPosition = unit.Height;

                }

                unit.transform.position = new Vector3(unit.transform.position.x, yPosition, unit.transform.position.z);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            PlacementHelper.Move(unit, PointConverter.ToPoint(finalPosition), new MovementActionValidator());
            yield return null;
        }

        public static bool Remove(Unit unit, IValidator validator)
        {
            if (unit == null || validator == null || !validator.IsValid())
            {
                Logcat.I(null, "unable to remove");
                return false;
            }

            unit?.Health.DeadByOneHit();
            unit?.GetComponent<EnemyUnit>()?.NotifyDyingEvent();
            unit?.GetComponent<PlayerUnit>()?.NotifyDyingEvent();
            unit?.UnitsMap.Remove(unit.GetPosition());
            MonoBehaviour.Destroy(unit.gameObject);
            return true;
        }
    }
}